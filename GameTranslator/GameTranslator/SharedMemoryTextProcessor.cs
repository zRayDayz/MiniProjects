using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{

    class SharedMemoryTextProcessor
    {
        const string SHARED_MEMORY_NAME = "SharedMemoryForSubtitlesStrs";   // в дальнейшем константу можно вынести в UI для возможности выбора имени пользователю
        const int INIT_SHARED_MEMORY_SIZE = 32768;  // не играет роли, если Shared memory открывается, а не создается (в моем случае именно так и происходит, т.к. создает Shared memory Cheat Engine)

        long openedSharedMemorySize;            // если Shared memory открывается, сюда положится ее реальный размер (указанный в Cheat Engine)
        MemoryMappedFile memoryMappedFile;
        MemoryMappedViewAccessor accessor;

        BackgroundWorker backgroundWorker;
        BaseTranslatorProcessor translatorBootstrapper;

        byte[] tempBytesOfStr = new byte[64];

        public SharedMemoryTextProcessor()
        {            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        public bool StartTextProcessor(BaseTranslatorProcessor translatorBootstrapper)
        {
            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show("Runtime Translator в процессе завершения работы, требуется дождаться его завершения перед новым запуском.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            memoryMappedFile = MemoryMappedFile.CreateOrOpen(SHARED_MEMORY_NAME, INIT_SHARED_MEMORY_SIZE);
            accessor = memoryMappedFile.CreateViewAccessor();
            openedSharedMemorySize = accessor.Capacity;

            this.translatorBootstrapper = translatorBootstrapper;
            backgroundWorker.RunWorkerAsync();
            return true;
        }

        public bool StopTextProcessor()
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                return true;
            }
            return false;
        }

        void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            memoryMappedFile.Dispose();
            accessor.Dispose();

            if (e.Error != null)
            {
                MessageBox.Show("Произошла ошибка в Background worker-е: " + Environment.NewLine + e.Error.ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
     
        void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is List<string> listOfLines)
            {              
                translatorBootstrapper.TranslateAndAppendTextInTranslationWindow(listOfLines);
                ListOfStringPool.Instance.ReturnObject(listOfLines, true);
            }
            
        }

        readonly int mainDataInfoStructSize = 16;
        struct MainSharedMemoryDataInfo
        {
            public int lockVar;
            public int counter;
            public int maxCounter;
            public int reserveInt32;
        }
        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {            
            int previousIndex = 0;
            int maxCountOfPointers = -1;
            List<string> listOfLines = new List<string>(32);

            while (true)
            {
                if (backgroundWorker.CancellationPending) return;

                accessor.Read(0, out MainSharedMemoryDataInfo mainInfo);

                if (mainInfo.lockVar == 1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                int counter = mainInfo.counter;
                if (counter == 0 || counter == previousIndex)
                {
                    Thread.Sleep(1);
                    continue;
                }

                long pointersArrBeginningOffset = mainDataInfoStructSize;

                int indexOfLastAddedStringToRead = counter - 1;

                if (indexOfLastAddedStringToRead < previousIndex)
                {
                    if (maxCountOfPointers == -1)
                    {
                        accessor.Read(8, out int tempMaxCountOfPointers);
                        maxCountOfPointers = tempMaxCountOfPointers;
                    }

                    ReadAllNewStringSinceLastRead(maxCountOfPointers, ref previousIndex, pointersArrBeginningOffset, ref listOfLines);
                    previousIndex = 0;
                }

                ReadAllNewStringSinceLastRead(counter, ref previousIndex, pointersArrBeginningOffset, ref listOfLines);

                var tempListOfLines = ListOfStringPool.Instance.GetObject();
                tempListOfLines.AddRange(listOfLines);
                backgroundWorker.ReportProgress(0, tempListOfLines);
                listOfLines.Clear();
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ReadAllNewStringSinceLastRead(int count, ref int startIndex, long pointersArrBeginningOffset, ref List<string> listOfLines)
        {
            while (startIndex < count)
            {
                long pointersArrOffset = pointersArrBeginningOffset + startIndex * 8;
                accessor.Read(pointersArrOffset, out long offsetOfStringLength);

                if (offsetOfStringLength == 0)
                {
                    startIndex = 0;
                    return;
                }

                string finalString = ReadLineByOffset(offsetOfStringLength);
                listOfLines.Add(finalString);

                startIndex++;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string ReadLineByOffset(long offsetOfStringLength)
        {
            accessor.Read(offsetOfStringLength, out int strLength);

            // Первыми 4 байтами в строке являются ее размер
            if(offsetOfStringLength + 4 + strLength > openedSharedMemorySize)
            {
                throw new BadReadedStringLength(offsetOfStringLength, strLength);
            }

            if (tempBytesOfStr.Length < strLength) Array.Resize(ref tempBytesOfStr, strLength);
            accessor.ReadArray(offsetOfStringLength + 4, tempBytesOfStr, 0, strLength);
            string finalStr = Encoding.Unicode.GetString(tempBytesOfStr, 0, strLength);

            return finalStr;
        }

    }

    class BadReadedStringLength : Exception
    {
        const string ERROR_MESSAGE = "Кастомный OutOfMemoryException. Прочитанная длина строки оказалась слишком большой. Вероятно ошибка синхронизации.";
        public long OffsetOfStringLength { get; private set; }
        public int ReadedStrLength { get; private set; }
        public BadReadedStringLength(long offsetOfStringLength, int readedStrLength) : base(ERROR_MESSAGE)
        {
            OffsetOfStringLength = offsetOfStringLength;
            ReadedStrLength = readedStrLength;
        }

        public override string ToString()
        {
            string mainParams = "Параметры исключения: " + Environment.NewLine + "Оффсет (от начала Shared memory) по которому читалась длина строки: " + OffsetOfStringLength + Environment.NewLine +
                "Прочитанная (ошибочная) длина строки: " + ReadedStrLength + Environment.NewLine;
            return mainParams + base.ToString();
        }

    }


}

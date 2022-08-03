using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{

    public class TranslatorProcessor : BaseTranslatorProcessor
    {
        MainForm mainForm;
        SharedMemoryTextProcessor textProcessor;
        TranslationForm translationForm;      

        public override TranslationForm TranslationWindow 
        {
            get 
            {
                if (IsTranslationWindowDisposing) CreateNewTranslationForm();
                return translationForm;
            }
        }
        public bool IsTranslationWindowDisposing
        {
            get
            {
                if (translationForm.IsDisposed || translationForm.Disposing) return true;
                else return false;
            }
        }

        readonly Encoding defaultTextFileEncoding = Encoding.UTF8;
        readonly Encoding targetEncoding = Encoding.Unicode;
        TranslatorProcessorState currentState;
               
        string pathForOriginalStringsFile = "";
        string pathForTranslatedStringsFile = "";
        Dictionary<string, string> translationDict;               

        public TranslatorProcessor()
        {
            textProcessor = new SharedMemoryTextProcessor();

            CreateNewTranslationForm();

            currentState = TranslatorProcessorState.BeginningState;

            this.mainForm = new MainForm(this);
            mainForm.Show();
        }

        void CreateNewTranslationForm()
        {
            translationForm = new TranslationForm(this);
            translationForm.MinimumSize = new System.Drawing.Size(125, 50);

            translationForm.Show();
        }

        public override void OnFormClosed(Form form, FormClosedEventArgs e)
        {
            if(form == mainForm) Application.Exit();
            else if(form == translationForm) StopTranslator();
        }

        public override void InitOriginalStringsFilePath(string filePath)
        {
            pathForOriginalStringsFile = filePath;
        }

        public override void InitTranslatedStringsFilePath(string filePath)
        {
            pathForTranslatedStringsFile = filePath;
        }
        public override bool GenerateTranslationData()
        {

            if (pathForOriginalStringsFile == "" || pathForTranslatedStringsFile == "") 
            {
                MessageBox.Show("Нужно выбрать оба файла, с ориг. строками и строками переведенными.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (translationDict != null)
            {
                var result = MessageBox.Show("Перевод ранее уже был инициализирован, запустить инициализацию заного?", "Initialization.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                if (result == DialogResult.Cancel) return false;
            }
            

            translationDict = new Dictionary<string, string>(16384);

            using (StreamReader strReaderOrig = new StreamReader(pathForOriginalStringsFile, defaultTextFileEncoding))
            using (StreamReader strReaderTransl  = new StreamReader(pathForTranslatedStringsFile, defaultTextFileEncoding))
            {
                byte[] arrOfStrBytes = new byte[256];
                Encoding streamEncoding = strReaderOrig.CurrentEncoding;
                while (!strReaderOrig.EndOfStream)
                {
                    if (strReaderTransl.EndOfStream)
                    {
                        MessageBox.Show("В оригинальном файле большее кол-во строк чем в файле перевода, требуется дабы у каждой строки 1-го файла была соотв. строка перевода во 2-м файле.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    string origLine = strReaderOrig.ReadLine();
                    string trLine = strReaderTransl.ReadLine();

                    int strBytesCount = streamEncoding.GetByteCount(origLine);
                    int origStrLength = origLine.Length;
                    if (origStrLength > arrOfStrBytes.Length) Array.Resize(ref arrOfStrBytes, origStrLength);

                    #region Конвертирование строки из кодировки текстового файла в кодировку текста целевой игры (в данном случае Psychonauts 2)
                    streamEncoding.GetBytes(origLine, 0, origStrLength, arrOfStrBytes, 0);

                    if (arrOfStrBytes[strBytesCount - 1] == ' ') strBytesCount--;   // trim одного пробела в конце, нюанс работы с текстом из Psychonauts 2

                    byte[] convertedOrigStrBytes = Encoding.Convert(streamEncoding, targetEncoding, arrOfStrBytes, 0, strBytesCount);
                    var encodedOrigStr = targetEncoding.GetString(convertedOrigStrBytes, 0, convertedOrigStrBytes.Length);
                    #endregion

                    if (!translationDict.ContainsKey(encodedOrigStr))
                    {
                        translationDict.Add(encodedOrigStr, trLine);
                    }
                }
                if (!strReaderTransl.EndOfStream)
                {
                    MessageBox.Show("В файле перевода большее кол-во строк чем в оригинальном файле, требуется дабы у каждой строки 1-го файла была соотв. строка перевода во 2-м файле.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }


            }

            currentState = TranslatorProcessorState.TranslationDictionaryInitialized;
            return true;
        }

        public override bool RunTranslator()
        {
            if (currentState < TranslatorProcessorState.TranslationDictionaryInitialized)
            {
                MessageBox.Show("Требуется сначала проинициализировать данные для перевода.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            bool result = textProcessor.StartTextProcessor(this);
            if (result)
            {
                currentState = TranslatorProcessorState.TranslatorIsRunning;
                return true;
            }
            return false;
        }

        public override bool StopTranslator()
        {
            bool result = textProcessor.StopTextProcessor();
            if (result)
            {
                currentState = TranslatorProcessorState.TranslationDictionaryInitialized;
                mainForm.UpdateTranslatorStatus(false);
                return true;
            }
            return false;
        }

        public override void TranslateAndAppendTextInTranslationWindow(List<string> listOfLines)
        {
            if (IsTranslationWindowDisposing)
            {
                StopTranslator();
                return;
            }

            for (int i = 0; i < listOfLines.Count; i++)
            {
                string text = listOfLines[i];
                if (text.Length == 1)
                {
                    var outStr = text + "\t\t- Перевода нет, т.к. строка в один символ";
                    Console.WriteLine(outStr);
                }
                else if (translationDict.TryGetValue(text, out string translatedStr))
                {
                    var outStr = "=> " + text + "\n     " + translatedStr;
                    translationForm.PrintNewLine(outStr);
                }
                else
                {
                    string outStr;
                    if (text.Length < 100)
                    {
                        outStr = $"{text} \t- перевод не был найден.";                        
                    }
                    else
                    {
                        outStr = text.Substring(0, 100);
                        outStr = $"{outStr} \t- перевод не был найден. Строка слишком большая поэтому была обрезана.";
                    }
                    Console.WriteLine(outStr);
                }
            }           
        }

        public override void ToggleTranslationWindowTransparency()
        {
            TranslationWindow.ToggleTransparencyState();
        }

    }

    enum TranslatorProcessorState
    {
        BeginningState,
        TranslationDictionaryInitialized,
        TranslatorIsRunning
    }

    public abstract class BaseTranslatorProcessor : IOnFormClosedEventSubscriber
    {
        public abstract TranslationForm TranslationWindow { get; }
        public abstract void InitOriginalStringsFilePath(string filePath);
        public abstract void InitTranslatedStringsFilePath(string filePath);
        public abstract bool GenerateTranslationData();
        public abstract bool RunTranslator();
        public abstract bool StopTranslator();
        public abstract void TranslateAndAppendTextInTranslationWindow(List<string> listOfLines);
        public abstract void ToggleTranslationWindowTransparency();
        public abstract void OnFormClosed(Form form, FormClosedEventArgs e);
    }
    public interface IOnFormClosedEventSubscriber
    {
        void OnFormClosed(Form form, FormClosedEventArgs e);
    }

}

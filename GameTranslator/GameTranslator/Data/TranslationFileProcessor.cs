using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{
    public abstract class BaseTranslationFileProcessor
    {
        public abstract bool GenerateTranslationData(string pathForOriginalStringsFile, string pathForTranslatedStringsFile, Encoding targetEncoding);
        public abstract bool TryGetTranslation(string origStr, out string translatedStr);
    }

    class TranslationFileProcessor : BaseTranslationFileProcessor
    {
        Dictionary<string, string> translationData;

        public override bool GenerateTranslationData(string pathForOriginalStringsFile, string pathForTranslatedStringsFile, Encoding targetEncoding)
        {
            if (translationData != null)
            {
                var result = MessageBox.Show("Перевод ранее уже был инициализирован, запустить инициализацию заного?", "Initialization.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                if (result == DialogResult.Cancel) return false;
            }

            translationData = new Dictionary<string, string>(16384);

            using (StreamReader strReaderOrig = new StreamReader(pathForOriginalStringsFile, Encoding.UTF8))
            using (StreamReader strReaderTransl = new StreamReader(pathForTranslatedStringsFile, Encoding.UTF8))
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

                    streamEncoding.GetBytes(origLine, 0, origStrLength, arrOfStrBytes, 0);

                    if (arrOfStrBytes[strBytesCount - 1] == ' ') strBytesCount--;   // trim одного пробела в конце, нюанс работы с текстом из Psychonauts 2

                    byte[] convertedOrigStrBytes = Encoding.Convert(streamEncoding, targetEncoding, arrOfStrBytes, 0, strBytesCount);
                    var encodedOrigStr = targetEncoding.GetString(convertedOrigStrBytes, 0, convertedOrigStrBytes.Length);

                    if (!translationData.ContainsKey(encodedOrigStr))
                    {
                        translationData.Add(encodedOrigStr, trLine);
                    }
                }
                if (!strReaderTransl.EndOfStream)
                {
                    MessageBox.Show("В файле перевода большее кол-во строк чем в оригинальном файле, требуется чтобы у каждой строки 1-го файла была соотв. строка перевода во 2-м файле.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            return true;
        }

        public override bool TryGetTranslation(string origStr, out string translatedStr)
        {
            return translationData.TryGetValue(origStr, out translatedStr);
        }
    }
}

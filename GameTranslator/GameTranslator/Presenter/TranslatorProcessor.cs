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
    enum TranslatorProcessorState
    {
        BeginningState,
        TranslationDataInitialized,
        TranslatorIsRunning
    }

    public abstract class BaseTranslatorProcessor
    {
        public abstract bool GenerateTranslationData(string pathForOriginalStringsFile, string pathForTranslatedStringsFile);
        public abstract bool RunTranslator();
        public abstract bool StopTranslator();
        public abstract void TranslateAndAppendTextInTranslationWindow(List<string> listOfLines);
        public abstract void OnTranslationFormClosed(Form form, FormClosedEventArgs e);
    }

    public class TranslatorProcessor : BaseTranslatorProcessor
    {
        BaseViewController baseViewController;
        BaseTranslationFileProcessor baseTranslationFileProcessor;
        SharedMemoryTextProcessor textProcessor;

        Encoding targetEncoding;
        TranslatorProcessorState currentState;
        IntPtr gameWindowPtr;

        public TranslatorProcessor(BaseViewController baseViewController, BaseTranslationFileProcessor baseTranslationFileProcessor)
        {
            this.baseViewController = baseViewController;
            baseViewController.Initialization(this);
            this.baseTranslationFileProcessor = baseTranslationFileProcessor;
            textProcessor = new SharedMemoryTextProcessor();

            targetEncoding = Encoding.Unicode;
            currentState = TranslatorProcessorState.BeginningState;
        }

        public override void OnTranslationFormClosed(Form form, FormClosedEventArgs e)
        {
            StopTranslator();
        }

        public override bool GenerateTranslationData(string pathForOriginalStringsFile, string pathForTranslatedStringsFile)
        {
            bool result = baseTranslationFileProcessor.GenerateTranslationData(pathForOriginalStringsFile, pathForTranslatedStringsFile, targetEncoding);
            if(result == true)
            {
                currentState = TranslatorProcessorState.TranslationDataInitialized;
            }
            return result;
        }

        public override bool RunTranslator()
        {
            if (currentState < TranslatorProcessorState.TranslationDataInitialized)
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
                currentState = TranslatorProcessorState.TranslationDataInitialized;
                baseViewController.UpdateTranslatorStatus(false);
                return true;
            }
            return false;
        }

        public override void TranslateAndAppendTextInTranslationWindow(List<string> listOfLines)
        {
            for (int i = 0; i < listOfLines.Count; i++)
            {
                string text = listOfLines[i];
                if (text.Length == 1)
                {
                    var outStr = text + "\t\t- Перевода нет, т.к. строка в один символ";
                    Console.WriteLine(outStr);
                }
                else if (baseTranslationFileProcessor.TryGetTranslation(text, out string translatedStr))
                {
                    var outStr = "=> " + text + "\n     " + translatedStr;
                    baseViewController.PrintNewLine(outStr);
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

    }

}

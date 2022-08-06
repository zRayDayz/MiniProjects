﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{
    static class Program
    {
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var viewController = new ViewController();
            var translationFileProcessor = new TranslationFileProcessor();
            var translatorProcessor = new TranslatorProcessor(viewController, translationFileProcessor);

            Application.Run();
        }
    }

}

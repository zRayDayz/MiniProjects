using GameTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{
    public abstract class BaseViewController
    {
        public abstract TranslationForm TranslationWindow { get; }
        public abstract void Initialization(BaseTranslatorProcessor baseTranslator);
        public abstract void OnFormClosed(Form form, FormClosedEventArgs e);
        public abstract void UpdateTranslatorStatus(bool isWorking);
        public abstract void PrintNewLine(string line);
        public abstract void ToggleTranslationWindowTransparency();
    }

    class ViewController : BaseViewController
    {
        BaseTranslatorProcessor baseTranslator;
        MainForm mainForm;
        TranslationForm translationForm;

        public override TranslationForm TranslationWindow
        {
            get
            {
                if (CommonHelpers.IsFormDisposing(translationForm)) CreateNewTranslationForm();
                return translationForm;
            }
        }

        public ViewController() {  }

        public override void Initialization(BaseTranslatorProcessor baseTranslator)
        {
            this.baseTranslator = baseTranslator;

            if (translationForm == null) CreateNewTranslationForm();

            this.mainForm = new MainForm(this, baseTranslator);
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
            if (form == mainForm) Application.Exit();
            else if (form == translationForm) baseTranslator.OnTranslationFormClosed(form, e);
        }

        public override void UpdateTranslatorStatus(bool isWorking)
        {
            mainForm.UpdateTranslatorStatus(false);
        }

        public override void PrintNewLine(string line)
        {
            TranslationWindow.PrintNewLine(line);
        }

        public override void ToggleTranslationWindowTransparency()
        {
            TranslationWindow.ToggleTransparencyState();
        }

    }
}

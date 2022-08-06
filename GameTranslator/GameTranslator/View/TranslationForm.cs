using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{
    public partial class TranslationForm : Form
    {
        Color formColorForTransparency;
        uint formColorUInt32;
        Color initFormBackColor;

        bool isClickThroughAble = false;
        uint backupWindowLong;

        BaseViewController baseViewController;

        public TranslationForm(BaseViewController viewFacade)
        {
            InitializeComponent();

            initFormBackColor = this.BackColor;
            this.SizeGripStyle = SizeGripStyle.Show;

            formColorForTransparency = Color.FromArgb(255, 0, 0);
            formColorUInt32 = (uint)formColorForTransparency.ToArgb();
            this.TransparencyKey = formColorForTransparency;

            backupWindowLong = WinAPIWrapper.GetWindowLong(Handle, -20);

            this.baseViewController = viewFacade;

        }

        private void TranslationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            baseViewController.OnFormClosed(this, e);
        }

        public void ToggleTransparencyState()
        {
          
            if (isClickThroughAble)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                var wndLong = backupWindowLong | WinAPIWrapper.WS_EX_LAYERED;
                WinAPIWrapper.SetWindowLong(this.Handle, WinAPIWrapper.GWL_EXSTYLE, (int)wndLong);
                this.BackColor = initFormBackColor;
                this.SizeGripStyle = SizeGripStyle.Show;

                this.TopMost = false;
                this.Opacity = 1; 
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                var wndLong = backupWindowLong | WinAPIWrapper.WS_EX_LAYERED | WinAPIWrapper.WS_EX_TRANSPARENT;
                WinAPIWrapper.SetWindowLong(this.Handle, WinAPIWrapper.GWL_EXSTYLE, (int)wndLong);
                this.BackColor = formColorForTransparency;
                this.SizeGripStyle = SizeGripStyle.Hide;

                this.TopMost = true;
                this.Opacity = 0.8;
            }
            isClickThroughAble = !isClickThroughAble;
        }

        public void PrintNewLine(string line)
        {
            translationRichTextBox.AppendText(line + Environment.NewLine);
            translationRichTextBox.ScrollToCaret();
            this.Refresh();
        }
      
    }
}

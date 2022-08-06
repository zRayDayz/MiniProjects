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
    public partial class MainForm : Form
    {
        BaseViewController baseViewController;
        BaseTranslatorProcessor baseTranslator;

        Form translationWndPosHandleForm;
        Form translationWndSizeHandleForm;

        MainForm()
        {
            InitializeComponent();
        }
        
        public MainForm(BaseViewController baseViewController, BaseTranslatorProcessor baseTranslator)
        {
            InitializeComponent();

            this.baseViewController = baseViewController;
            this.baseTranslator = baseTranslator;

            toolTip1.Active = false;

            CreateNewTranslationWndPosHandleForm();
            CreateNewTranslationWndSizeHandleForm();
        }

        void CreateNewTranslationWndPosHandleForm()
        {
            translationWndPosHandleForm = new Form();
            translationWndPosHandleForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            translationWndPosHandleForm.Size = new Size(15, 15);
            translationWndPosHandleForm.ControlBox = false;
            translationWndPosHandleForm.MouseDown += translationWndPosHandleForm_MouseDown;
            translationWndPosHandleForm.MouseMove += translationWndPosHandleForm_MouseMove;
            var trWindowPos = baseViewController.TranslationWindow.Location;
            translationWndPosHandleForm.Location = trWindowPos;
        }
        void CreateNewTranslationWndSizeHandleForm()
        {
            translationWndSizeHandleForm = new Form();
            translationWndSizeHandleForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            translationWndSizeHandleForm.Size = new Size(15, 15);
            translationWndSizeHandleForm.ControlBox = false;
            translationWndSizeHandleForm.MouseDown += translationWndSizeHandleForm_MouseDown;
            translationWndSizeHandleForm.MouseMove += translationWndSizeHandleForm_MouseMove;
            var trWndPos = baseViewController.TranslationWindow.Location;
            var trWndSize = baseViewController.TranslationWindow.Size;
            var sizeHandlePos = new Point(trWndPos.X + trWndSize.Width, trWndPos.Y + trWndSize.Height);
            translationWndSizeHandleForm.Location = sizeHandlePos;
        }

        public void UpdateTranslatorStatus(bool isWorking)
        {
            if (isWorking) translatorStatusLabel.Text = "Is Working";
            else translatorStatusLabel.Text = "Is Stopped";
        }

        private void openOrigFileBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog1.FileName;
            origFilePathTextBox.Text = filePath;
        }

        private void openTranslationFileBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog1.FileName;
            translFilePathTextBox.Text = filePath;
        }

        private void initTranslationDataBtn_Click(object sender, EventArgs e)
        {
            string origFilePathText = origFilePathTextBox.Text;
            string translFilePathText = translFilePathTextBox.Text;
            if (origFilePathText == "" || translFilePathText == "")
            {
                MessageBox.Show("Нужно выбрать оба файла, с ориг. строками и строками переведенными.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bool result = baseTranslator.GenerateTranslationData(origFilePathTextBox.Text, translFilePathTextBox.Text);
            if (result) translatorInitStatusLabel.Text = "Initialized";
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (baseTranslator.RunTranslator())
            {
                UpdateTranslatorStatus(true);
            }
        }
        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (baseTranslator.StopTranslator())
            {
                UpdateTranslatorStatus(false);
            }
        }

        private void setTranslationWindowTransparency_Click(object sender, EventArgs e)
        {
            baseViewController.ToggleTranslationWindowTransparency();
        }

        private void toggleTooltipBtn_Click(object sender, EventArgs e)
        {
            var toogleBtn = sender as Control;
            var mousePos = e as MouseEventArgs;
            toolTip1.Active = !toolTip1.Active;
            toolTip1.Show(toolTip1.GetToolTip(toogleBtn), toogleBtn, mousePos.X, mousePos.Y, 15000);
        }

        private void toggleTooltipBtn_MouseLeave(object sender, EventArgs e)
        {
            var toogleBtn = sender as Control;
            toolTip1.Hide(toogleBtn);
        }

        private void togglePositionHandleBtn_Click(object sender, EventArgs e)
        {
            if (translationWndPosHandleForm.Disposing || translationWndPosHandleForm.IsDisposed) CreateNewTranslationWndPosHandleForm();           
            translationWndPosHandleForm.Visible = !translationWndPosHandleForm.Visible;
            var trWindowPos = baseViewController.TranslationWindow.Location;
            translationWndPosHandleForm.Location = trWindowPos;
            translationWndPosHandleForm.TopMost = true;
        }

        Point lastMouseLocation;
        private void translationWndPosHandleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMouseLocation = e.Location;
            }
        }

        private void translationWndPosHandleForm_MouseMove(object sender, MouseEventArgs e)
        {
            var posHandle = sender as Control;
            if (MouseButtons.Left == MouseButtons)
            {
                int xDiffr = e.X - lastMouseLocation.X;
                int yDiffr = e.Y - lastMouseLocation.Y;

                posHandle.Left = xDiffr + posHandle.Left;
                posHandle.Top = yDiffr + posHandle.Top;

                baseViewController.TranslationWindow.Location = posHandle.Location;

                if(translationWndSizeHandleForm != null)
                {
                    var newPos = translationWndSizeHandleForm.Location;
                    newPos.X += xDiffr;
                    newPos.Y += yDiffr;
                    translationWndSizeHandleForm.Location = newPos;
                }
            }
        }

        private void toggleSizeHandleBtn_Click(object sender, EventArgs e)
        {
            if (translationWndSizeHandleForm.Disposing || translationWndSizeHandleForm.IsDisposed) CreateNewTranslationWndPosHandleForm();
            translationWndSizeHandleForm.Visible = !translationWndSizeHandleForm.Visible;
            var trWndPos = baseViewController.TranslationWindow.Location;
            var trWndSize = baseViewController.TranslationWindow.Size;
            var sizeHandlePos = new Point(trWndPos.X + trWndSize.Width - 7, trWndPos.Y + trWndSize.Height - 100);
            translationWndSizeHandleForm.Location = sizeHandlePos;
            translationWndSizeHandleForm.TopMost = true;
        }

        private void translationWndSizeHandleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMouseLocation = e.Location;
            }
        }

        private void translationWndSizeHandleForm_MouseMove(object sender, MouseEventArgs e)
        {
            var sizeHandle = sender as Control;
            if (MouseButtons.Left == MouseButtons)
            {
                int xDiffr = e.X - lastMouseLocation.X;
                int yDiffr = e.Y - lastMouseLocation.Y;

                Size oldTrWndSize = baseViewController.TranslationWindow.Size;
                Size tempNewTrWndSize = new Size(oldTrWndSize.Width + xDiffr, oldTrWndSize.Height + yDiffr);

                baseViewController.TranslationWindow.Size = tempNewTrWndSize;

                Size newTrWndSize = baseViewController.TranslationWindow.Size;
                
                if (newTrWndSize.Width == oldTrWndSize.Width)
                {
                    xDiffr = 0;
                }
                if (newTrWndSize.Height == oldTrWndSize.Height)
                {
                    yDiffr = 0;
                }

                Point handlePos = sizeHandle.Location;
                sizeHandle.Location = new Point(handlePos.X + xDiffr, handlePos.Y + yDiffr);

                baseViewController.TranslationWindow.Refresh();
            }
            
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            baseViewController.OnFormClosed(this, e);
        }
    }
}

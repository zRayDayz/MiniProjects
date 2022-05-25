namespace GameTranslator
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chooseTranslationFileBtn = new System.Windows.Forms.Button();
            this.chooseOrigFileBtn = new System.Windows.Forms.Button();
            this.initTranslationDataBtn = new System.Windows.Forms.Button();
            this.setTranslationWindowTransparency = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toggleTooltipBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toggleSizeHandleBtn = new System.Windows.Forms.Button();
            this.togglePositionHandleBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.translatorStatusLabel = new System.Windows.Forms.Label();
            this.origFilePathTextBox = new System.Windows.Forms.TextBox();
            this.translFilePathTextBox = new System.Windows.Forms.TextBox();
            this.translatorInitStatusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(11, 141);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(92, 141);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(75, 23);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chooseTranslationFileBtn
            // 
            this.chooseTranslationFileBtn.Location = new System.Drawing.Point(6, 45);
            this.chooseTranslationFileBtn.Name = "chooseTranslationFileBtn";
            this.chooseTranslationFileBtn.Size = new System.Drawing.Size(119, 23);
            this.chooseTranslationFileBtn.TabIndex = 2;
            this.chooseTranslationFileBtn.Text = "Choose translation file";
            this.toolTip1.SetToolTip(this.chooseTranslationFileBtn, "Требуется выбрать файл с переведенным текстом в кодировке UTF-8.");
            this.chooseTranslationFileBtn.UseVisualStyleBackColor = true;
            this.chooseTranslationFileBtn.Click += new System.EventHandler(this.openTranslationFileBtn_Click);
            // 
            // chooseOrigFileBtn
            // 
            this.chooseOrigFileBtn.Location = new System.Drawing.Point(6, 19);
            this.chooseOrigFileBtn.Name = "chooseOrigFileBtn";
            this.chooseOrigFileBtn.Size = new System.Drawing.Size(104, 23);
            this.chooseOrigFileBtn.TabIndex = 3;
            this.chooseOrigFileBtn.Text = "Choose original file";
            this.toolTip1.SetToolTip(this.chooseOrigFileBtn, "Требуется выбрать файл с оригинальным текстом (английским) в кодировке UTF-8.");
            this.chooseOrigFileBtn.UseVisualStyleBackColor = true;
            this.chooseOrigFileBtn.Click += new System.EventHandler(this.openOrigFileBtn_Click);
            // 
            // initTranslationDataBtn
            // 
            this.initTranslationDataBtn.Location = new System.Drawing.Point(6, 74);
            this.initTranslationDataBtn.Name = "initTranslationDataBtn";
            this.initTranslationDataBtn.Size = new System.Drawing.Size(135, 23);
            this.initTranslationDataBtn.TabIndex = 4;
            this.initTranslationDataBtn.Text = "Initialize translation data";
            this.initTranslationDataBtn.UseVisualStyleBackColor = true;
            this.initTranslationDataBtn.Click += new System.EventHandler(this.initTranslationDataBtn_Click);
            // 
            // setTranslationWindowTransparency
            // 
            this.setTranslationWindowTransparency.Location = new System.Drawing.Point(117, 61);
            this.setTranslationWindowTransparency.Name = "setTranslationWindowTransparency";
            this.setTranslationWindowTransparency.Size = new System.Drawing.Size(75, 23);
            this.setTranslationWindowTransparency.TabIndex = 5;
            this.setTranslationWindowTransparency.Text = "On/Off";
            this.setTranslationWindowTransparency.UseVisualStyleBackColor = true;
            this.setTranslationWindowTransparency.Click += new System.EventHandler(this.setTranslationWindowTransparency_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Set translation window transparency";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 50;
            // 
            // toggleTooltipBtn
            // 
            this.toggleTooltipBtn.Location = new System.Drawing.Point(73, 19);
            this.toggleTooltipBtn.Name = "toggleTooltipBtn";
            this.toggleTooltipBtn.Size = new System.Drawing.Size(119, 23);
            this.toggleTooltipBtn.TabIndex = 7;
            this.toggleTooltipBtn.Text = "On/Off Bubble Tooltip";
            this.toolTip1.SetToolTip(this.toggleTooltipBtn, "Показывает всплывающую подсказку над различными кнопками/элементами управления.");
            this.toggleTooltipBtn.UseVisualStyleBackColor = true;
            this.toggleTooltipBtn.Click += new System.EventHandler(this.toggleTooltipBtn_Click);
            this.toggleTooltipBtn.MouseLeave += new System.EventHandler(this.toggleTooltipBtn_MouseLeave);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.toggleSizeHandleBtn);
            this.groupBox1.Controls.Add(this.togglePositionHandleBtn);
            this.groupBox1.Controls.Add(this.toggleTooltipBtn);
            this.groupBox1.Controls.Add(this.setTranslationWindowTransparency);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(510, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 169);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional controls";
            // 
            // toggleSizeHandleBtn
            // 
            this.toggleSizeHandleBtn.Location = new System.Drawing.Point(50, 119);
            this.toggleSizeHandleBtn.Name = "toggleSizeHandleBtn";
            this.toggleSizeHandleBtn.Size = new System.Drawing.Size(142, 23);
            this.toggleSizeHandleBtn.TabIndex = 9;
            this.toggleSizeHandleBtn.Text = "Show/hide size handle";
            this.toggleSizeHandleBtn.UseVisualStyleBackColor = true;
            this.toggleSizeHandleBtn.Click += new System.EventHandler(this.toggleSizeHandleBtn_Click);
            // 
            // togglePositionHandleBtn
            // 
            this.togglePositionHandleBtn.Location = new System.Drawing.Point(50, 90);
            this.togglePositionHandleBtn.Name = "togglePositionHandleBtn";
            this.togglePositionHandleBtn.Size = new System.Drawing.Size(142, 23);
            this.togglePositionHandleBtn.TabIndex = 8;
            this.togglePositionHandleBtn.Text = "Show/hide position handle";
            this.togglePositionHandleBtn.UseVisualStyleBackColor = true;
            this.togglePositionHandleBtn.Click += new System.EventHandler(this.togglePositionHandleBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Translator status:";
            // 
            // translatorStatusLabel
            // 
            this.translatorStatusLabel.AutoSize = true;
            this.translatorStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.translatorStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.translatorStatusLabel.Location = new System.Drawing.Point(101, 123);
            this.translatorStatusLabel.Name = "translatorStatusLabel";
            this.translatorStatusLabel.Size = new System.Drawing.Size(60, 15);
            this.translatorStatusLabel.TabIndex = 10;
            this.translatorStatusLabel.Text = "Is Stopped";
            // 
            // origFilePathTextBox
            // 
            this.origFilePathTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.origFilePathTextBox.Location = new System.Drawing.Point(131, 19);
            this.origFilePathTextBox.Name = "origFilePathTextBox";
            this.origFilePathTextBox.ReadOnly = true;
            this.origFilePathTextBox.Size = new System.Drawing.Size(100, 20);
            this.origFilePathTextBox.TabIndex = 11;
            // 
            // translFilePathTextBox
            // 
            this.translFilePathTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.translFilePathTextBox.Location = new System.Drawing.Point(131, 45);
            this.translFilePathTextBox.Name = "translFilePathTextBox";
            this.translFilePathTextBox.ReadOnly = true;
            this.translFilePathTextBox.Size = new System.Drawing.Size(100, 20);
            this.translFilePathTextBox.TabIndex = 12;
            // 
            // translatorInitStatusLabel
            // 
            this.translatorInitStatusLabel.AutoSize = true;
            this.translatorInitStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.translatorInitStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.translatorInitStatusLabel.Location = new System.Drawing.Point(147, 79);
            this.translatorInitStatusLabel.Name = "translatorInitStatusLabel";
            this.translatorInitStatusLabel.Size = new System.Drawing.Size(71, 15);
            this.translatorInitStatusLabel.TabIndex = 13;
            this.translatorInitStatusLabel.Text = "Not initialized";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chooseOrigFileBtn);
            this.groupBox2.Controls.Add(this.translatorInitStatusLabel);
            this.groupBox2.Controls.Add(this.chooseTranslationFileBtn);
            this.groupBox2.Controls.Add(this.translFilePathTextBox);
            this.groupBox2.Controls.Add(this.initTranslationDataBtn);
            this.groupBox2.Controls.Add(this.origFilePathTextBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(242, 108);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Initialization controls";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.translatorStatusLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button chooseTranslationFileBtn;
        private System.Windows.Forms.Button chooseOrigFileBtn;
        private System.Windows.Forms.Button initTranslationDataBtn;
        private System.Windows.Forms.Button setTranslationWindowTransparency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button toggleTooltipBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label translatorStatusLabel;
        private System.Windows.Forms.TextBox origFilePathTextBox;
        private System.Windows.Forms.TextBox translFilePathTextBox;
        private System.Windows.Forms.Label translatorInitStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button togglePositionHandleBtn;
        private System.Windows.Forms.Button toggleSizeHandleBtn;
    }
}


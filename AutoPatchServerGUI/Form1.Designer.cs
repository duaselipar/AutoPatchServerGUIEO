namespace AutoPatchServerGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            titleBarPanel = new Panel();
            titleLabel = new Label();
            themeBtn = new Button();
            minimizeBtn = new Button();
            closeBtn = new Button();
            groupBox1 = new GroupBox();
            webNumeric = new NumericUpDown();
            autoNumeric = new NumericUpDown();
            verNumeric = new NumericUpDown();
            hostBox = new TextBox();
            pathBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            consoleBox = new RichTextBox();
            autosBox = new CheckBox();
            statusValueLabel = new Label();
            serverTimeValueLabel = new Label();
            label6 = new Label();
            startBtn = new Button();
            groupBox2 = new GroupBox();
            button1 = new Button();
            titleBarPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)autoNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)verNumeric).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // titleBarPanel
            // 
            titleBarPanel.BackColor = SystemColors.Control;
            titleBarPanel.Controls.Add(titleLabel);
            titleBarPanel.Controls.Add(themeBtn);
            titleBarPanel.Controls.Add(minimizeBtn);
            titleBarPanel.Controls.Add(closeBtn);
            titleBarPanel.Dock = DockStyle.Top;
            titleBarPanel.Location = new Point(0, 0);
            titleBarPanel.Name = "titleBarPanel";
            titleBarPanel.Size = new Size(448, 34);
            titleBarPanel.TabIndex = 0;
            titleBarPanel.MouseDown += TitleBar_MouseDown;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            titleLabel.ForeColor = SystemColors.ControlText;
            titleLabel.Location = new Point(12, 10);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(130, 19);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Auto Patch Server";
            titleLabel.MouseDown += TitleBar_MouseDown;
            // 
            // themeBtn
            // 
            themeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            themeBtn.Cursor = Cursors.Hand;
            themeBtn.Font = new Font("Segoe UI Symbol", 11F, FontStyle.Bold);
            themeBtn.ForeColor = SystemColors.ControlText;
            themeBtn.Location = new Point(344, 0);
            themeBtn.Name = "themeBtn";
            themeBtn.Size = new Size(34, 34);
            themeBtn.TabIndex = 1;
            themeBtn.Text = "☀";
            themeBtn.UseVisualStyleBackColor = true;
            themeBtn.Click += themeBtn_Click;
            // 
            // minimizeBtn
            // 
            minimizeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minimizeBtn.Cursor = Cursors.Hand;
            minimizeBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            minimizeBtn.ForeColor = SystemColors.ControlText;
            minimizeBtn.Location = new Point(379, 0);
            minimizeBtn.Name = "minimizeBtn";
            minimizeBtn.Size = new Size(34, 34);
            minimizeBtn.TabIndex = 2;
            minimizeBtn.Text = "—";
            minimizeBtn.UseVisualStyleBackColor = true;
            minimizeBtn.Click += minimizeBtn_Click;
            // 
            // closeBtn
            // 
            closeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeBtn.Cursor = Cursors.Hand;
            closeBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            closeBtn.ForeColor = SystemColors.ControlText;
            closeBtn.Location = new Point(414, 0);
            closeBtn.Name = "closeBtn";
            closeBtn.Size = new Size(34, 34);
            closeBtn.TabIndex = 3;
            closeBtn.Text = "✕";
            closeBtn.UseVisualStyleBackColor = true;
            closeBtn.Click += closeBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(webNumeric);
            groupBox1.Controls.Add(autoNumeric);
            groupBox1.Controls.Add(verNumeric);
            groupBox1.Controls.Add(hostBox);
            groupBox1.Controls.Add(pathBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.ForeColor = SystemColors.ControlText;
            groupBox1.Location = new Point(7, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(429, 120);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Server Settings";
            // 
            // webNumeric
            // 
            webNumeric.Location = new Point(310, 25);
            webNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            webNumeric.Name = "webNumeric";
            webNumeric.Size = new Size(108, 23);
            webNumeric.TabIndex = 3;
            // 
            // autoNumeric
            // 
            autoNumeric.Location = new Point(109, 52);
            autoNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            autoNumeric.Name = "autoNumeric";
            autoNumeric.Size = new Size(109, 23);
            autoNumeric.TabIndex = 1;
            // 
            // verNumeric
            // 
            verNumeric.Location = new Point(109, 22);
            verNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            verNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            verNumeric.Name = "verNumeric";
            verNumeric.Size = new Size(109, 23);
            verNumeric.TabIndex = 0;
            verNumeric.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // hostBox
            // 
            hostBox.Location = new Point(109, 80);
            hostBox.Name = "hostBox";
            hostBox.Size = new Size(309, 23);
            hostBox.TabIndex = 4;
            // 
            // pathBox
            // 
            pathBox.Location = new Point(309, 52);
            pathBox.Name = "pathBox";
            pathBox.Size = new Size(109, 23);
            pathBox.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 25);
            label1.Name = "label1";
            label1.Size = new Size(85, 15);
            label1.TabIndex = 0;
            label1.Text = "Latest Version :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 54);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 0;
            label2.Text = "AutoPatch Port :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 83);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 0;
            label3.Text = "Server IP/Host :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(224, 27);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 0;
            label4.Text = "Website Port :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(225, 55);
            label5.Name = "label5";
            label5.Size = new Size(79, 15);
            label5.TabIndex = 0;
            label5.Text = "Patch Folder :";
            // 
            // consoleBox
            // 
            consoleBox.BackColor = SystemColors.Window;
            consoleBox.BorderStyle = BorderStyle.None;
            consoleBox.ForeColor = SystemColors.WindowText;
            consoleBox.Location = new Point(12, 21);
            consoleBox.Name = "consoleBox";
            consoleBox.ReadOnly = true;
            consoleBox.Size = new Size(404, 282);
            consoleBox.TabIndex = 2;
            consoleBox.Text = "";
            // 
            // autosBox
            // 
            autosBox.AutoSize = true;
            autosBox.ForeColor = SystemColors.ControlText;
            autosBox.Location = new Point(334, 517);
            autosBox.Name = "autosBox";
            autosBox.Size = new Size(79, 19);
            autosBox.TabIndex = 6;
            autosBox.Text = "Auto Start";
            autosBox.UseVisualStyleBackColor = true;
            // 
            // statusValueLabel
            // 
            statusValueLabel.AutoSize = true;
            statusValueLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            statusValueLabel.ForeColor = SystemColors.ControlText;
            statusValueLabel.Location = new Point(10, 517);
            statusValueLabel.Name = "statusValueLabel";
            statusValueLabel.Size = new Size(95, 15);
            statusValueLabel.TabIndex = 0;
            statusValueLabel.Text = "Status : OFFLINE";
            // 
            // serverTimeValueLabel
            // 
            serverTimeValueLabel.AutoSize = true;
            serverTimeValueLabel.ForeColor = SystemColors.ControlText;
            serverTimeValueLabel.Location = new Point(10, 536);
            serverTimeValueLabel.Name = "serverTimeValueLabel";
            serverTimeValueLabel.Size = new Size(148, 15);
            serverTimeValueLabel.TabIndex = 0;
            serverTimeValueLabel.Text = "Server Time (24-H): --:--:--";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.ControlText;
            label6.Location = new Point(10, 556);
            label6.Name = "label6";
            label6.Size = new Size(88, 15);
            label6.TabIndex = 0;
            label6.Text = "Online Server: -";
            // 
            // startBtn
            // 
            startBtn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            startBtn.ForeColor = SystemColors.ControlText;
            startBtn.Location = new Point(334, 539);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(100, 32);
            startBtn.TabIndex = 7;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(consoleBox);
            groupBox2.Location = new Point(7, 166);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(429, 345);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Logs";
            // 
            // button1
            // 
            button1.Location = new Point(331, 309);
            button1.Name = "button1";
            button1.Size = new Size(85, 29);
            button1.TabIndex = 3;
            button1.Text = "Clear Log";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(448, 579);
            Controls.Add(groupBox2);
            Controls.Add(startBtn);
            Controls.Add(label6);
            Controls.Add(serverTimeValueLabel);
            Controls.Add(statusValueLabel);
            Controls.Add(autosBox);
            Controls.Add(groupBox1);
            Controls.Add(titleBarPanel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(388, 478);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Patch Server By DuaSelipar";
            Load += Form1_Load;
            titleBarPanel.ResumeLayout(false);
            titleBarPanel.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)autoNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)verNumeric).EndInit();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel titleBarPanel;
        private Label titleLabel;
        private Button themeBtn;
        private Button minimizeBtn;
        private Button closeBtn;
        private GroupBox groupBox1;
        private NumericUpDown webNumeric;
        private NumericUpDown autoNumeric;
        private NumericUpDown verNumeric;
        private TextBox hostBox;
        private TextBox pathBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private RichTextBox consoleBox;
        private CheckBox autosBox;
        private Label statusValueLabel;
        private Label serverTimeValueLabel;
        private Label label6;
        private Button startBtn;
        private GroupBox groupBox2;
        private Button button1;
    }
}

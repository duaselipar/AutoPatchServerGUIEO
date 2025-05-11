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
            hostBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            pathBox = new TextBox();
            startBtn = new Button();
            autosBox = new CheckBox();
            groupBox1 = new GroupBox();
            webNumeric = new NumericUpDown();
            autoNumeric = new NumericUpDown();
            verNumeric = new NumericUpDown();
            label6 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            richTextBox1 = new RichTextBox();
            consoleBox = new RichTextBox();
            fbLink = new LinkLabel();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)autoNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)verNumeric).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // hostBox
            // 
            hostBox.Location = new Point(109, 80);
            hostBox.Name = "hostBox";
            hostBox.Size = new Size(381, 23);
            hostBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 25);
            label1.Name = "label1";
            label1.Size = new Size(85, 15);
            label1.TabIndex = 3;
            label1.Text = "Latest Version :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 54);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 4;
            label2.Text = "AutoPatch Port :";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 83);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 5;
            label3.Text = "Server IP/Host :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(264, 25);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 6;
            label4.Text = "Website Port :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(265, 54);
            label5.Name = "label5";
            label5.Size = new Size(79, 15);
            label5.TabIndex = 7;
            label5.Text = "Patch Folder :";
            // 
            // pathBox
            // 
            pathBox.Location = new Point(350, 51);
            pathBox.Name = "pathBox";
            pathBox.Size = new Size(140, 23);
            pathBox.TabIndex = 9;
            // 
            // startBtn
            // 
            startBtn.Location = new Point(506, 22);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(195, 52);
            startBtn.TabIndex = 10;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // autosBox
            // 
            autosBox.AutoSize = true;
            autosBox.Location = new Point(622, 80);
            autosBox.Name = "autosBox";
            autosBox.Size = new Size(79, 19);
            autosBox.TabIndex = 11;
            autosBox.Text = "Auto Start";
            autosBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(webNumeric);
            groupBox1.Controls.Add(autoNumeric);
            groupBox1.Controls.Add(verNumeric);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(autosBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(startBtn);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(pathBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(hostBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(712, 121);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // webNumeric
            // 
            webNumeric.Location = new Point(350, 22);
            webNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            webNumeric.Name = "webNumeric";
            webNumeric.Size = new Size(140, 23);
            webNumeric.TabIndex = 15;
            // 
            // autoNumeric
            // 
            autoNumeric.Location = new Point(109, 52);
            autoNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            autoNumeric.Name = "autoNumeric";
            autoNumeric.Size = new Size(140, 23);
            autoNumeric.TabIndex = 14;
            // 
            // verNumeric
            // 
            verNumeric.Location = new Point(109, 22);
            verNumeric.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            verNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            verNumeric.Name = "verNumeric";
            verNumeric.Size = new Size(140, 23);
            verNumeric.TabIndex = 13;
            verNumeric.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(523, 103);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 12;
            label6.Text = "Online Server :";
            label6.Click += label6_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(732, 161);
            tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(724, 133);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Configuration";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(724, 133);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "About";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(3, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(715, 130);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // consoleBox
            // 
            consoleBox.Location = new Point(7, 154);
            consoleBox.Name = "consoleBox";
            consoleBox.ReadOnly = true;
            consoleBox.Size = new Size(712, 346);
            consoleBox.TabIndex = 14;
            consoleBox.Text = "";
            // 
            // fbLink
            // 
            fbLink.AutoSize = true;
            fbLink.Location = new Point(7, 503);
            fbLink.Name = "fbLink";
            fbLink.Size = new Size(63, 15);
            fbLink.TabIndex = 15;
            fbLink.TabStop = true;
            fbLink.Text = "DuaSelipar";
            fbLink.LinkClicked += fbLink_LinkClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(731, 523);
            Controls.Add(fbLink);
            Controls.Add(consoleBox);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Patch Server By DuaSelipar";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)autoNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)verNumeric).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox hostBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox pathBox;
        private Button startBtn;
        private CheckBox autosBox;
        private GroupBox groupBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox consoleBox;
        private Label label6;
        private NumericUpDown webNumeric;
        private NumericUpDown autoNumeric;
        private NumericUpDown verNumeric;
        private LinkLabel fbLink;
        private RichTextBox richTextBox1;
    }
}

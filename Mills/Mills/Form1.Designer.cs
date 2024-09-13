namespace Mills
{
    partial class Menu
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            checkBoxSMM = new CheckBox();
            checkBoxNMM = new CheckBox();
            checkBoxTMM = new CheckBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Sienna;
            button1.FlatAppearance.BorderColor = Color.Black;
            button1.FlatAppearance.BorderSize = 3;
            button1.FlatAppearance.MouseDownBackColor = Color.SaddleBrown;
            button1.FlatAppearance.MouseOverBackColor = Color.SaddleBrown;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Calibri", 18F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button1.ForeColor = Color.PeachPuff;
            button1.Location = new Point(62, 211);
            button1.Name = "button1";
            button1.Size = new Size(275, 70);
            button1.TabIndex = 0;
            button1.Text = "Player vs Player";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Player_vs_Player_Button_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Sienna;
            button2.FlatAppearance.BorderColor = Color.Black;
            button2.FlatAppearance.BorderSize = 3;
            button2.FlatAppearance.MouseDownBackColor = Color.SaddleBrown;
            button2.FlatAppearance.MouseOverBackColor = Color.SaddleBrown;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Calibri", 18F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button2.ForeColor = Color.PeachPuff;
            button2.Location = new Point(62, 288);
            button2.Name = "button2";
            button2.Size = new Size(275, 70);
            button2.TabIndex = 1;
            button2.Text = "Player vs Computer";
            button2.UseVisualStyleBackColor = false;
            button2.Click += Player_vs_Computer_Button_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Sienna;
            button3.FlatAppearance.BorderColor = Color.Black;
            button3.FlatAppearance.BorderSize = 3;
            button3.FlatAppearance.MouseDownBackColor = Color.SaddleBrown;
            button3.FlatAppearance.MouseOverBackColor = Color.SaddleBrown;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Calibri", 18F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button3.ForeColor = Color.PeachPuff;
            button3.Location = new Point(249, 397);
            button3.Name = "button3";
            button3.Size = new Size(275, 70);
            button3.TabIndex = 5;
            button3.Text = "Rules";
            button3.UseVisualStyleBackColor = false;
            button3.Click += Rules_Button_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cooper Black", 48F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.SaddleBrown;
            label1.Location = new Point(276, 9);
            label1.Name = "label1";
            label1.Size = new Size(235, 91);
            label1.TabIndex = 6;
            label1.Text = "Mills";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cooper Black", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Sienna;
            label2.Location = new Point(395, 135);
            label2.Name = "label2";
            label2.Size = new Size(361, 42);
            label2.TabIndex = 7;
            label2.Text = "Choose gamemode";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cooper Black", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Sienna;
            label3.Location = new Point(24, 135);
            label3.Name = "label3";
            label3.Size = new Size(342, 42);
            label3.TabIndex = 8;
            label3.Text = "Choose opponent";
            // 
            // checkBoxSMM
            // 
            checkBoxSMM.BackColor = Color.Transparent;
            checkBoxSMM.Cursor = Cursors.Hand;
            checkBoxSMM.Font = new Font("Calibri", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 238);
            checkBoxSMM.ForeColor = Color.Sienna;
            checkBoxSMM.Location = new Point(428, 262);
            checkBoxSMM.Name = "checkBoxSMM";
            checkBoxSMM.Size = new Size(285, 45);
            checkBoxSMM.TabIndex = 10;
            checkBoxSMM.Text = "Six Men's Morris";
            checkBoxSMM.UseVisualStyleBackColor = false;
            checkBoxSMM.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // checkBoxNMM
            // 
            checkBoxNMM.BackColor = Color.Transparent;
            checkBoxNMM.Cursor = Cursors.Hand;
            checkBoxNMM.Font = new Font("Calibri", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 238);
            checkBoxNMM.ForeColor = Color.Sienna;
            checkBoxNMM.Location = new Point(428, 313);
            checkBoxNMM.Name = "checkBoxNMM";
            checkBoxNMM.Size = new Size(285, 45);
            checkBoxNMM.TabIndex = 11;
            checkBoxNMM.Text = "Nine Men's Morris";
            checkBoxNMM.UseVisualStyleBackColor = false;
            checkBoxNMM.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // checkBoxTMM
            // 
            checkBoxTMM.BackColor = Color.Transparent;
            checkBoxTMM.Cursor = Cursors.Hand;
            checkBoxTMM.Font = new Font("Calibri", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 238);
            checkBoxTMM.ForeColor = Color.Sienna;
            checkBoxTMM.Location = new Point(428, 211);
            checkBoxTMM.Name = "checkBoxTMM";
            checkBoxTMM.Size = new Size(285, 45);
            checkBoxTMM.TabIndex = 12;
            checkBoxTMM.Text = "Three Men's Morris";
            checkBoxTMM.UseVisualStyleBackColor = false;
            checkBoxTMM.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Tan;
            ClientSize = new Size(782, 553);
            Controls.Add(checkBoxTMM);
            Controls.Add(checkBoxNMM);
            Controls.Add(checkBoxSMM);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            MaximizeBox = false;
            MaximumSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Name = "Menu";
            Text = "Menu";
            FormClosing += Menu_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
        private Label label2;
        private Label label3;
        private CheckBox checkBoxSMM;
        private CheckBox checkBoxNMM;
        private CheckBox checkBoxTMM;
    }
}

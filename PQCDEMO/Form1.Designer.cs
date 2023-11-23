namespace PQCDEMO
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
            button1 = new Button();
            groupBox1 = new GroupBox();
            groupBox4 = new GroupBox();
            pictureBox1 = new PictureBox();
            groupBox2 = new GroupBox();
            pictureBox2 = new PictureBox();
            groupBox3 = new GroupBox();
            pictureBox3 = new PictureBox();
            groupBox5 = new GroupBox();
            panel1 = new Panel();
            groupBox6 = new GroupBox();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(687, 152);
            button1.Name = "button1";
            button1.Size = new Size(114, 167);
            button1.TabIndex = 0;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(643, 145);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "軸卡";
            // 
            // groupBox4
            // 
            groupBox4.Location = new Point(816, 6);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(435, 551);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "機台Config";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = Properties.Resources.M114;
            pictureBox1.Location = new Point(490, 23);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(150, 119);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pictureBox2);
            groupBox2.Location = new Point(12, 162);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(643, 145);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "IO卡";
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.DP;
            pictureBox2.Location = new Point(490, 23);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(150, 119);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(pictureBox3);
            groupBox3.Location = new Point(12, 319);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(643, 145);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "機台";
            // 
            // pictureBox3
            // 
            pictureBox3.Dock = DockStyle.Right;
            pictureBox3.Image = Properties.Resources.DP3000_G3_V2_Ori_color;
            pictureBox3.Location = new Point(490, 23);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(150, 119);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 1;
            pictureBox3.TabStop = false;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(panel1);
            groupBox5.Location = new Point(827, 18);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(435, 551);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "機台Config";
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 23);
            panel1.Margin = new Padding(1);
            panel1.Name = "panel1";
            panel1.Size = new Size(429, 525);
            panel1.TabIndex = 0;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(button7);
            groupBox6.Controls.Add(button6);
            groupBox6.Controls.Add(button5);
            groupBox6.Controls.Add(button4);
            groupBox6.Controls.Add(button3);
            groupBox6.Location = new Point(12, 471);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(643, 145);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "DW";
            // 
            // button7
            // 
            button7.Location = new Point(307, 33);
            button7.Margin = new Padding(1);
            button7.Name = "button7";
            button7.Size = new Size(158, 28);
            button7.TabIndex = 4;
            button7.Text = "DediNetLinkerLog";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(185, 74);
            button6.Margin = new Padding(1);
            button6.Name = "button6";
            button6.Size = new Size(123, 28);
            button6.TabIndex = 3;
            button6.Text = "FirmwareLog";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(189, 33);
            button5.Margin = new Padding(1);
            button5.Name = "button5";
            button5.Size = new Size(96, 28);
            button5.TabIndex = 2;
            button5.Text = "ServerLog";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(79, 74);
            button4.Margin = new Padding(1);
            button4.Name = "button4";
            button4.Size = new Size(96, 28);
            button4.TabIndex = 1;
            button4.Text = "ProjLog";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(79, 33);
            button3.Margin = new Padding(1);
            button3.Name = "button3";
            button3.Size = new Size(96, 28);
            button3.TabIndex = 0;
            button3.Text = "ClientLog";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(687, 377);
            button2.Name = "button2";
            button2.Size = new Size(114, 167);
            button2.TabIndex = 6;
            button2.Text = "EXPORT";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1285, 621);
            Controls.Add(button2);
            Controls.Add(groupBox6);
            Controls.Add(groupBox5);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Form1";
            Text = "PQC";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private GroupBox groupBox1;
        private PictureBox pictureBox1;
        private GroupBox groupBox2;
        private PictureBox pictureBox2;
        private Label label5;
        private Label label6;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private PictureBox pictureBox3;
        private Button button2;
        private Button button3;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Panel panel1;
    }
}
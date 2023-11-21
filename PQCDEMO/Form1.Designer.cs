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
            groupBox6 = new GroupBox();
            button2 = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(1604, 368);
            button1.Margin = new Padding(8);
            button1.Name = "button1";
            button1.Size = new Size(267, 404);
            button1.TabIndex = 0;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(29, 28);
            groupBox1.Margin = new Padding(8);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(8);
            groupBox1.Size = new Size(1501, 350);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "軸卡";
            // 
            // groupBox4
            // 
            groupBox4.Location = new Point(1903, 14);
            groupBox4.Margin = new Padding(8);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(8);
            groupBox4.Size = new Size(1014, 1334);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "機台Config";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = Properties.Resources.M114;
            pictureBox1.Location = new Point(1144, 54);
            pictureBox1.Margin = new Padding(8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(349, 288);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pictureBox2);
            groupBox2.Location = new Point(29, 392);
            groupBox2.Margin = new Padding(8);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(8);
            groupBox2.Size = new Size(1501, 350);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "IO卡";
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.DP;
            pictureBox2.Location = new Point(1144, 54);
            pictureBox2.Margin = new Padding(8);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(349, 288);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(pictureBox3);
            groupBox3.Location = new Point(29, 772);
            groupBox3.Margin = new Padding(8);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(8);
            groupBox3.Size = new Size(1501, 350);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "機台";
            // 
            // pictureBox3
            // 
            pictureBox3.Dock = DockStyle.Right;
            pictureBox3.Image = Properties.Resources.DP3000_G3_V2_Ori_color;
            pictureBox3.Location = new Point(1144, 54);
            pictureBox3.Margin = new Padding(8);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(349, 288);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 1;
            pictureBox3.TabStop = false;
            // 
            // groupBox5
            // 
            groupBox5.Location = new Point(1930, 44);
            groupBox5.Margin = new Padding(8);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(8);
            groupBox5.Size = new Size(1014, 1334);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "機台Config";
            // 
            // groupBox6
            // 
            groupBox6.Location = new Point(29, 1140);
            groupBox6.Margin = new Padding(8);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(8);
            groupBox6.Size = new Size(1501, 350);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "DW";
            // 
            // button2
            // 
            button2.Location = new Point(1604, 913);
            button2.Margin = new Padding(8);
            button2.Name = "button2";
            button2.Size = new Size(267, 404);
            button2.TabIndex = 6;
            button2.Text = "EXPORT";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(21F, 46F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(3045, 1506);
            Controls.Add(button2);
            Controls.Add(groupBox6);
            Controls.Add(groupBox5);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(8);
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
    }
}
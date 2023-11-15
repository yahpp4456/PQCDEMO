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
            label1 = new Label();
            pictureBox1 = new PictureBox();
            groupBox2 = new GroupBox();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            groupBox3 = new GroupBox();
            label3 = new Label();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
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
            button1.Location = new Point(1157, 860);
            button1.Name = "button1";
            button1.Size = new Size(310, 167);
            button1.TabIndex = 0;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1464, 260);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "軸卡";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft JhengHei UI", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(1193, 59);
            label1.Name = "label1";
            label1.Size = new Size(240, 152);
            label1.TabIndex = 1;
            label1.Text = "OK";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = Properties.Resources.M114;
            pictureBox1.Location = new Point(1148, 23);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(313, 234);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(pictureBox2);
            groupBox2.Location = new Point(9, 283);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1464, 260);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "機台Config";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft JhengHei UI", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(1196, 63);
            label2.Name = "label2";
            label2.Size = new Size(240, 152);
            label2.TabIndex = 2;
            label2.Text = "OK";
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.DP3000_G3_V2_Ori_color;
            pictureBox2.Location = new Point(1148, 23);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(313, 234);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(pictureBox3);
            groupBox3.Location = new Point(9, 563);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1464, 260);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "DediWare";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft JhengHei UI", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(1196, 72);
            label3.Name = "label3";
            label3.Size = new Size(253, 152);
            label3.TabIndex = 3;
            label3.Text = "NG";
            // 
            // pictureBox3
            // 
            pictureBox3.Dock = DockStyle.Right;
            pictureBox3.Image = Properties.Resources.DP;
            pictureBox3.Location = new Point(1148, 23);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(313, 234);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Red;
            label4.Font = new Font("Microsoft JhengHei UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = Color.White;
            label4.Location = new Point(14, 72);
            label4.Name = "label4";
            label4.Size = new Size(225, 43);
            label4.TabIndex = 4;
            label4.Text = "Version:1.3.5";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.LimeGreen;
            label5.Font = new Font("Microsoft JhengHei UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.White;
            label5.Location = new Point(14, 172);
            label5.Name = "label5";
            label5.Size = new Size(301, 43);
            label5.TabIndex = 4;
            label5.Text = "Mdata=QCconfig";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.LimeGreen;
            label6.Font = new Font("Microsoft JhengHei UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label6.ForeColor = Color.White;
            label6.Location = new Point(358, 172);
            label6.Name = "label6";
            label6.Size = new Size(232, 43);
            label6.TabIndex = 5;
            label6.Text = "Voltage=24V";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1482, 1051);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private GroupBox groupBox1;
        private PictureBox pictureBox1;
        private GroupBox groupBox2;
        private PictureBox pictureBox2;
        private Label label1;
        private GroupBox groupBox3;
        private PictureBox pictureBox3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
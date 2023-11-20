using PQCDEMO.Properties;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TPM;

namespace PQCDEMO
{

    public partial class Form1 : Form
    {


        private AxisController axisXController;
        private AxisController axisYController;
        private AxisController axisZController;
        private List<Label> inputLabels;

        private MotionController _m114 = new MotionController(true);

        public Form1()
        {

            IOCardWrapper pCE_D122Wrapper = new IOCardWrapper(true);
            InitializeComponent();
            // 初始化軸物件
            axisXController = new AxisController("X", _m114, 0, groupBox1);
            axisYController = new AxisController("Y", _m114, 1, groupBox1);
            axisZController = new AxisController("Z", _m114, 2, groupBox1);

            int startY = 30; // Starting Y position for the first axis
            int gap = 5; // Gap between each group of TextBoxes
            int textBoxHeight = 30;

            // 更新文本框組
            axisXController.UpdateTextBoxGroup(startY);
            axisYController.UpdateTextBoxGroup(startY + (textBoxHeight + gap)); // 16 TextBoxes per axis
            axisZController.UpdateTextBoxGroup(startY + (textBoxHeight + gap) * 2);






            inputLabels = new List<Label>();
            for (byte bitNo = 0; bitNo < 16; bitNo++)
            {
                Button btn = new Button();
                //  label.Text = $"Input {bitNo}: {ioControl.ReadInput(bitNo)}";

                btn.Text = bitNo.ToString();
                btn.Top = 20 + (bitNo * 25);
                btn.Left = 10;
                btn.Location = new Point(btn.Top + 10, btn.Left + 50);
                //  inputLabels.Add(btn);
                groupBox2.Controls.Add(btn); // 添加到 group2 中
            }







        }



        /* private void pictrans(PictureBox pic, Label lab)
         {

             Image originalImage = pic.Image;


             ColorMatrix colorMatrix = new ColorMatrix();
             colorMatrix.Matrix33 = 0.5f;


             ImageAttributes imageAttributes = new ImageAttributes();
             imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


             Bitmap transparentImage = new Bitmap(originalImage.Width, originalImage.Height);
             using (Graphics graphics = Graphics.FromImage(transparentImage))
             {
                 graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                    0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);
             }


             pic.Image = transparentImage;



             lab.BackColor = Color.Transparent;
             lab.Parent = pic;

             lab.ForeColor = Color.Green;
             int x = (pic.Width - lab.Width) / 2;
             int y = (pic.Height - lab.Height) / 2;

             lab.Location = new Point(x, y);

         }*/

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                test();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void test()
        {
            axisXController.UpdateStatus();

            HashSet<int> functionPositionsToCheck = new HashSet<int>
        {
            0, // RDY
            2, // +EL
            4, // ORG
            6  // EMG
        };

            bool allFunctionsActive = axisXController.AreAllFunctionsActive(functionPositionsToCheck);

            int axisStatus = 0;
            foreach (int position in functionPositionsToCheck)
            {
                axisStatus |= (1 << position);
            }

            string message = allFunctionsActive
                ? $"所有指定的功能位置都處於活動狀態。AxisStatus: 0x{axisStatus:X}"
                : $"有一些或所有指定的功能位置不處於活動狀態。AxisStatus: 0x{axisStatus:X}";

            MessageBox.Show(message, allFunctionsActive ? "提示" : "錯誤", MessageBoxButtons.OK, allFunctionsActive ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
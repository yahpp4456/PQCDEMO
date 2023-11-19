using PQCDEMO.Properties;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TPM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PQCDEMO
{



    public partial class Form1 : Form
    {


        private AxisController axisXController;
        private AxisController axisYController;
        private AxisController axisZController;

        private MotionController _m114= new MotionController(true);

               public Form1()
        {
            InitializeComponent();
           // initMot();

            axisXController = new AxisController("X", _m114,0,groupBox1);
            axisYController = new AxisController("Y", _m114,1,groupBox1);
            axisZController = new AxisController("Z", _m114,2, groupBox1);
            // InitializeTextBoxGroup(axis1TextBoxes, groupBox3, 100, "I");
            // Set the starting Y positions so that each group of TextBoxes is below the previous group
            int startY = 30; // Starting Y position for the first axis
            int gap = 5; // Gap between each group of TextBoxes
            int textBoxHeight = 30; // Assuming each TextBox is 20px high

            // Update the TextBox groups to position them correctly within the same GroupBox
            axisXController.UpdateTextBoxGroup(startY);
            axisYController.UpdateTextBoxGroup(startY + (textBoxHeight + gap)); // 16 TextBoxes per axis
            axisZController.UpdateTextBoxGroup(startY + (textBoxHeight + gap)*2);
        }

        private void QcStatus()
        {


            //   pictrans(pictureBox1, label1);
            // pictrans(pictureBox2, label2);
            // pictrans(pictureBox3, label3);

        }

        private void pictrans(PictureBox pic, Label lab)
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

        }

        /*private void initMot()
        {


            UInt16 existcards = 0;
            UInt16 CardNo = 0; // 


            ret = Master.PCI_M114._m114_open(ref existcards);
            if (existcards == 0 || ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("No any PCI_M114 or error in opening card!", "Error");
                return;
            }
            else { MessageBox.Show("existcards: " + existcards); }


            ret = Master.PCI_M114._m114_get_switch_card_num(CardNo, ref SwitchNo);
            if (ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("Error in getting switch card number!", "Error");
                return;
            }

            // 初始化控制卡
            ret = Master.PCI_M114._m114_initial(SwitchNo);
            if (ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("Error in initializing card!", "Error");
                return;
            }
            else { MessageBox.Show("SwitchNo: " + SwitchNo); }


        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            axisXController.UpdateStatus();



        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
using System.Diagnostics;
using TPM;

namespace PQCDEMO
{



    public partial class Form1 : Form
    {


        private readonly HashSet<int> requiredFunctionPositions = new HashSet<int> { 11, 11, 12, 13 }; // 可以動態更新這個集合

        private bool AreAllRequiredFunctionsActive(UInt16 axisStatus, HashSet<int> requiredPositions)
        {
            foreach (int position in requiredPositions)
            {
                if (!IsFunctionActive(bitMeaningMapping[position], axisStatus))
                {
                    return false;
                }
            }
            return true;
        }

        UInt16 AxisOrgStatus;
        Master.PCI_M114.ErrCode ret;
        ushort SwitchNo;

        Dictionary<int, string> bitMeaningMapping = new Dictionary<int, string>
                {
                    { 0, "RDY" },
                    { 1, "ALM " },
                    { 2, "+EL" },
                    { 3, "-EL" },
                    { 4, "ORG" },
                    { 5, "DIR" },
                    { 6, "EMG " },
                    { 7, "Reserved" },
                    { 8, "ERC" },
                    { 9, "EZ " },
                    { 10, "Reserved" },
                    { 11, "Latch" },
                    { 12, "SD" },
                    { 13, "INP" },
                    { 14, "SVON" },
                    { 15, "RALM" }
                 };

        bool IsFunctionActive(string functionName, int statusWord)
        {
            var bitPosition = bitMeaningMapping.FirstOrDefault(x => x.Value == functionName).Key;
            return (statusWord & (1 << bitPosition)) != 0;
        }




        private void UpdateTextBoxesAndColors(UInt16 axis, UInt16 axisStatus)
        {
            List<TextBox> textBoxGroup = null;
            string axisName = "";

            switch (axis)
            {
                case 0:
                    textBoxGroup = axis1TextBoxes;
                    axisName = "X";
                    break;
                case 1:
                    textBoxGroup = axis2TextBoxes;
                    axisName = "Y";
                    break;
                case 2:
                    textBoxGroup = axis3TextBoxes;
                    axisName = "Z";
                    break;
                default:
                    // Handle invalid axis value if needed
                    break;
            }

            if (textBoxGroup != null)
            {

                bool allRequiredFunctionsActive = AreAllRequiredFunctionsActive(axisStatus, requiredFunctionPositions);
                for (int i = 0; i < 16; i++)
                {
                    TextBox textBox = textBoxGroup[i];
                    string functionName = bitMeaningMapping[i];

                    // Set the TextBox text to the corresponding function name
                    textBox.Text = axisName + " " + functionName;

                    // Check if the function is active (IsFunctionActive result is 1)
                    bool isActive = IsFunctionActive(functionName, axisStatus);

                    // Change TextBox background color based on the IsActive status
                    textBox.BackColor = isActive ? Color.LightGreen : Color.Green;
                }
            }
        }


        // 創建三個不同的列表，每個列表代表一組軸的文本框
        private List<TextBox> axis1TextBoxes = new List<TextBox>();
        private List<TextBox> axis2TextBoxes = new List<TextBox>();
        private List<TextBox> axis3TextBoxes = new List<TextBox>();
        public Form1()
        {
            InitializeComponent();
            initMot();

            InitializeTextBoxGroup(axis1TextBoxes, groupBox1, 50, "X");
            InitializeTextBoxGroup(axis2TextBoxes, groupBox1, 100, "Y");
            InitializeTextBoxGroup(axis3TextBoxes, groupBox1, 150, "Z");
        }
        private void InitializeTextBoxGroup(List<TextBox> textBoxGroup, GroupBox groupBox, int startY, string axisName)
        {
            for (int i = 0; i < 16; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Parent = groupBox; // 設置文本框的父容器為groupBox
                textBox.Location = new System.Drawing.Point((i + 1) * 65 - 30, startY);
                textBox.Size = new System.Drawing.Size(65, 30); // 調整文本框的寬度為50

                // 為文本框指定名稱，格式為 axisName + "info" + i
                string v = axisName + "info" + i;
                string textBoxName = v;
                textBox.Name = textBoxName;
                textBox.Text = v;
                textBox.BackColor = Color.LightGreen;
                // 設置 TextBox 為只讀模式
                textBox.ReadOnly = true;
                textBox.Enabled = false;
                textBox.TextAlign = HorizontalAlignment.Center;
                textBox.Cursor = Cursors.Arrow;
                // 將文本框添加到textBoxGroup中
                textBoxGroup.Add(textBox);
            }
        }
        private void initMot()
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


        }
        private void button1_Click(object sender, EventArgs e)
        {

            for (UInt16 axis = 0; axis < 3; axis++)
            {
                ret = Master.PCI_M114._m114_get_io_status(SwitchNo, axis, ref AxisOrgStatus);
                if (ret == Master.PCI_M114.ErrCode.ERR_NoError)
                {

                    UpdateTextBoxesAndColors(axis, AxisOrgStatus);
                }
                else
                {

                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
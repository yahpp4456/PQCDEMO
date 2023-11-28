using Newtonsoft.Json;
using PQCDEMO.Properties;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using TPM;
using System.Configuration;
using System.Xml.Linq;
using PQCDEMO.Model;

namespace PQCDEMO
{

    public partial class Form1 : Form
    {


        private AxisController axisXController;
        private AxisController axisYController;
        private AxisController axisZController;

        private MainConfig mainConfig1 = new MainConfig();
        private ApplicationConfig appConfig;
        private static bool _isdemo = true;
        private MotionController _m114 = new MotionController(_isdemo);

        IOCardWrapper pCE_D122Wrapper = new IOCardWrapper(_isdemo);

        //資料來源
        string sourceFilePath = System.Configuration.ConfigurationManager.AppSettings["sourceFilePath"];
        //目的位置
        string targetFilePath = System.Configuration.ConfigurationManager.AppSettings["targetFilePath"];

        string needDataPeriod = System.Configuration.ConfigurationManager.AppSettings["needDataPeriod"];
        public Form1()
        {

            InitializeComponent();
            // 初始化軸物件
            axisXController = new AxisController("X", _m114, 0, groupBox1);
            axisYController = new AxisController("Y", _m114, 1, groupBox1);
            axisZController = new AxisController("Z", _m114, 2, groupBox1);

            int startY = 80; // Starting Y position for the first axis
            int gap = 50; // Gap between each group of TextBoxes
            int textBoxHeight = 30;

            // 更新文本框組
            axisXController.UpdateTextBoxGroup(startY);
            axisYController.UpdateTextBoxGroup(startY + (textBoxHeight + gap)); // 16 TextBoxes per axis
            axisZController.UpdateTextBoxGroup(startY + (textBoxHeight + gap) * 2);

            LoadConfig();
            //button3.Click += (sender, args) => ToggleGroup(groupBox7);
            //button4.Click += (sender, args) => ToggleGroup(groupBox8);
            //button5.Click += (sender, args) => ToggleGroup(groupBox9);
            //button6.Click += (sender, args) => ToggleGroup(groupBox10);

            //button3.Click += (sender, args) => ToggleGroup(_xgroupBox);
            //button4.Click += (sender, args) => ToggleGroup(_ygroupBox);
            //button5.Click += (sender, args) => ToggleGroup(_zgroupBox);

        }


        private void ToggleGroup(GroupBox groupBox)
        {
            // Toggle visibility of the group
            groupBox.Visible = !groupBox.Visible;

            // Adjust the layout of visible groups
            AdjustGroupLayout();
        }

        private void AdjustGroupLayout()
        {
            // panel1.SuspendLayout();
            // 重設面板的控件集合
            panel1.Controls.Clear();
            int maxPanelHeight = panel1.Height;
            // 創建一個臨時列表以按順序保持群組盒
            // 計算當前所有可見GroupBox的總高度
            int currentHeight = 0;
            var groupBoxes = new List<GroupBox> { groupBox1 };
            //groupBox7, groupBox8, groupBox9, groupBox10
            // 如果群組盒是可見的，按順序添加回面板，並將它們停靠在頂部

            foreach (var groupBox in groupBoxes)
            {
                // 假如該GroupBox可見
                if (groupBox.Visible)
                {
                    // 手動計算要加入的GroupBox的高度
                    int groupBoxHeight = groupBox.Height;

                    // 檢查加上這個GroupBox後的總高度是否會超過面板的最大高度
                    if (currentHeight + groupBoxHeight > maxPanelHeight)
                    {
                        // 如果加上這個GroupBox會超過最大高度，則停止添加
                        break;
                    }
                    else
                    {
                        // 設置Dock屬性為DockStyle.Top，將群組盒停靠在面板頂部
                        groupBox.Dock = DockStyle.Top;
                        panel1.Controls.Add(groupBox);
                        groupBox.BringToFront();

                        // 更新目前的總高度
                        currentHeight += groupBoxHeight;
                    }
                }
            }
            //  panel1.ResumeLayout();
            //   panel1.Refresh();   
        }

        private void LoadConfig()
        {
            // 創建和配置 OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "選擇配置文件";
            openFileDialog.Filter = "JSON文件 (*.json)|*.json"; // 篩選只顯示JSON文件
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // 設置初始目錄

            // 顯示對話框
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 讀取選擇的文件的內容
                string jsonConfig = System.IO.File.ReadAllText(openFileDialog.FileName);
                mainConfig1 = JsonConvert.DeserializeObject<MainConfig>(jsonConfig);
            }
            if (mainConfig1 != null)
            {
                CreateButtons();

            }
        }
        Panel buttonPanel;
        private void CreateButtons()
        {
            // 创建一个Panel用于放置按钮
            buttonPanel = new Panel
            {
                AutoScroll = true, // 启用滚动条
                Location = new Point(10, 20), // 设置Panel的位置
                Size = new Size(groupBox2.Width - pictureBox2.Width - 20, groupBox2.Height - 50), // 设置Panel的大小
                BorderStyle = BorderStyle.None // 为了清晰可见，给Panel设置一个边框

            };

            groupBox2.Controls.Add(buttonPanel); // 将Panel添加到GroupBox

            int x = 10; // Panel内的初始X坐标
            int y = 10; // Panel内的初始Y坐标
            const int padding = 10; // 按钮之间的间距
            const int buttonWidth = 100; // 按钮的宽度
            const int buttonHeight = 30; // 按钮的高度
            int buttonPanelWidth = buttonPanel.Width - padding; // 用于计算何时需要换行

            // 创建输入按钮
            foreach (var input in mainConfig1.IOConfig.Inputs)
            {
                Button btn = new Button
                {
                    Text = input.Name,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(x, y),
                    Tag = input.Id // 设置按钮的Tag属性为ID
                };
                btn.Click += (sender, e) => InputButtonClick(input.Id);
                bool inputState = pCE_D122Wrapper.ReadInputBit(input.Id);
                btn.BackColor = inputState ? Color.LightGreen : Color.Green; ;
                buttonPanel.Controls.Add(btn);

                x += buttonWidth + padding;
                // 如果下一个按钮的位置超出Panel的宽度，则换行
                if (x + buttonWidth > buttonPanelWidth)
                {
                    x = 10; // 重置X坐标
                    y += buttonHeight + padding; // 增加Y坐标
                }
            }

            // 创建输出按钮，同样需要考虑换行
            x = 10; // 重置X坐标
            y += buttonHeight + padding; // 假设输入和输出按钮至少有一行的间隔

            foreach (var output in mainConfig1.IOConfig.Outputs)
            {
                Button btn = new Button
                {
                    Text = output.Name,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(x, y)
                };
                btn.Click += (sender, e) => OutputButtonClick(output.Id);
                buttonPanel.Controls.Add(btn);

                x += buttonWidth + padding;
                // 如果下一个按钮的位置超出Panel的宽度，则换行
                if (x + buttonWidth > buttonPanelWidth)
                {
                    x = 10; // 重置X坐标
                    y += buttonHeight + padding; // 增加Y坐标
                }
            }
        }


        private void CheckAndUpdateIOStatus()
        {
            // 在這裡檢查IO狀態並更新按鈕的顏色
            foreach (var input in mainConfig1.IOConfig.Inputs)
            {
                bool inputState = pCE_D122Wrapper.ReadInputBit(input.Id);
                //UpdateButtonColor(input.Id, inputState);
            }

            foreach (var output in mainConfig1.IOConfig.Outputs)
            {
                // 如果您需要檢查輸出的狀態，也可以在這裡添加相應的代碼
            }
        }

        private void StartIOStatusCheckingThread()
        {
            // 創建一個新的執行緒
            Thread ioThread = new Thread(() =>
            {
                while (true)
                {
                    // 檢查IO狀態並更新按鈕顏色
                    CheckAndUpdateIOStatus();

                    // 暫停一段時間，例如500毫秒，然後再次檢查
                    Thread.Sleep(100);
                }
            });
            ioThread.IsBackground = true;
            // 啟動執行緒
            ioThread.Start();
        }
        //private void UpdateButtonColor(int id, bool state)
        //{
        //    // 在这里更新按钮的颜色，根据IO状态
        //    if (InvokeRequired)
        //    {
        //        // 如果需要在UI线程上更新按钮，请使用Invoke方法
        //        Invoke(new Action(() => UpdateButtonColor(id, state)));
        //    }
        //    else
        //    {
        //        // 根据IO状态更新按钮的背景色
        //        foreach (Control control in buttonPanel.Controls)
        //        {
        //            if (control is Button button && (byte)button.Tag == id)
        //            {

        private void InputButtonClick(byte id)
        {
            bool state = pCE_D122Wrapper.ReadInputBit(id);
            MessageBox.Show($"Input {id}: {(state ? "On" : "Off")}", "Input State");
        }

        private void OutputButtonClick(byte id)
        {
            // 這裡的操作取決於您想要如何處理輸出按鈕的點擊事件。
            // 例如，您可以切換輸出的狀態：
            pCE_D122Wrapper.ToggleOutputBit(id);
            MessageBox.Show($"Toggled output {id}.", "Output State");
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
                // getAxisStatus();
                StartUpdatingThread();
                StartIOStatusCheckingThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void StartUpdatingThread()
        {
            Thread updateThread = new Thread(UpdateStatusLoop);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        // 定期更新状态的循环
        private void UpdateStatusLoop()
        {
            while (true)
            {

                getAxisStatus();

                Thread.Sleep(100);
            }
        }

        void getAxisStatus()
        {

            axisXController.UpdateStatus();
            axisYController.UpdateStatus();
            axisZController.UpdateStatus();

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
            bool allFunctionsActive2 = axisYController.AreAllFunctionsActive(functionPositionsToCheck);
            bool allFunctionsActive3 = axisZController.AreAllFunctionsActive(functionPositionsToCheck);
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

        private void button2_Click(object sender, EventArgs e)
        {


            MainConfig mainConfig = new MainConfig();
            mainConfig.IOConfig = mainConfig1.IOConfig;

            AxisConfig axis1Config = new AxisConfig(axisXController.AxisName, axisXController.AxisStatus);
            AxisConfig axis2Config = new AxisConfig(axisYController.AxisName, axisYController.AxisStatus);
            AxisConfig axis3Config = new AxisConfig(axisZController.AxisName, axisZController.AxisStatus);

            mainConfig.AxisConfigs.Add(axis1Config);
            mainConfig.AxisConfigs.Add(axis2Config);
            mainConfig.AxisConfigs.Add(axis3Config);

            string json = JsonConvert.SerializeObject(mainConfig);


            File.WriteAllText("config.json", json);


            /* string jsonFromFile = File.ReadAllText("config.json");
              MainConfig deserializedConfig = JsonConvert.DeserializeObject<MainConfig>(jsonFromFile);


              IOBoardConfig ioBoardConfig = deserializedConfig.IOB;
              List<AxisConfig> axisConfigs = deserializedConfig.AxisConfigs;*/

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DirectoryInfo root = new DirectoryInfo(sourceFilePath);

            var needFileList = GetNeedFile(root);
            foreach (var data in needFileList)
            {
                MoveFile(data.targetUrl, data.fileName);
            }

            Process.Start("explorer.exe", targetFilePath);
        }


        private List<LogFileInfo> GetNeedFile(DirectoryInfo root)
        {
            var result = new List<LogFileInfo>();
            var period = int.Parse(needDataPeriod);

            foreach (FileInfo file in root.GetFiles())
            {
                var data = new LogFileInfo();
                if (file.Extension == ".html")
                {
                    var timeStr = file.Name.Substring(0, 10);
                    if (DateTime.Now.AddDays(-period).Date > DateTime.Parse(timeStr) || DateTime.Parse(timeStr) > DateTime.Now.Date)
                    {
                        continue;
                    }

                    if (Path.GetFileNameWithoutExtension(file.Name).Length > 19)
                    {
                        data.targetUrl = $"{targetFilePath}ProjectSummaryLog";
                    }
                    else
                    {
                        data.targetUrl = $"{targetFilePath}ClientLog";
                    }

                    data.fileName = file.Name;
                }
                else if (file.Extension == ".log")
                {
                    var timeStr = "";

                    if (file.Name.Contains("DediProg"))
                    {
                        timeStr = file.Name.Substring(9, 10);
                        data.targetUrl = $"{targetFilePath}ServerLog";
                    }
                    else if (file.Name.Contains("Programmer"))
                    {
                        timeStr = file.Name.Substring(12, 10);
                        data.targetUrl = $"{targetFilePath}FirmwareLog";
                    }
                    else if (file.Name.Contains("DediNetLinker"))
                    {
                        timeStr = file.Name.Substring(14, 10);
                        data.targetUrl = $"{targetFilePath}DediNetLinkerLog";
                    }

                    if (DateTime.Now.AddDays(-period).Date > DateTime.Parse(timeStr) || DateTime.Parse(timeStr) > DateTime.Now.Date)
                    {
                        continue;
                    }
                    data.fileName = file.Name;
                }
                else
                {
                    continue;
                }
                result.Add(data);
            }
            return result;
        }

        private void MoveFile(string targetUrl, string name)
        {
            if (!Directory.Exists(targetUrl))
            {
                Directory.CreateDirectory(targetUrl);

                File.Copy(sourceFilePath + name, $"{targetUrl}/{name}");
            }
            else
            {
                File.Copy(sourceFilePath + name, $"{targetUrl}/{name}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var str1 = ReadXmlString($"{sourceFilePath}/MData.xml");
            var str2 = ReadXmlString($"{sourceFilePath}/MData_Modify.xml");
            var bl = mappingConfigXml(str1, str2);

            if (bl)
                button5.BackColor = Color.GreenYellow;
        }


        private string ReadXmlString(string xmlFilePath)
        {
            // 讀取 XML 檔案並解析為 XDocument
            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            return xmlDoc.ToString();
        }

        private bool mappingConfigXml(string xmlString1, string xmlString2)
        {
            XDocument xmlDoc1 = XDocument.Parse(xmlString1);
            XDocument xmlDoc2 = XDocument.Parse(xmlString2);

            bool areEqual = XNode.DeepEquals(xmlDoc1, xmlDoc2);

            return areEqual;

        }

        /*private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // 獲取Panel的Graphics對象
            Graphics g = e.Graphics;
            sender = sender as Panel;
            // 定義漸變的兩種顏色
            Color colorTop = Color.Blue;
            Color colorBottom = Color.LightSkyBlue;

            // 創建一個線性漸變筆刷
            Rectangle rectangle = new Rectangle(0, 0, sender.Width, panel1.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, colorTop, colorBottom, LinearGradientMode.Vertical))
            {
                // 使用漸變筆刷填充Panel的背景
                g.FillRectangle(brush, rectangle);
            }
        }*/
    }
}
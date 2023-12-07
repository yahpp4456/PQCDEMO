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
using static System.Windows.Forms.AxHost;
namespace PQCDEMO
{

    public partial class Form1 : Form
    {
        private AxisController axisXController;
        private AxisController axisYController;
        private AxisController axisZController;

        private MainConfig mainConfig1 = new MainConfig();
        //private ApplicationConfig appConfig;
        private static bool _isdemo = false;
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
            groupBox_axis.Visible = false;
            axisgroup(groupBox_axis);

            LoadConfig();
            groupBoxes.Add(groupBox7);
        }
        string[] texts = { "X", "Y", "Z" };
        private void axisgroup(GroupBox groupBox)
        {
            int startY = 30; // Starting Y position for the first axis
            for (int i = 0; i < 3; i++)
            {
                GroupBox newGroupBox = new GroupBox
                {
                    Location = new Point(groupBox.Location.X, groupBox.Location.Y + (i + 1) * (groupBox.Height + 10)), // 调整Y坐标以防止重叠
                    Size = groupBox.Size,
                    Text = groupBox.Text,
                    Font = groupBox.Font,
                    ForeColor = groupBox.ForeColor,
                    BackColor = groupBox.BackColor,
                    Visible = groupBox.Visible,
                    Dock = groupBox.Dock,
                    Parent = panel1 // 
                };
                groupBoxes.Add(newGroupBox);
                switch (i)
                {
                    case 0:
                        axisXController = new AxisController("X", _m114, 0, newGroupBox);
                        axisXController.UpdateTextBoxGroup(startY);
                        button_X.Click += (sender, args) => ToggleGroup(newGroupBox);

                        break;
                    case 1:
                        axisYController = new AxisController("Y", _m114, 1, newGroupBox);
                        axisYController.UpdateTextBoxGroup(startY);
                        button_Y.Click += (sender, args) => ToggleGroup(newGroupBox);
                        break;
                    case 2:
                        axisZController = new AxisController("Z", _m114, 2, newGroupBox);
                        axisZController.UpdateTextBoxGroup(startY);
                        button_Z.Click += (sender, args) => ToggleGroup(newGroupBox);
                        break;

                    default:
                        break;
                }


            }

        }
        private void ToggleGroup(GroupBox groupBox)
        {

            groupBox.SuspendLayout();
            // Toggle visibility of the group
            groupBox.Visible = !groupBox.Visible;

            // Adjust the layout of visible groups
            //Thread thread = new Thread(AdjustGroupLayoutThread);
            //thread.Start();
            AdjustGroupLayout();
            groupBox.ResumeLayout(true);

        }
        private void AdjustGroupLayoutThread()
        {
            // Check if we are on the UI thread
            if (InvokeRequired)
            {
                // We are not on the UI thread, so use Invoke to call AdjustGroupLayout
                Invoke(new Action(AdjustGroupLayout));
            }
            else
            {
                // We are on the UI thread, so call AdjustGroupLayout directly
                AdjustGroupLayout();
            }
        }

        List<GroupBox> groupBoxes = new List<GroupBox> { };
        private void AdjustGroupLayout()
        {
            panel1.SuspendLayout();
            panel1.Controls.Clear();
            int maxPanelHeight = panel1.Height;
            // 計算當前所有可見GroupBox的總高度
            int currentHeight = 0;


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
            panel1.ResumeLayout(true);

        }
        public void GetButtonStatesAsJson(MainConfig mainConfig, GroupBox groupBox)
        {
            var ioStates = groupBox.Controls
                .OfType<Panel>()
                .SelectMany(panel => panel.Controls.OfType<Button>())
                .Select(btn => new
                {
                    Tag = btn.Tag.ToString(),
                    IoItem = mainConfig.IOConfig.Inputs.Concat(mainConfig.IOConfig.Outputs)
                               .FirstOrDefault(item => item.Tag == btn.Tag.ToString())
                })
                .Where(x => x.IoItem != null)
                .Select(x => new IO_State
                {
                    Tag = x.Tag,
                    Id = x.IoItem.Id,
                    State = pCE_D122Wrapper.ReadInputBit(x.IoItem.Id) // 

                })
                .ToList();

            //        // 添加 AxisConfigs 成員的示例數據
            var axisConfigs = new List<AxisConfig>{
            //
           new AxisConfig(axisXController.AxisName, axisXController.AxisStatus),
           new AxisConfig(axisYController.AxisName, axisYController.AxisStatus),
           new AxisConfig(axisZController.AxisName, axisZController.AxisStatus)

            };

            var dataToSerialize = new
            {
                ioStates = ioStates,
                AxisConfigs = axisConfigs
            };

            string json = JsonConvert.SerializeObject(dataToSerialize);


            //  string json = JsonConvert.SerializeObject(ioStates);

            File.WriteAllText("QCreport.json", json);
        }


        public bool CompareButtonStatesWithJson(MainConfig mainConfig, GroupBox groupBox)
        {
            // 讀取之前生成的JSON檔案
            string json = File.ReadAllText("QCreport.json");

            // 將JSON轉換為List<IO_State>
            List<IO_State> savedStates = JsonConvert.DeserializeObject<List<IO_State>>(json);

            // 獲取當前按鈕的狀態
            var currentStates = groupBox.Controls
                .OfType<Panel>()
                .SelectMany(panel => panel.Controls.OfType<Button>())
                .Select(btn => new
                {
                    Tag = btn.Tag.ToString(),
                    IoItem = mainConfig.IOConfig.Inputs.Concat(mainConfig.IOConfig.Outputs)
                                .FirstOrDefault(item => item.Tag == btn.Tag.ToString())
                })
                .Where(x => x.IoItem != null)
                .Select(x => new IO_State
                {
                    Tag = x.Tag,
                    Id = x.IoItem.Id,
                    State = false // 假设一个函数来读取按钮的状态
                })
                .ToList();

            // 比對當前按鈕狀態與之前存儲的狀態
            bool isEqual = savedStates.Count == currentStates.Count &&
                           savedStates.All(savedState =>
                               currentStates.Any(currentState =>
                                   savedState.Tag == currentState.Tag &&
                                   savedState.Id == currentState.Id &&
                                   savedState.State == currentState.State
                               )
                           );

            return isEqual;
        }

        private void LoadConfig()
        {
            // 保存當前激活的窗口

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
                //CreateButtons();
                UpdateButtonLabels(mainConfig1, groupBox7);

                GetButtonStatesAsJson(mainConfig1, groupBox7);
            }


        }
        Panel buttonPanel;

        public void UpdateButtonLabels(MainConfig mainConfig, GroupBox groupBox)
        {
            foreach (Control panelControl in groupBox.Controls)
            {
                if (panelControl is Panel panel)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control is Button btn)
                        {
                            string buttonTag = btn.Tag.ToString();

                            // 查找对应的Input或Output
                            IOItem ioItem = mainConfig.IOConfig.Inputs.FirstOrDefault(input => input.Tag == buttonTag)
                                ?? mainConfig.IOConfig.Outputs.FirstOrDefault(output => output.Tag == buttonTag);

                            if (ioItem != null)
                            {
                                btn.Text = ioItem.Text;

                                // 如果是Input，添加点击事件并设置背景色
                                if (mainConfig.IOConfig.Inputs.Any(input => input.Tag == buttonTag))
                                {
                                    btn.Click += (sender, e) => InputButtonClick(ioItem);
                                    bool inputState = pCE_D122Wrapper.ReadInputBit(ioItem.Id);
                                    btn.BackColor = inputState ? Color.LightGreen : Color.Green;
                                }
                                // 如果是Output，添加点击事件
                                else if (mainConfig.IOConfig.Outputs.Any(output => output.Tag == buttonTag))
                                {
                                    btn.Click += (sender, e) => OutputButtonClick(ioItem);
                                }
                            }
                        }
                    }
                }
            }
        }


        private void CheckAndUpdateIOStatus()
        {
            //在這裡檢查IO狀態並更新按鈕的顏色
            foreach (var input in mainConfig1.IOConfig.Inputs)
            {
                bool inputState = pCE_D122Wrapper.ReadInputBit(input.Id);
                UpdateButtonColor(input, inputState);
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
        

                    Thread.Sleep(100);
                }
            });
            ioThread.IsBackground = true;
            // 啟動執行緒
            ioThread.Start();
        }
        private void UpdateButtonColor(IOItem ioitem, bool status)
        {
            // 在这里更新按钮的颜色，根据IO状态
            if (InvokeRequired)
            {
                // 如果需要在UI线程上更新按钮，请使用Invoke方法
                Invoke(new Action(() => UpdateButtonColor(ioitem, status)));
            }
            else
            {

                foreach (Control panelControl in groupBox7.Controls)
                {
                    if (panelControl is Panel panel)
                    {

                        foreach (Control control in panel.Controls)
                        {

                            if (control is Button button && (string)button.Tag == ioitem.Tag)
                            {

                                button.BackColor = status ? Color.LightGreen : Color.Green;
                            }
                        }

                    }
                }


            }
        }

        private void InputButtonClick(IOItem ioItem)
        {
            bool state = pCE_D122Wrapper.ReadInputBit(ioItem.Id);
            MessageBox.Show($"Input {ioItem.Id}: {(state ? "On" : "Off")}", "Input State");
        }

        private void OutputButtonClick(IOItem ioItem)
        {
            // 這裡的操作取決於您想要如何處理輸出按鈕的點擊事件。
            // 例如，您可以切換輸出的狀態：
            pCE_D122Wrapper.ToggleOutputBit(ioItem.Id);
            MessageBox.Show($"Toggled output {ioItem.Id}.", "Output State");
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
                StartUpdatingThread();//軸
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

        private void button2_Click_1(object sender, EventArgs e)
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

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button34_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            ToggleGroup(groupBox7);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
            //UpdateButtonColor();
        }

        //private void button2_Click_1(object sender, EventArgs e)
        //{

        //}

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
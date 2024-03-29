﻿using Newtonsoft.Json;
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
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace PQCDEMO
{

    public partial class MainForm : Form
    {
        private AxisController axisXController;
        private AxisController axisYController;
        private AxisController axisZController;


        private MainConfig mainConfig = new MainConfig();//主要配置文件
        private static bool _isdemo = true;
        private MotionController _m114 = new MotionController(_isdemo);

        private IOCardWrapper pCE_D122Wrapper = new IOCardWrapper(_isdemo);

        //資料來源
        string sourceFilePath = ConfigurationManager.AppSettings["sourceFilePath"];
        //目的位置
        string targetFilePath = ConfigurationManager.AppSettings["targetFilePath"];

        string needDataPeriod = ConfigurationManager.AppSettings["needDataPeriod"];

        string typeFilePath = ConfigurationManager.AppSettings["typeFilePath"];


        public MainForm()
        {

            InitializeComponent();
            // 初始化軸物件

            groupBox_axis.Visible = false;
            axisgroup(groupBox_axis);

            LoadConfig();
            groupBoxes.Add(groupBox_io);
            Info_groupBox.ForeColor = Color.White;
            groupBox_io.ForeColor = Color.White;
            StartUpdatingThread();
            //  StartIOStatusCheckingThread();
            StartIOStatusCheckingThreadAsync();


            // 詢問使用者輸入密碼
            string password = PromptForPassword();
            if (CheckPasswordLevel(password) == PasswordLevel.High)
            {
                // 高級初始化
                HighLevelInitialization();
            }
            else
            {
                // 標準初始化
                StandardInitialization();
            }

        }

        private string PromptForPassword()
        {
            // 使用 MessageBox 顯示輸入框，並取得使用者輸入的密碼
            return Microsoft.VisualBasic.Interaction.InputBox("權限輸入", "權限", "FQC");
        }
        private PasswordLevel CheckPasswordLevel(string password)
        {
            // 根據實際需求檢查密碼等級，這裡只是一個示例
            if (password == "PQC")
            {
                return PasswordLevel.High;
            }
            else
            {
                return PasswordLevel.Standard;
            }
        }

        private void HighLevelInitialization()
        {
            button2.Visible = true;
        }

        private void StandardInitialization()
        {
            button2.Visible = false;
        }

        // 定義密碼等級的列舉型別
        private enum PasswordLevel
        {
            High,
            Standard
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
                        button_X.Click += (sender, args) => ToggleGroup((Button)sender, newGroupBox);

                        break;
                    case 1:
                        axisYController = new AxisController("Y", _m114, 1, newGroupBox);
                        axisYController.UpdateTextBoxGroup(startY);
                        button_Y.Click += (sender, args) => ToggleGroup((Button)sender, newGroupBox);
                        break;
                    case 2:
                        axisZController = new AxisController("Z", _m114, 2, newGroupBox);
                        axisZController.UpdateTextBoxGroup(startY);
                        button_Z.Click += (sender, args) => ToggleGroup((Button)sender, newGroupBox);
                        break;

                    default:
                        break;
                }


            }

        }
        private void ToggleGroup(Button btn, GroupBox groupBox)
        {

            groupBox.SuspendLayout();
            // Toggle visibility of the group
            groupBox.Visible = !groupBox.Visible;

            AdjustGroupLayout();
            // Check if groupBox is a child of panel1
            if (panel1.Controls.Contains(groupBox))
            {
                // Change btn color to yellow
                btn.BackColor = Color.Yellow;
                btn.ForeColor = SystemColors.WindowFrame;
            }
            else
            {
                // Change btn color back to original
                btn.BackColor = SystemColors.WindowFrame;
                btn.ForeColor = Color.White;
            }
            groupBox.ResumeLayout(true);

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
                        groupBox.Visible = !groupBox.Visible;
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
        public void ExportQCreport()
        {
            var ioStates = groupBox_io.Controls
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
                    State = pCE_D122Wrapper.ReadInputBit(x.IoItem.Id)

                })
                .ToList();

            //添加 AxisConfigs 成員的示例數據
            var axisConfigs = new List<AxisConfig>{

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


            File.WriteAllText("QCreport.json", json);
        }


        public object CompareButtonStatesWithJson(MainConfig mainConfig, GroupBox groupBox)
        {
            // 讀取之前生成的JSON檔案
            string json = File.ReadAllText("QCreport.json");

            // 將JSON轉換為ReportData對象
            ReportData savedData = JsonConvert.DeserializeObject<ReportData>(json);

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
                    State = pCE_D122Wrapper.ReadInputBit(x.IoItem.Id)
                })
                .ToList();

            // 獲取當前的AxisConfigs狀態
            var currentAxisConfigs = new List<AxisConfig>{
        new AxisConfig(axisXController.AxisName, axisXController.AxisStatus),
        new AxisConfig(axisYController.AxisName, axisYController.AxisStatus),
        new AxisConfig(axisZController.AxisName, axisZController.AxisStatus)
    };

            var ioStateDifferences = new List<object>();
            var axisConfigDifferences = new List<object>();

            // 比對IO狀態
            foreach (var savedState in savedData.IOStates)
            {
                var currentState = currentStates.FirstOrDefault(cs => cs.Tag == savedState.Tag && cs.Id == savedState.Id);
                if (currentState == null || currentState.State != savedState.State)
                {
                    ioStateDifferences.Add(new
                    {
                        Tag = savedState.Tag,
                        Id = savedState.Id,
                        SavedState = savedState.State,
                        CurrentState = currentState != null ? currentState.State : false
                    });
                }
            }

            // 比對Axis配置
            foreach (var savedConfig in savedData.AxisConfigs)
            {
                var currentConfig = currentAxisConfigs.FirstOrDefault(cc => cc.AxisName == savedConfig.AxisName);
                if (currentConfig == null || currentConfig.AxisStatus != savedConfig.AxisStatus)
                {
                    axisConfigDifferences.Add(new
                    {
                        AxisName = savedConfig.AxisName,
                        SavedStatus = savedConfig.AxisStatus,
                        CurrentStatus = currentConfig != null ? currentConfig.AxisStatus : (UInt16)0
                    });
                }
            }

            return new { IOStateDifferences = ioStateDifferences, AxisConfigDifferences = axisConfigDifferences };
        }





        public DataTable CompareButtonStatesWithJson3(MainConfig mainConfig, GroupBox groupBox)
        {
            // 讀取JSON文件並反序列化
            string json = File.ReadAllText("QCreport.json");
            ReportData savedData = JsonConvert.DeserializeObject<ReportData>(json);


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
                    State = pCE_D122Wrapper.ReadInputBit(x.IoItem.Id)
                })
                .ToList();

            // 獲取當前的AxisConfigs狀態
            var currentAxisConfigs = new List<AxisConfig>{
        new AxisConfig(axisXController.AxisName, axisXController.AxisStatus),
        new AxisConfig(axisYController.AxisName, axisYController.AxisStatus),
        new AxisConfig(axisZController.AxisName, axisZController.AxisStatus)
    };



            // 創建並設置DataTable
            DataTable differencesTable = new DataTable();
            differencesTable.Columns.Add("類型", typeof(string));
            differencesTable.Columns.Add("名稱", typeof(string));
            differencesTable.Columns.Add("標準狀態碼", typeof(string));
            differencesTable.Columns.Add("當前狀態碼", typeof(string));
            differencesTable.Columns.Add("備註", typeof(string));


            // 比對IO狀態
            foreach (var savedState in savedData.IOStates)
            {
                var currentState = currentStates.FirstOrDefault(cs => cs.Tag == savedState.Tag && cs.Id == savedState.Id);
                var currentIOItem = mainConfig.IOConfig.Inputs.Concat(mainConfig.IOConfig.Outputs)
                      .FirstOrDefault(item => item.Id == savedState.Id);

                string ioItemRemark = currentIOItem?.Remark ?? "未知";


                if (currentState == null || currentState.State != savedState.State)
                {
                    differencesTable.Rows.Add(
                        "IO點",
                        savedState.Tag,
                        savedState.State.ToString(),
                        currentState != null ? currentState.State.ToString() : "不存在",
                        ioItemRemark
                    );
                }
            }

            // 比對Axis配置
            foreach (var savedConfig in savedData.AxisConfigs)
            {
                var currentConfig = currentAxisConfigs.FirstOrDefault(cc => cc.AxisName == savedConfig.AxisName);
                if (currentConfig == null || currentConfig.AxisStatus != savedConfig.AxisStatus)
                {
                    differencesTable.Rows.Add(
                        "軸",
                        savedConfig.AxisName,
                        savedConfig.AxisStatus.ToString(),
                        currentConfig != null ? currentConfig.AxisStatus.ToString() : "不存在"
                    );
                }
            }

            return differencesTable;
        }





        public class ReportData
        {
            public List<IO_State> IOStates { get; set; }
            public List<AxisConfig> AxisConfigs { get; set; }
        }
        public class IO_State
        {
            public int Id { get; set; }
            public string Tag { get; set; }
            public bool State { get; set; }
        }

        public bool CompareButtonStatesWithJson2(MainConfig mainConfig, GroupBox groupBox)
        {
            // 讀取之前生成的JSON檔案
            string json = File.ReadAllText("QCreport.json");

            // 將JSON轉換為ReportData對象
            ReportData savedData = JsonConvert.DeserializeObject<ReportData>(json);

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
                    State = pCE_D122Wrapper.ReadInputBit(x.IoItem.Id)
                })
                .ToList();

            // 獲取當前的AxisConfigs狀態
            var currentAxisConfigs = new List<AxisConfig>{
        new AxisConfig(axisXController.AxisName, axisXController.AxisStatus),
        new AxisConfig(axisYController.AxisName, axisYController.AxisStatus),
        new AxisConfig(axisZController.AxisName, axisZController.AxisStatus)
    };

            // 比對當前按鈕狀態與之前存儲的狀態
            bool ioStatesEqual = savedData.IOStates.Count == currentStates.Count &&
                                 savedData.IOStates.All(savedState =>
                                     currentStates.Any(currentState =>
                                         savedState.Tag == currentState.Tag &&
                                         savedState.Id == currentState.Id &&
                                         savedState.State == currentState.State
                                     )
                                 );

            // 比對當前AxisConfigs與之前存儲的狀態
            bool axisConfigsEqual = savedData.AxisConfigs.Count == currentAxisConfigs.Count &&
                                    savedData.AxisConfigs.All(savedConfig =>
                                        currentAxisConfigs.Any(currentConfig =>
                                            savedConfig.AxisName == currentConfig.AxisName &&
                                            savedConfig.AxisStatus == currentConfig.AxisStatus
                                        )
                                    );

            return ioStatesEqual && axisConfigsEqual;
        }


        private void LoadConfig()
        {
            // 保存當前激活的窗口

            // 創建和配置 OpenFileDialog
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Title = "選擇配置文件";
            //openFileDialog.Filter = "JSON文件 (*.json)|*.json"; // 篩選只顯示JSON文件
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // 設置初始目錄



            //// 顯示對話框
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    // 讀取選擇的文件的內容
            //    string jsonConfig = File.ReadAllText(openFileDialog.FileName);
            //    mainConfig1 = JsonConvert.DeserializeObject<MainConfig>(jsonConfig);
            //}


            string json = File.ReadAllText($"{typeFilePath}cof.json");

            mainConfig = JsonConvert.DeserializeObject<MainConfig>(json);


            if (mainConfig != null)
            {

                UpdateButtonLabels(mainConfig, groupBox_io);
                //ExportQCreport();
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
                                btn.TextAlign = ContentAlignment.MiddleCenter;
                                // 如果是Input，添加点击事件并设置背景色
                                if (mainConfig.IOConfig.Inputs.Any(input => input.Tag == buttonTag))
                                {
                                    btn.Click += (sender, e) => InputButtonClick(ioItem);
                                    bool inputState = pCE_D122Wrapper.ReadInputBit(ioItem.Id);
                                    btn.BackColor = inputState ? Color.LightGreen : Color.Green;
                                    btn.ForeColor = inputState ? Color.DarkSlateGray : Color.White;

                                }
                                // 如果是Output，添加点击事件
                                else if (mainConfig.IOConfig.Outputs.Any(output => output.Tag == buttonTag))
                                {
                                    btn.Click += (sender, e) => OutputButtonClick(ioItem);
                                    btn.ForeColor = Color.DarkSlateGray;
                                }
                            }
                        }
                    }
                }
            }
        }

        private object threadLock = new object();
        private void CheckAndUpdateIOStatus()
        {


            foreach (var input in mainConfig.IOConfig.Inputs)
            {
                bool inputState = pCE_D122Wrapper.ReadInputBit(input.Id);
                UpdateButtonColor(input, inputState);
            }
        }
        Thread ioThread;
        private void StartIOStatusCheckingThread()
        {
            ioThread = new Thread(() =>
            {

                while (!stopthread)
                {
                    CheckAndUpdateIOStatus();
                    Thread.Sleep(500);
                }



            });

            ioThread.IsBackground = true;
            ioThread.Start();
        }
        private void UpdateButtonColor(IOItem ioitem, bool status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateButtonColor(ioitem, status)));
            }
            else
            {
                Button buttonToUpdate = FindButtonByTag(ioitem.Tag);
                if (buttonToUpdate != null)
                {
                    buttonToUpdate.BackColor = status ? Color.LightGreen : Color.Green;
                    buttonToUpdate.ForeColor = status ? Color.DarkSlateGray : Color.White;
                }
            }
        }

        private Button FindButtonByTag(string tag)
        {
            foreach (Control panelControl in groupBox_io.Controls)
            {
                if (panelControl is Panel panel)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control is Button button && (string)button.Tag == tag)
                        {
                            return button;
                        }
                    }
                }
            }
            return null;
        }
        //private void UpdateButtonColor(IOItem ioitem, bool status)
        //{


        //        // 在这里更新按钮的颜色，根据IO状态
        //        if (InvokeRequired)
        //    {
        //        // 如果需要在UI线程上更新按钮，请使用Invoke方法
        //        Invoke(new Action(() => UpdateButtonColor(ioitem, status)));
        //    }
        //    else
        //    {

        //        foreach (Control panelControl in groupBox_io.Controls)
        //        {
        //            if (panelControl is Panel panel)
        //            {

        //                foreach (Control control in panel.Controls)
        //                {

        //                    if (control is Button button && (string)button.Tag == ioitem.Tag)
        //                    {

        //                        button.BackColor = status ? Color.LightGreen : Color.Green;
        //                    }
        //                }

        //            }
        //        }


        //    }

        //}
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




        private CancellationTokenSource ioThreadCancellation;

        private async Task CheckAndUpdateIOStatusAsync()
        {
            foreach (var input in mainConfig.IOConfig.Inputs)
            {
                bool inputState = await Task.Run(() => pCE_D122Wrapper.ReadInputBit(input.Id));
                UpdateButtonColor(input, inputState);
            }
        }

        private async Task StartIOStatusCheckingThreadAsync()
        {
            ioThreadCancellation = new CancellationTokenSource();

            try
            {
                while (!ioThreadCancellation.Token.IsCancellationRequested)
                {
                    await CheckAndUpdateIOStatusAsync();
                    await Task.Delay(100);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void StopIOStatusCheckingThread()
        {
            ioThreadCancellation?.Cancel();
        }







        private void t2()
        {

            DataTable differencesTable = CompareButtonStatesWithJson3(mainConfig, groupBox_io);

            if (differencesTable.Rows.Count > 0)
            {
                DifferencesForm differencesForm = new DifferencesForm();
                differencesForm.DataGridView.DataSource = differencesTable;
                differencesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("所有項目均匹配！", "比對結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                t2();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        Thread updateThread;
        public void StartUpdatingThread()
        {
            updateThread = new Thread(UpdateStatusLoop);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        bool stopthread = false;
        // 定期更新状态的循环
        private void UpdateStatusLoop()
        {
            while (!stopthread)
            {

                getAxisStatus();

                Thread.Sleep(500);
            }
        }

        void getAxisStatus()
        {

            axisXController.UpdateStatus();
            axisYController.UpdateStatus();
            axisZController.UpdateStatus();

        }


        private void button2_Click_1(object sender, EventArgs e)
        {


            ExportQCreport();




        }


        /// <summary>
        /// DediWareLog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void button29_Click(object sender, EventArgs e)
        {
            DirectoryInfo root = new DirectoryInfo(sourceFilePath);

            // 清空目標資料夾內容
            ClearTargetFolder(targetFilePath);

            var needFileList = GetNeedFile(root);
            foreach (var data in needFileList)
            {
                MoveFile(data.targetUrl, data.fileName);
            }

            Process.Start("explorer.exe", targetFilePath);
        }

        // 新增清空目標資料夾的方法
        private void ClearTargetFolder(string targetPath)
        {
            if (Directory.Exists(targetPath))
            {
                foreach (var file in Directory.GetFiles(targetPath))
                {
                    File.Delete(file);
                }
                foreach (var dir in Directory.GetDirectories(targetPath))
                {
                    Directory.Delete(dir, true);
                }
            }
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
                    else if (file.Name.Contains("scan"))
                    {
                        continue;
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
        /// <summary>
        /// Madata比對
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender; // 強制轉型為 Button

            var str1 = ReadXmlString($"{sourceFilePath}/MData.xml");
            var str2 = ReadXmlString($"{sourceFilePath}/MData_Modify.xml");
            var bl = mappingConfigXml(str1, str2);

            if (bl)
                clickedButton.BackColor = Color.GreenYellow; // 使用點擊的按鈕
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
            ToggleGroup((Button)sender, groupBox_io);
        }



        private void MainForm_Closed(object sender, FormClosedEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)

        {
            StopIOStatusCheckingThread();
        }

        private void button_X_Click(object sender, EventArgs e)
        {

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
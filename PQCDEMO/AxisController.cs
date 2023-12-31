﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPM;
using static System.Windows.Forms.AxHost;

namespace PQCDEMO
{
    public class AxisController
    {
        public UInt16 AxisStatus { get; private set; }  //軸的IO狀態
        public string AxisName { get; private set; } //軸的名稱
        public ushort AxisNumber { get; private set; }

        private MotionController motionController;
        public List<TextBox> TextBoxes { get; private set; }
        public GroupBox GroupBox { get; private set; }



        private static readonly Dictionary<int, string> defaultBitMeaningMapping = new Dictionary<int, string>
    {
        { 0, "RDY" },
        { 1, "ALM" },
        { 2, "+EL" },
        { 3, "-EL" },
        { 4, "ORG" },
        { 5, "DIR" },
        { 6, "EMG" },
        { 7, "N/A" },
        { 8, "ERC" },
        { 9, "EZ" },
        { 10, "N/A" },
        { 11, "Latch" },
        { 12, "SD" },
        { 13, "INP" },
        { 14, "SVON" },
        { 15, "RALM" }
    };

        // Constructor 接受一個 GroupBox 作為參數
        public AxisController(string axisName, MotionController motionCtl, ushort axisNumber,GroupBox groupBox)
        {
            AxisName = axisName;
            groupBox.Text= axisName;
            groupBox.ForeColor= Color.White;
            AxisNumber = axisNumber;
            motionController = motionCtl;
            GroupBox = groupBox;
            TextBoxes = new List<TextBox>();
            InitializeTextBoxes();
        }

        private void InitializeTextBoxes()
        {
            for (int i = 0; i < defaultBitMeaningMapping.Count; i++)
            {
                TextBox textBox = new TextBox
                {
                    Size = new Size(50,50),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Center,
                    Cursor = Cursors.Default,
                         
                };
                GroupBox.Controls.Add(textBox);
                TextBoxes.Add(textBox);
            }
        }

        public void UpdateTextBoxGroup(int startY)
        {
            int xOffset = 10;
            int yOffset = 10;
            const int textBoxWidth = 50;
            const int xPitch = 2;
            const int textBoxHeight = 50;

            for (int i = 0; i < TextBoxes.Count; i++)
            {
                TextBox textBox = TextBoxes[i];

                // 如果超出容器宽度，换行
                if (xOffset + textBoxWidth > GroupBox.Width)
                {
                    xOffset = 10;
                    yOffset += textBoxHeight; // 增加垂直间距
                }

                textBox.Location = new Point(xOffset, yOffset + startY);
                textBox.Name = $"{AxisName}info{i}";
                textBox.Text = $"{defaultBitMeaningMapping[i]}";
                xOffset += textBoxWidth+xPitch;
            }
        }

        public void UpdateStatus()
        {
            UInt16 axisStatus;
           
            if (motionController.GetAxisStatus(this.AxisNumber, out axisStatus))
            {
                this.AxisStatus = axisStatus; // 
                UpdateAxisStatus(); // 
            }
            else
            {
        
            }
        }



        private void UpdateAxisStatus()
        {
            if (GroupBox.InvokeRequired)
            {
                GroupBox.Invoke(new Action(UpdateAxisStatus));
            }
            for (int i = 0; i < TextBoxes.Count; i++)
            {
                bool isActive = IsFunctionActive(i);
                TextBoxes[i].BackColor = isActive ? Color.LightGreen : Color.Red;
                TextBoxes[i].ForeColor = isActive ? Color.Black : Color.White;
            }
        }

        private bool IsFunctionActive(int bitPosition)
        {
            return (AxisStatus & (1 << bitPosition)) != 0;
        }

        public bool AreAllFunctionsActive(HashSet<int> functionPositions)
        {
            foreach (int position in functionPositions)
            {
                if (!IsFunctionActive(position))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

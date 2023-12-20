using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PQCDEMO
{
    public partial class DifferencesForm : Form
    {
        public DataGridView DataGridView { get; private set; }

        public DifferencesForm()
        {
            InitializeComponent();
            DataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                
            };
            DataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView.CellFormatting += (s, e) =>
            {
                if (DataGridView.Columns[e.ColumnIndex].Name == "標準狀態碼")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                
                }
                if (DataGridView.Columns[e.ColumnIndex].Name == "當前狀態碼")
                {
                    e.CellStyle.BackColor = Color.OrangeRed;
                    e.CellStyle.ForeColor = Color.White;
                }
                if (DataGridView.Columns[e.ColumnIndex].Name == "備註")
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                }
            };
            this.Controls.Add(DataGridView);




        }

        public void SetDifferences(DataTable table)
        {
            DataGridView.DataSource = table;
        }


    }



}

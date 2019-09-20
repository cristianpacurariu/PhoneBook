using PhoneBook.Domain;
using PhoneBook.Infrastructure.Specific;
using PhoneBook.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class FormMain : Form
    {
        private readonly ISubscriberRepo<SubscriberDto, SubscriberFilterDto> _subscriberRepo = new SubscriberRepo();
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //adding columns to the dataGridView
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[]
                {
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Id", HeaderText = "Id", Name = "Id"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "FirstName", HeaderText = "First Name", Name = "First Name"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "LastName", HeaderText = "Last Name", Name = "Last Name"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "PhoneNumber", HeaderText = "Phone Number", Name = "Phone Number"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Details", HeaderText = "Details", Name = "Details"},
                };
            dataGridView.Columns.AddRange(columns);

            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            //dataGridView.DataSource = allSubscribers;

            for (int i = 0; i < allSubscribers.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView["Id", i].Value = allSubscribers[i].Id;
                dataGridView["First Name", i].Value = allSubscribers[i].FirstName;
                dataGridView["Last Name", i].Value = allSubscribers[i].LastName;
                dataGridView["Phone Number", i].Value = allSubscribers[i].PhoneNumber;
                dataGridView["Details", i].Value = allSubscribers[i].Details;
            }

            //some styling
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.RowsDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;

            dataGridView.EnableHeadersVisualStyles = false;

            //setting the cells font in the dataGridView
            using (Font font = new Font(dataGridView.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular))
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView.Columns[i].DefaultCellStyle.Font = font;
                }
            }

        }

        private void BtnSaveData_Click(object sender, EventArgs e)
        {

        }

        private void BtnLoadData_Click(object sender, EventArgs e)
        {
            //dataGridView.Rows.Clear();
            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            if (allSubscribers.Count == 0)
            {
                MessageBox.Show("There are no subscribers in the database");
            }
            else
            {
                dataGridView.DataSource = allSubscribers;
            }
        }

        private void SaveToToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void BackgroundColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();

            btnLoadData.BackColor = colorDialog.Color;
            btnSaveData.BackColor = colorDialog.Color;

            //dataGridView.BackgroundColor = colorDialog.Color;

            //dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colorDialog.Color;

            dataGridView.RowHeadersDefaultCellStyle.BackColor = colorDialog.Color;

            dataGridView.RowsDefaultCellStyle.BackColor = colorDialog.Color;
        }

        private void ChangeFontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.ShowDialog();

            btnLoadData.Font = fontDialog.Font;
            btnSaveData.Font = fontDialog.Font;

            //styling dataGridView headers
            dataGridView.Font = fontDialog.Font;

            //styling the cells in the dataGridView
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (fontDialog.Font.Bold)
                {
                    dataGridView.Columns[i].DefaultCellStyle.Font = new Font(fontDialog.Font, FontStyle.Regular);
                }
                else
                {
                    dataGridView.Columns[i].DefaultCellStyle.Font = fontDialog.Font;
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void SearchPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.Show();
        }
    }
}

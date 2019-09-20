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
        private int rowIndex = 0;

        public FormMain()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            //adding columns to the dataGridView
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[]
                {
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Id", HeaderText = "Id", Name = "Id", ReadOnly = false},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "FirstName", HeaderText = "First Name", Name = "First Name", ReadOnly = false},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "LastName", HeaderText = "Last Name", Name = "Last Name", ReadOnly = false},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "PhoneNumber", HeaderText = "Phone Number", Name = "Phone Number", ReadOnly = false},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Details", HeaderText = "Details", Name = "Details", ReadOnly = false},
                };
            dataGridView.Columns.AddRange(columns);

            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            BindingList<SubscriberDto> bindingList = new BindingList<SubscriberDto>();
            foreach (SubscriberDto subscriber in allSubscribers)
            {
                bindingList.Add(subscriber);
            }
            dataGridView.DataSource = bindingList;

            //for (int i = 0; i < bindingList.Count; i++)
            //{
            //    dataGridView.Rows.Add();
            //    dataGridView["Id", i].Value = bindingList[i].Id;
            //    dataGridView["First Name", i].Value = bindingList[i].FirstName;
            //    dataGridView["Last Name", i].Value = bindingList[i].LastName;
            //    dataGridView["Phone Number", i].Value = bindingList[i].PhoneNumber;
            //    dataGridView["Details", i].Value = bindingList[i].Details;
            //}

            //some styling
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.RowsDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;

            dataGridView.EnableHeadersVisualStyles = false;

            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.Font = new Font(dataGridView.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
            }
        }
        private void BtnSaveData_Click(object sender, EventArgs e)
        {
            //CType(dataGridView.DataSource, DataTable).GetChanges(DataRowState.Modified).Rows
        }

        private void BtnLoadData_Click(object sender, EventArgs e)
        {
            //dataGridView.Rows.Clear();

            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            BindingList<SubscriberDto> bindingList = new BindingList<SubscriberDto>();
            foreach (SubscriberDto subscriber in allSubscribers)
            {
                bindingList.Add(subscriber);
            }

            if (allSubscribers.Count == 0)
            {
                MessageBox.Show("There are no subscribers in the database");
            }
            else
            {
                dataGridView.DataSource = bindingList;
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
        private void DataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                //select entire row
                this.dataGridView.Rows[e.RowIndex].Selected = true;

                //is used when clicked on Delete Row
                this.rowIndex = e.RowIndex;

                //to prevent multiple rows being selected
                this.dataGridView.CurrentCell = this.dataGridView.Rows[e.RowIndex].Cells[1];

                //show the context menu
                this.contextMenu.Show(this.dataGridView, e.Location);

                //show the context menu at the cell where the right click was performed
                contextMenu.Show(Cursor.Position);
            }
        }

        private void AddRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            BindingList<SubscriberDto> bindingList = new BindingList<SubscriberDto>();
            foreach (SubscriberDto subscriber in allSubscribers)
            {
                bindingList.Add(subscriber);
            }

            bindingList.Add(new SubscriberDto { });
            dataGridView.DataSource = bindingList;
        }

        private void DeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView.Rows[this.rowIndex].IsNewRow && MessageBox.Show(
                $"Do you want to delete the row with Id = {dataGridView.Rows[this.rowIndex].Cells["Id"].Value} ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int idToDelete = Convert.ToInt32(dataGridView.Rows[this.rowIndex].Cells["Id"].Value);

                this.dataGridView.Rows.RemoveAt(this.rowIndex);

                if (_subscriberRepo.Delete(idToDelete))
                {
                    MessageBox.Show("Data succesfully deleted from the database");
                }
            }
        }
    }
}

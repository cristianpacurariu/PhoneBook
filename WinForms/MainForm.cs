using PhoneBook.Domain;
using PhoneBook.Infrastructure.Specific;
using PhoneBook.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WinForms
{
    public partial class FormMain : Form
    {
        private readonly ISubscriberRepo<SubscriberDto, SubscriberFilterDto> _subscriberRepo = new SubscriberRepo();
        private int rowIndex = 0;
        private List<int> toUpdateList = new List<int>();

        public FormMain()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            //adding columns to the dataGridView
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[]
                {
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Id", HeaderText = "Id", Name = "Id", ReadOnly = true},
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
            UpdateChangesInDatabase();

            AddNewRowsInDatabase();

            RefreshDataSource();
        }

        private void RefreshDataSource()
        {
            //Refresh datasource
            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            BindingList<SubscriberDto> bindingList = new BindingList<SubscriberDto>();
            foreach (SubscriberDto subscriber in allSubscribers)
            {
                bindingList.Add(subscriber);
            }
            dataGridView.DataSource = bindingList;
        }

        private void AddNewRowsInDatabase()
        {
            //Add new subscribers to the database
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow && (int)row.Cells["Id"].Value == 0)
                {
                    SubscriberDto toAdd = new SubscriberDto
                    {
                        FirstName = dataGridView.Rows[row.Index].Cells["First Name"].Value?.ToString(),
                        LastName = dataGridView.Rows[row.Index].Cells["Last Name"].Value?.ToString(),
                        PhoneNumber = dataGridView.Rows[row.Index].Cells["Phone Number"].Value?.ToString(),
                        Details = dataGridView.Rows[row.Index].Cells["Details"].Value?.ToString(),
                    };

                    try
                    {
                        _subscriberRepo.Add(toAdd);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occured while trying to add rows to the database.");
                    }
                }
            }
        }

        private void UpdateChangesInDatabase()
        {
            //Update changes in the database
            foreach (int id in toUpdateList)
            {
                //Find the row by id, where the changes took place
                int rowIndex = -1;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if ((int)row.Cells["Id"].Value == id)
                    {
                        rowIndex = row.Index;
                        break;
                    }
                }

                SubscriberDto subToUpdate = _subscriberRepo.Get(id);
                subToUpdate.Id = (int)dataGridView.Rows[rowIndex].Cells["Id"].Value;
                subToUpdate.FirstName = dataGridView.Rows[rowIndex].Cells["First Name"].Value?.ToString();
                subToUpdate.LastName = dataGridView.Rows[rowIndex].Cells["Last Name"].Value?.ToString();
                subToUpdate.PhoneNumber = dataGridView.Rows[rowIndex].Cells["Phone Number"].Value?.ToString();
                subToUpdate.Details = dataGridView.Rows[rowIndex].Cells["Details"].Value?.ToString();

                try
                {
                    _subscriberRepo.Update(subToUpdate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occured while trying to update the database.");
                }
                toUpdateList = new List<int>();
            }
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
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to save");
            }
            else
            {
                SaveFileDialog savefile = new SaveFileDialog
                {
                    FileName = $"Subscribers_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xml",
                    Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*"
                };

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.Combine("D:/Temp", savefile.FileName);
                    Serialize(dataGridView, fileName);
                    MessageBox.Show($"Data saved succesfully at {fileName}");
                }
            }
        }

        private static bool Serialize(DataGridView dgv, string fileName)
        {
            if (dgv.DataSource == null)
            {
                return false;
            }
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(dgv.DataSource.GetType());
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                xmlSerializer.Serialize(stream, dgv.DataSource);
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void BackgroundColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();

            btnLoadData.BackColor = colorDialog.Color;
            btnSaveData.BackColor = colorDialog.Color;

            //datagridview background
            dataGridView.BackgroundColor = colorDialog.Color;

            //columns headers
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colorDialog.Color;

            //row headers
            dataGridView.RowHeadersDefaultCellStyle.BackColor = colorDialog.Color;

            //data cells
            dataGridView.RowsDefaultCellStyle.BackColor = ControlPaint.Light(ControlPaint.Light(colorDialog.Color));
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

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int changedRowId = (int)dataGridView.Rows[e.RowIndex].Cells["Id"].Value;
            if (!toUpdateList.Contains(changedRowId) && changedRowId != 0)
            {
                toUpdateList.Add(changedRowId);
            }
        }
    }
}

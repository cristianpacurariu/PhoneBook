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
    public partial class SearchForm : Form
    {
        private readonly ISubscriberRepo<SubscriberDto, SubscriberFilterDto> _subscriberRepo = new SubscriberRepo();
        public SearchForm()
        {
            InitializeComponent();
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[]
                {
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Id", HeaderText = "Id"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "FirstName", HeaderText = "First Name"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "LastName", HeaderText = "Last Name"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "PhoneNumber", HeaderText = "Phone Number"},
                    new DataGridViewTextBoxColumn{ DataPropertyName = "Details", HeaderText = "Details"},
                };
            dataGridView.Columns.AddRange(columns);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.RowsDefaultCellStyle.BackColor = Color.LightGray;

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

        private void SearchForm_Load(object sender, EventArgs e)
        {
            List<SubscriberDto> allSubscribers = _subscriberRepo.All();
            dataGridView.DataSource = allSubscribers;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SubscriberFilterDto filter = new SubscriberFilterDto { textToSearchFor = tbSearch.Text };
            List<SubscriberDto> filteredList = _subscriberRepo.Filter(filter);

            dataGridView.DataSource = filteredList;
        }

        //to avoid error on double-click
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            
        }
    }
}

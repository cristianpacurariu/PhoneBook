using PhoneBook.Domain;
using PhoneBook.Infrastructure.Specific;
using PhoneBook.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private readonly ISubscriberRepo<SubscriberDto, SubscriberFilterDto> _subscriberRepo = new SubscriberRepo();
        public SearchWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            searchDataGrid.Items.Clear();

            List<SubscriberDto> subscriberDtos = _subscriberRepo.All();
            foreach (SubscriberDto subscriber in subscriberDtos)
            {
                searchDataGrid.Items.Add(subscriber);
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchDataGrid.Items.Clear();

            SubscriberFilterDto filter = new SubscriberFilterDto { textToSearchFor = tbSearch.Text };
            List<SubscriberDto> filteredList = _subscriberRepo.Filter(filter);

            foreach (SubscriberDto item in filteredList)
            {
                searchDataGrid.Items.Add(item);

            }
        }
    }
}

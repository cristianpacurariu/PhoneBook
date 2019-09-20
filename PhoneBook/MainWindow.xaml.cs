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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace PhoneBook
{
    public partial class MainWindow : Window
    {
        private readonly ISubscriberRepo<SubscriberDto, SubscriberFilterDto> _subscriberRepo = new SubscriberRepo();
        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            DataGridXAML.Items.Clear();
            //DataGridTextColumn Id = new DataGridTextColumn { Header = "Id", Binding = new Binding("SubscriberDto.Id"), FontSize = 22 };
            //DataGridTextColumn FirstName = new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName"), FontSize = 26 };
            //DataGridTextColumn lastName = new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName") };
            //DataGridTextColumn phoneNumber = new DataGridTextColumn { Header = "Phone Number", Binding = new Binding("PhoneNumber") };
            //DataGridTextColumn Details = new DataGridTextColumn { Header = "Details", Binding = new Binding("Details") };

            ////DataGridXAML.Columns.Add(Id);
            //DataGridXAML.Columns.Add(FirstName);
            //DataGridXAML.Columns.Add(lastName);
            //DataGridXAML.Columns.Add(phoneNumber);
            //DataGridXAML.Columns.Add(Details);

            List<SubscriberDto> subscriberDtos = _subscriberRepo.All();
            foreach (SubscriberDto subscriber in subscriberDtos)
            {
                DataGridXAML.Items.Add(subscriber);
            }
        }
        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            DataGridXAML.Items.Clear();

            List<SubscriberDto> subscriberDtos = _subscriberRepo.All();

            if (subscriberDtos.Count == 0)
            {
                MessageBox.Show("There are no subscribers in the database");
            }
            foreach (SubscriberDto subscriber in subscriberDtos)
            {
                DataGridXAML.Items.Add(subscriber);
            }
        }

        private void SaveDataBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in DataGridXAML.Items)
            {

            }

            //SubscriberDto item = new SubscriberDto();

            //_subscriberRepo.Add(item);
        }


        private void MenuPreferences_Click(object sender, RoutedEventArgs e)
        {
            PreferencesWindow preferencesWindow = new PreferencesWindow();
            preferencesWindow.Show();
        }

        private void MenuSaveToFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Show();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

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

namespace Baibakov4122UfanetCourseWork
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private static TextBlock CCTB, ACTB;

        public ClientsPage(TextBlock CurrentCountTB, TextBlock AllCountTB)
        {
            InitializeComponent();
            CCTB = CurrentCountTB;
            ACTB = AllCountTB;

            var currentClients = UFANETEntities.GetContext().Clients.ToList();
            ClientsListView.ItemsSource = currentClients;

            CCTB.Text = currentClients.Count.ToString();
            ACTB.Text = UFANETEntities.GetContext().Clients.Count().ToString();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SomethingHasChanged();
        }

        private void ClientEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClientAddContractButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClientContractButton_Click(object sender, RoutedEventArgs e)
        {
            var client_contracts = (sender as Button).DataContext as Clients;

            string answer = "";

            if (client_contracts.contracts == "")
            {
                answer = "     Отсутствуют     ";
            } else
            {
                foreach (var contract in client_contracts.contracts.Split(' '))
                {
                    answer = answer + "     " + contract + "     \n";
                }
            }

            MessageBox.Show
                (
                    answer,
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
        }

        private void SomethingHasChanged()
        {
            var currentClients = UFANETEntities.GetContext().Clients.ToList();

            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                currentClients = currentClients.Where(p => p.full_name.ToLower().Contains(SearchTB.Text.ToLower())).ToList().
                    Union(currentClients.Where(p => p.email.ToLower().Contains(SearchTB.Text.ToLower())).ToList()).ToList().
                    Union(currentClients.Where(p => p.phone.ToLower().Contains(SearchTB.Text.ToLower())).ToList()).ToList().
                    Union(currentClients.Where(p => p.passport_data.ToLower().Contains(SearchTB.Text.ToLower())).ToList()).ToList().
                    Union(currentClients.Where(p => p.address.ToLower().Contains(SearchTB.Text.ToLower())).ToList()).ToList().
                    Union(currentClients.Where(p => p.contracts.ToLower().Contains(SearchTB.Text.ToLower())).ToList()).ToList();
            }

            ClientsListView.ItemsSource = currentClients;
            CCTB.Text = currentClients.Count.ToString();
            ACTB.Text = UFANETEntities.GetContext().Clients.Count().ToString();
        }
    }
}

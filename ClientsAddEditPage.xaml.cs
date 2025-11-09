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
    /// Логика взаимодействия для ClientsAddEditPage.xaml
    /// </summary>
    public partial class ClientsAddEditPage : Page
    {
        public ClientsAddEditPage(Clients client)
        {
            InitializeComponent();

            ClientIDTextBox.Text = client.client_id.ToString();
            ClientLastNameTextBox.Text = client.last_name;
            ClientFirstNameTextBox.Text = client.first_name;
            ClientMiddleNameTextBox.Text = client.middle_name;
            ClientPassportTextBox.Text = client.passport_data;
            ClientAddressTextBox.Text = client.address;
            ClientPhoneTextBox.Text = client.phone;
            ClientEmailTextBox.Text = client.email;
            ClientSignUpDateTextBox.Text = client.registration_date.ToShortDateString();
            ClientBirthDateTextBox.Text = client.birthday == null ? "Отсутствует" : ((System.DateTime)client.birthday).ToShortDateString();
            ClientPhotoImage.Source = new BitmapImage(new Uri("pack://application:,,,/" + client.photoOptimized));
        }

        private void EditCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
            Manager.MainTextBlock.Text = "Список клиентов";
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

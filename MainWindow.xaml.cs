using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool is_clients_image = false;
        private static ClientsPage ClientsPageTo;
        private static TariffsPage TariffsPageTo;

        private static ClientsPage CPage(TextBlock CCTB, TextBlock ACTB)
        {
            if (ClientsPageTo == null)
            {
                ClientsPageTo = new ClientsPage(CCTB, ACTB);
            }

            return ClientsPageTo;
            
        }

        private static TariffsPage TPage()
        {
            if (TariffsPageTo == null)
            {
                TariffsPageTo = new TariffsPage();
            }

            return TariffsPageTo;

        }

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(CPage(CurrentCountTB, AllCountTB));
            Manager.MainFrame = MainFrame;
            Manager.MainTextBlock = MainPageTextBlock;
        }

        

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            back_btn.Visibility = Visibility.Hidden;

           // TODO
           /* if (MainFrame.CanGoBack)
            {
                back_btn.Visibility = Visibility.Visible;
            }
            else
            {
                back_btn.Visibility = Visibility.Hidden;
            }*/
        }

        private void PageChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (is_clients_image)
            {
                MainFrame.Navigate(CPage(CurrentCountTB, AllCountTB));
                Manager.MainFrame = MainFrame;
                Manager.MainTextBlock.Text = "Список клиентов";

                PageRouteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/tariffs_page.png"));
                is_clients_image = false;
            } else
            {
                MainFrame.Navigate(TPage());
                Manager.MainFrame = MainFrame;
                Manager.MainTextBlock.Text = "Список тарифов";

                PageRouteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/clients_page.png"));
                is_clients_image = true;
            }
        }
    }
}
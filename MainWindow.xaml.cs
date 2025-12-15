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

        private static ClientsPage CPage(TextBlock CCTB, TextBlock ACTB)
        {
            if (ClientsPageTo == null)
            {
                ClientsPageTo = new ClientsPage(CCTB, ACTB);
            }

            return ClientsPageTo;
        }

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(CPage(CurrentCountTB, AllCountTB));
            Manager.MainFrame = MainFrame;
            Manager.MainTextBlock = MainPageTextBlock;
            MainFrame.Navigated += MainFrame_Navigated;
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Показываем кнопку "Назад", если есть куда возвращаться
            back_btn.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;

            // Обновляем заголовок
            if (e.Content is Page page)
            {
                // Устанавливаем заголовок в зависимости от типа страницы
                if (page is ClientsPage)
                {
                    MainPageTextBlock.Text = "Список клиентов";
                }
                else if (page is ClientsAddEditPage)
                {
                    // Заголовок будет установлен в самой странице ClientsAddEditPage
                    // через Manager.MainTextBlock.Text
                    // Поэтому здесь ничего не делаем
                }
                // Добавьте другие страницы по мере необходимости
                else if (page.Title != null)
                {
                    MainPageTextBlock.Text = page.Title;
                }
            }
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            // Устаревший метод, можно удалить или оставить пустым
        }

        private void PageChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (is_clients_image)
            {
                MainFrame.Navigate(CPage(CurrentCountTB, AllCountTB));
                Manager.MainFrame = MainFrame;
                Manager.MainTextBlock.Text = "Список клиентов";

                is_clients_image = false;
            }
       
        }
    }
}
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
        private List<Clients> _allClients;
        private bool _isPageLoaded = false;

        public ClientsPage(TextBlock CurrentCountTB, TextBlock AllCountTB)
        {
            InitializeComponent();
            CCTB = CurrentCountTB;
            ACTB = AllCountTB;

            // Подписываемся на событие загрузки страницы
            this.Loaded += ClientsPage_Loaded;

            // Подписываемся на событие навигации на эту страницу
            if (NavigationService != null)
            {
                NavigationService.Navigated += NavigationService_Navigated;
            }
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            // Если вернулись на эту страницу, обновляем данные
            if (e.Content == this && _isPageLoaded)
            {
                LoadClients();
                ApplyFiltersAndSort();
            }
        }

        private void ClientsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _isPageLoaded = true;
            LoadClients();
            ApplyFiltersAndSort();

            // Убедимся, что подписаны на событие навигации
            if (NavigationService != null)
            {
                NavigationService.Navigated += NavigationService_Navigated;
            }
        }

        private void LoadClients()
        {
            // Очищаем старый список и загружаем заново
            _allClients = UFANETEntities.GetContext().Clients.ToList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isPageLoaded)
                ApplyFiltersAndSort();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isPageLoaded)
                ApplyFiltersAndSort();
        }

        private void FilterRadioButton_Changed(object sender, RoutedEventArgs e)
        {
            if (_isPageLoaded)
                ApplyFiltersAndSort();
        }

        private void ApplyFiltersAndSort()
        {
            // Проверяем, что страница загружена
            if (!_isPageLoaded) return;

            // Проверяем, что элементы управления инициализированы
            if (WithContractsRadioButton == null || WithoutContractsRadioButton == null)
                return;

            // Используем контекст БД для проверки наличия договоров
            var db = UFANETEntities.GetContext();

            var filteredClients = _allClients.AsEnumerable();

            // Поиск по всем полям
            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                string searchText = SearchTB.Text.ToLower();
                filteredClients = filteredClients.Where(p =>
                    p.full_name.ToLower().Contains(searchText) ||
                    (p.email != null && p.email.ToLower().Contains(searchText)) ||
                    p.phone.ToLower().Contains(searchText) ||
                    (p.passport_data != null && p.passport_data.ToLower().Contains(searchText)) ||
                    p.address.ToLower().Contains(searchText));
            }

            // Фильтрация с RadioButton
            if (WithContractsRadioButton.IsChecked == true)
            {
                // Фильтруем клиентов с договорами
                filteredClients = filteredClients.Where(p =>
                    db.Contracts.Any(c => c.client_id == p.client_id && c.status == "Активен"));
            }
            else if (WithoutContractsRadioButton.IsChecked == true)
            {
                // Фильтруем клиентов без договоров
                filteredClients = filteredClients.Where(p =>
                    !db.Contracts.Any(c => c.client_id == p.client_id && c.status == "Активен"));
            }

            // Применяем сортировку
            var sortedClients = filteredClients.ToList();

            switch (SortComboBox.SelectedIndex)
            {
                case 1: // По ФИО (А-Я)
                    sortedClients = sortedClients.OrderBy(p => p.last_name)
                                                .ThenBy(p => p.first_name)
                                                .ThenBy(p => p.middle_name).ToList();
                    break;
                case 2: // По ФИО (Я-А)
                    sortedClients = sortedClients.OrderByDescending(p => p.last_name)
                                                .ThenByDescending(p => p.first_name)
                                                .ThenByDescending(p => p.middle_name).ToList();
                    break;
                case 3: // По дате регистрации (новые)
                    sortedClients = sortedClients.OrderByDescending(p => p.registration_date).ToList();
                    break;
                case 4: // По дате регистрации (старые)
                    sortedClients = sortedClients.OrderBy(p => p.registration_date).ToList();
                    break;
            }

            ClientsListView.ItemsSource = sortedClients;
            CCTB.Text = sortedClients.Count.ToString();
            ACTB.Text = _allClients.Count.ToString();
        }

        private void ClientEditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientsAddEditPage((sender as Button).DataContext as Clients));
            Manager.MainTextBlock.Text = "Редактирование клиента";
        }

        private void ClientAddContractButton_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as Button).DataContext as Clients;
            MessageBox.Show($"Добавление договора для клиента: {client.full_name}",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClientContractButton_Click(object sender, RoutedEventArgs e)
        {
            var client_contracts = (sender as Button).DataContext as Clients;
            string answer = "";

            if (string.IsNullOrEmpty(client_contracts.contracts))
            {
                answer = "У клиента нет активных договоров";
            }
            else
            {
                answer = "Активные договоры клиента:\n\n";
                foreach (var contract in client_contracts.contracts.Split(' '))
                {
                    if (!string.IsNullOrEmpty(contract))
                        answer = answer + "• " + contract + "\n";
                }
            }

            MessageBox.Show(answer, "Договоры клиента",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClientAddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientsAddEditPage(null));
        }
    }
}
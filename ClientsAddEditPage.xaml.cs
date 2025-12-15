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
// Удалите эту строку: using PathIO = System.IO.Path;
using System.IO;

namespace Baibakov4122UfanetCourseWork
{
    /// <summary>
    /// Логика взаимодействия для ClientsAddEditPage.xaml
    /// </summary>
    public partial class ClientsAddEditPage : Page
    {
        private Clients _currentClient = null;
        private string _photoPath = "";
        private bool _isPhotoChanged = false;

        public ClientsAddEditPage(Clients client)
        {
            InitializeComponent();

            _currentClient = client;

            if (client != null) // Режим редактирования
            {
                ClientLastNameTextBox.Text = client.last_name;
                ClientFirstNameTextBox.Text = client.first_name;
                ClientMiddleNameTextBox.Text = client.middle_name;
                ClientPassportTextBox.Text = client.passport_data;
                ClientAddressTextBox.Text = client.address;
                ClientPhoneTextBox.Text = client.phone;
                ClientEmailTextBox.Text = client.email;

                // Устанавливаем даты
                if (client.birthday.HasValue)
                    ClientBirthDatePicker.SelectedDate = client.birthday.Value;
                else
                    ClientBirthDatePicker.SelectedDate = new DateTime(1990, 1, 1);

                ClientSignUpDatePicker.SelectedDate = client.registration_date;

                // Загружаем фото
                LoadClientPhoto(client);

                Manager.MainTextBlock.Text = "Редактирование клиента";
                DeleteBtn.Visibility = Visibility.Visible;
            }
            else // Режим добавления
            {
                ClientSignUpDatePicker.SelectedDate = DateTime.Today;
                ClientBirthDatePicker.SelectedDate = new DateTime(1990, 1, 1);
                ClientBirthDatePicker.IsEnabled = true;
                LoadDefaultPhoto();
                Manager.MainTextBlock.Text = "Добавление клиента";
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadClientPhoto(Clients client)
        {
            if (!string.IsNullOrEmpty(client.photo))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", client.photo);
                    if (File.Exists(fullPath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(fullPath);
                        bitmap.EndInit();
                        ClientPhotoImage.Source = bitmap;
                        _photoPath = fullPath;
                    }
                    else
                    {
                        LoadDefaultPhoto();
                    }
                }
                catch
                {
                    LoadDefaultPhoto();
                }
            }
            else
            {
                LoadDefaultPhoto();
            }
        }

        private void LoadDefaultPhoto()
        {
            try
            {
                ClientPhotoImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/user_default.png"));
            }
            catch
            {
                // Если стандартное фото не найдено, оставляем пустым
            }
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Validator validator = new Validator();
            StringBuilder errors = new StringBuilder();

            // Проверка всех полей
            if (!validator.FirstName(ClientFirstNameTextBox.Text))
                errors.AppendLine("Некорректное имя");
            if (!validator.MiddleName(ClientMiddleNameTextBox.Text))
                errors.AppendLine("Некорректное отчество");
            if (!validator.LastName(ClientLastNameTextBox.Text))
                errors.AppendLine("Некорректная фамилия");
            if (!validator.Passport(ClientPassportTextBox.Text))
                errors.AppendLine("Некорректные паспортные данные (формат: 4510 123456)");
            if (!validator.Address(ClientAddressTextBox.Text))
                errors.AppendLine("Некорректный адрес (формат: г. Москва, ул. Ленина, д.10, кв.5)");
            if (!validator.Phone(ClientPhoneTextBox.Text))
                errors.AppendLine("Некорректный телефон (формат: +7(912)345-67-89)");
            if (!validator.Email(ClientEmailTextBox.Text))
                errors.AppendLine("Некорректный email");

            // Проверка даты рождения
            if (!ClientBirthDatePicker.SelectedDate.HasValue)
                errors.AppendLine("Укажите дату рождения");
            else if (!validator.IsAdult(ClientBirthDatePicker.SelectedDate.Value,
                                       ClientSignUpDatePicker.SelectedDate ?? DateTime.Today))
                errors.AppendLine("Клиент должен быть совершеннолетним на момент регистрации");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Сохранение фото
                string photoFileName = "";
                if (_isPhotoChanged && !string.IsNullOrEmpty(_photoPath) && File.Exists(_photoPath))
                {
                    string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    photoFileName = $"{ClientLastNameTextBox.Text.ToLower()}_{Guid.NewGuid().ToString().Substring(0, 8)}.jpg";
                    string destinationFile = System.IO.Path.Combine(destinationPath, photoFileName);

                    // Копируем файл с перезаписью
                    File.Copy(_photoPath, destinationFile, true);
                }
                else if (_currentClient != null && !_isPhotoChanged)
                {
                    // Оставляем старое фото при редактировании
                    photoFileName = _currentClient.photo;
                }

                if (_currentClient == null) // Добавление нового клиента
                {
                    Clients newClient = new Clients()
                    {
                        last_name = ClientLastNameTextBox.Text,
                        first_name = ClientFirstNameTextBox.Text,
                        middle_name = string.IsNullOrWhiteSpace(ClientMiddleNameTextBox.Text) ? null : ClientMiddleNameTextBox.Text,
                        phone = ClientPhoneTextBox.Text,
                        email = ClientEmailTextBox.Text,
                        address = ClientAddressTextBox.Text,
                        passport_data = ClientPassportTextBox.Text,
                        birthday = ClientBirthDatePicker.SelectedDate,
                        registration_date = ClientSignUpDatePicker.SelectedDate ?? DateTime.Today,
                        photo = string.IsNullOrEmpty(photoFileName) ? null : photoFileName
                    };

                    UFANETEntities.GetContext().Clients.Add(newClient);
                }
                else // Редактирование существующего клиента
                {
                    _currentClient.last_name = ClientLastNameTextBox.Text;
                    _currentClient.first_name = ClientFirstNameTextBox.Text;
                    _currentClient.middle_name = string.IsNullOrWhiteSpace(ClientMiddleNameTextBox.Text) ? null : ClientMiddleNameTextBox.Text;
                    _currentClient.phone = ClientPhoneTextBox.Text;
                    _currentClient.email = ClientEmailTextBox.Text;
                    _currentClient.address = ClientAddressTextBox.Text;
                    _currentClient.passport_data = ClientPassportTextBox.Text;
                    _currentClient.birthday = ClientBirthDatePicker.SelectedDate;

                    if (_isPhotoChanged)
                    {
                        _currentClient.photo = string.IsNullOrEmpty(photoFileName) ? null : photoFileName;
                    }
                }

                UFANETEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PhotoChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if (dlg.ShowDialog() == true)
                {
                    _photoPath = dlg.FileName;
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(_photoPath);
                    bitmap.EndInit();
                    ClientPhotoImage.Source = bitmap;
                    _isPhotoChanged = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке фото: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentClient == null)
                return;

            // Логическое условие для удаления: клиент должен быть без активных договоров
            var hasActiveContracts = UFANETEntities.GetContext().Contracts
                .Any(c => c.client_id == _currentClient.client_id && c.status == "Активен");

            if (hasActiveContracts)
            {
                MessageBox.Show("Невозможно удалить клиента с активными договорами!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить клиента?\nЭто действие нельзя отменить.",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем все договоры клиента (если есть)
                    var contracts = UFANETEntities.GetContext().Contracts
                        .Where(c => c.client_id == _currentClient.client_id).ToList();
                    if (contracts.Any())
                    {
                        UFANETEntities.GetContext().Contracts.RemoveRange(contracts);
                    }

                    // Удаляем клиента
                    UFANETEntities.GetContext().Clients.Remove(_currentClient);
                    UFANETEntities.GetContext().SaveChanges();

                    MessageBox.Show("Клиент успешно удален!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
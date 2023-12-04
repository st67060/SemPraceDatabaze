using Aplikace.data;
using Aplikace.data.Entity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Aplikace.dialog
{
    public partial class UserDialog : Window
    {
        ObservableCollection<User> users;
        ObservableCollection<Employee> employees;
        DataAccess access;
        public UserDialog()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadUsers();
        }

        

        private async void LoadUsers()
        {
            await Task.Run(() =>
            {
                //users = new ObservableCollection<User>(access.GetUsers().Result());
                employees = new ObservableCollection<Employee>(access.GetEmployees().Result);

                Dispatcher.Invoke(() =>
                {
                    dgUsers.ItemsSource = users;
                    cmbEmployee.ItemsSource = employees;
                });
            });


        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления нового пользователя
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            // Логика для изменения существующего пользователя
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления пользователя
        }

        private void closeButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        // Остальные методы...
    }
}


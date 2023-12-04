using Aplikace.data;
using Aplikace.data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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
                users = new ObservableCollection<User>(access.GetUsers());
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
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text) && cmbEmployee.SelectedItem != null)
            {
                User user = new User(0, txtName.Text, txtPassword.Text, (Employee)cmbEmployee.SelectedItem);
                access.InsertUser(user);
                LoadUsers();
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text) && cmbEmployee.SelectedItem != null)
                {
                    User selectedUser = (User)dgUsers.SelectedItem;
                    selectedUser.Name = txtName.Text;
                    selectedUser.Password = txtPassword.Text;
                    selectedUser.Employee = (Employee)cmbEmployee.SelectedItem;
                    access.UpdateUser(selectedUser);
                    LoadUsers();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                User selectedUser = (User)dgUsers.SelectedItem;

                if (access.DeleteUser(selectedUser))
                {
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                User selectedUser = (User)dgUsers.SelectedItem;
                DataContext = selectedUser;
                List<Employee> tempEmployee = employees.ToList();
                int indexEmployee = tempEmployee.FindIndex(emp => emp.Id == selectedUser.Employee.Id);
                cmbEmployee.SelectedIndex = indexEmployee;
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        
    }
}

using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Aplikace.dialog
{
    public partial class DialogEmployee : Window
    {
        DataAccess access;
        ObservableCollection<Employee> employees;


        public Array roles => Enum.GetValues(typeof(Role));


        public DialogEmployee()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove(); 
            LoadEmployees();
        }

        
        private void LoadEmployees()
        {

            Task.Run(async () =>
            {
                employees = new ObservableCollection<Employee>(await access.GetEmployees());
                


                Dispatcher.Invoke(() =>
                {

                    dgEmployee.ItemsSource = employees;
                    cmbRole.ItemsSource = Enum.GetValues(typeof(Role)).Cast<Role>();

                });
            });
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployee.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)dgEmployee.SelectedItem;
                Employee employee = new Employee(selectedEmployee.Id, txtName.Text, txtSurname.Text,
                                                 (DateTime)dpHireDate.SelectedDate, selectedEmployee.Photo, (Role)cmbRole.SelectedItem);
                employee.Superior = selectedEmployee.Superior;
                access.UpdateEmployee(employee);
                LoadEmployees();
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            
            Employee employee = new Employee(0, txtName.Text, txtSurname.Text, DateTime.Now, Role.Employee);
            access.InsertEmployee(employee);
            LoadEmployees();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployee.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)dgEmployee.SelectedItem;
                if (access.DeleteEmployee(selectedEmployee))
                {
                    LoadEmployees();
                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void dgEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployee.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)dgEmployee.SelectedItem;
                detailStackPanel.DataContext = selectedEmployee;
                
            }
        }
    }
}


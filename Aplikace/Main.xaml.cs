﻿using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Aplikace
{
    public partial class Main : Window
    {
        private User user;
        private DataList data;
        private DataAccess access;
        public Main(User user)
        {
            data = new DataList();
            access = new DataAccess();
            this.user = user;
            InitializeComponent();
            DataContext = user;
            SetAccessToData(user);
            
            LoadListOfEmployees();
            //employees.Add(user.Employee);
            //employeeDataGrid.ItemsSource = employees;
        }
       

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void SetAccessToData(User user)
        {
            mainTabControl.SelectedItem = mainMenuTabItem;
            PatientTabItem.Visibility = Visibility.Collapsed;
            UserTabItem.Visibility = Visibility.Collapsed;
            chatTabItem.Visibility = Visibility.Collapsed;
            settingsTabItem.Visibility = Visibility.Collapsed;
            listTabItem.Visibility = Visibility.Collapsed;
            calendarTabItem.Visibility = Visibility.Collapsed;  

            if (user.Employee.Role == Role.Admin)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;
                chatTabItem.Visibility = Visibility.Visible;
                settingsTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Doctor || user.Employee.Role == Role.Nurse)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;
                chatTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Employee)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;
                chatTabItem.Visibility = Visibility.Visible;
                

            }


        }
        private void SetAccessForQuest() {
            calendarTabItem.IsEnabled = false;
            calendarTabItem.Visibility = Visibility.Hidden;
            chatTabItem.IsEnabled = false;
            chatTabItem.Visibility = Visibility.Hidden;
            settingsTabItem.IsEnabled = false;
            settingsTabItem.Visibility = Visibility.Hidden;
            listTabItem.IsEnabled = false;
            listTabItem.Visibility = Visibility.Hidden;


        }
        private BitmapImage SetProfileImage(User user)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(user.Employee.Photo);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private void LoadListOfEmployees() {
            ObservableCollection<Employee> doctorsAndNurses = new ObservableCollection<Employee>();
            foreach (Employee emp in access.GetEmployees()) {
                if ((int)emp.Role == 2 || (int)emp.Role == 3) {
                doctorsAndNurses.Add(emp);
                }
            
            }
            data.Employees = doctorsAndNurses;              
            employeeDataGrid.ItemsSource = data.Employees;
        
        }
    }
}

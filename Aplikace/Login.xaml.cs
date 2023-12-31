﻿using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.dialog;
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

namespace Aplikace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void loginButton_Click_1(object sender, RoutedEventArgs e)
        {
            string enteredName = UsernameTextBox.Text;
            string enteredPassword = PasswordBox.Password;
            DataAccess dataAccess = new DataAccess();
            User user = dataAccess.GetUserWithEmployee(enteredName, enteredPassword);
            if (user != null)
            {
                Main main = new Main(user);
                main.Show();
                Close();
            }
            else {
                errorBorder.Visibility = Visibility.Visible;
                errorMessage.Text = "Invalid Username or Password";
            }

        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            Close();
        }

        private void loginWithoutAcount_Click(object sender, RoutedEventArgs e)
        {


            Employee employee = new Employee(0, "", "", DateTime.Now, data.Enum.Role.Quest);
            User user = new User(0, "", "", employee);
            Main main = new Main(user);
            main.Show();
            Close();
        }



        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            errorBorder.Visibility = Visibility.Hidden;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            errorBorder.Visibility = Visibility.Hidden;
        }
    }
}
using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Aplikace
{
    public partial class Main : Window
    {
        private User user;
        private DataList data;
        public Main(User user)
        {
            data = new DataList();
            this.user = user;
            InitializeComponent();
            DataContext = user;
            SetAccessToData(user);
            userImage.Source = SetProfileImage(user);
            //employees.Add(user.Employee);
            //employeeDataGrid.ItemsSource = employees;
        }
        public Main()
        {
            
            User temp = new User("Quest");
            this.user = temp;
            DataContext = user;
            InitializeComponent();
            SetAccessForQuest();
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void SetAccessToData(User user)
        {
            chatTabItem.IsEnabled = false;
            chatTabItem.Visibility = Visibility.Hidden;
            settingsTabItem.IsEnabled = false;
            settingsTabItem.Visibility = Visibility.Hidden;
            listTabItem.IsEnabled = false;
            listTabItem.Visibility = Visibility.Hidden;

            if (user.Employee.Role == Role.Admin)
            {

                chatTabItem.IsEnabled = true;
                chatTabItem.Visibility = Visibility.Visible;
                settingsTabItem.IsEnabled = true;
                settingsTabItem.Visibility = Visibility.Visible;
                listTabItem.IsEnabled = true;
                listTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Doctor || user.Employee.Role == Role.Nurse)
            {
                chatTabItem.IsEnabled = true;
                chatTabItem.Visibility = Visibility.Visible;
                listTabItem.IsEnabled = true;
                listTabItem.Visibility = Visibility.Visible;

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
    }
}

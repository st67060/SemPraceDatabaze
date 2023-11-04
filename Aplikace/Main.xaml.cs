using Aplikace.data.Entity;
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
using System.Windows.Shapes;

namespace Aplikace
{
    /// <summary>
    /// Interakční logika pro Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private User user;
        public Main(User user)
        {
            this.user = user;
            InitializeComponent();
            DataContext = user;
            SetAccessToData(user);

        }
        public Main()
        {
            
            User temp = new User("Quest");
            this.user = temp;
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

            if (user.Employee.Role == data.Enum.Role.Admin)
            {

                chatTabItem.IsEnabled = true;
                chatTabItem.Visibility = Visibility.Visible;
                settingsTabItem.IsEnabled = true;
                settingsTabItem.Visibility = Visibility.Visible;
                listTabItem.IsEnabled = true;
                listTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == data.Enum.Role.Doctor || user.Employee.Role == data.Enum.Role.Nurse)
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
    }
}

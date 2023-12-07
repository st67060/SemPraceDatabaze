using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Aplikace.dialog
{
    /// <summary>
    /// Interaction logic for DialogRole.xaml
    /// MessageBox.Show("No reservation selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public partial class DialogRole : Window
    {
        DataAccess access;
        public Array roles => Enum.GetValues(typeof(Role));

        public DialogRole()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadRoles();

        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Unable to add another enum", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void dgRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRole.SelectedItem != null)
            {
               
                var selectedRole = (Role)dgRole.SelectedItem;

                
                cmbRole.SelectedItem = selectedRole;
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Unable to modify enum", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Unable to delete  enum", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void LoadRoles()
        {
            var roleList = Enum.GetValues(typeof(Role)).Cast<Role>().ToList();

            Dispatcher.Invoke(() =>
            {
                dgRole.ItemsSource = roleList;
                cmbRole.ItemsSource = roleList; 
            });
        }

    }
}

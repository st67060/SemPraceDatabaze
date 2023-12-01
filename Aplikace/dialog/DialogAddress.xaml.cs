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
using Aplikace.data.Entity;
using System.Collections.ObjectModel;
using Aplikace.data;
using System.Net;

namespace Aplikace.dialog
{
    public partial class DialogAddress : Window
    {
        DataAccess access;
        ObservableCollection<Address> addresses;

        public DialogAddress()
        {
            access = new DataAccess();

            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove(); // Allows the dialog to be moved
            LoadAddresses();
        }

        private void LoadAddresses()
        {
            addresses = new ObservableCollection<Address>(access.GetAllAddresses());
            dgAddress.ItemsSource = addresses;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddress.SelectedItem != null)
            {
                Address selectedAddress = (Address)dgAddress.SelectedItem;
                Address address = new Address(selectedAddress.Id, txtCity.Text, int.Parse(txtPostalCode.Text),
                                              int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
                access.UpdateAddress(address);
                LoadAddresses();
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            Address address = new Address(0, txtCity.Text, int.Parse(txtPostalCode.Text),
                                          int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
            access.InsertAddress(address);
            LoadAddresses();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddress.SelectedItem != null)
            {
                Address selectedAddress = (Address)dgAddress.SelectedItem;

                if (access.DeleteAddress(selectedAddress))
                {
                    LoadAddresses();
                }
                else {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }



        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void dgAddress_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Address selectedAddress = (Address)dgAddress.SelectedItem;
            DataContext = selectedAddress;
        }
    }
}


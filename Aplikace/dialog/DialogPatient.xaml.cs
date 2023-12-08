using Aplikace.data;
using Aplikace.data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace Aplikace.dialog
{
    public partial class DialogPatient : Window
    {
        DataAccess access;
        ObservableCollection<Patient> patients;
        ObservableCollection<Address> addresses;
        ObservableCollection<Insurance> insurances;
        ObservableCollection<HealthCard> healthCards;

        public DialogPatient()
        {

            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadPatients();
        }

        private async void LoadPatients()
        {
            await Task.Run(() =>
            {
                patients = new ObservableCollection<Patient>(access.GetAllPatients().Result);
                addresses = new ObservableCollection<Address>(access.GetAllAddresses());
                insurances = new ObservableCollection<Insurance>(access.GetAllInsurances());
                healthCards = new ObservableCollection<HealthCard>(access.GetAllHealthCards());

                Dispatcher.Invoke(() =>
                {
                    dgPatients.ItemsSource = patients;
                    cmbAddress.ItemsSource = addresses;
                    cmbInsuranceCompany.ItemsSource = insurances;
                    cmbHealthCard.ItemsSource = healthCards;
                });
            });
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem != null && dpDateOfBirth.SelectedDate.HasValue)
            {
                Patient patient = new Patient(0, txtFirstName.Text, txtLastName.Text, long.Parse(txtSSN.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate.Value, long.Parse(txtPhone.Text), txtEmail.Text, (Address)cmbAddress.SelectedItem, (HealthCard)cmbHealthCard.SelectedItem, (Insurance)cmbInsuranceCompany.SelectedItem);
                access.InsertPatient(patient);
                LoadPatients();

            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem != null && dpDateOfBirth.SelectedDate.HasValue)
            {
                Patient temp = (Patient)dgPatients.SelectedItem;
                Patient patient = new Patient(temp.Id, txtFirstName.Text, txtLastName.Text, long.Parse(txtSSN.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate.Value, long.Parse(txtPhone.Text), txtEmail.Text, (Address)cmbAddress.SelectedItem, (HealthCard)cmbHealthCard.SelectedItem, (Insurance)cmbInsuranceCompany.SelectedItem);
                access.UpdatePatient(temp);
                LoadPatients();

            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem != null)
            {
                var selectedPatient = (Patient)dgPatients.SelectedItem;



                if (access.DeletePatient(selectedPatient))
                {
                    LoadPatients();
                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPatients.SelectedItem != null)
            {
                var selectedPatient = (Patient)dgPatients.SelectedItem;
                DataContext = selectedPatient;
                List<Address> tempAddress = addresses.ToList();
                List<Insurance> tempInsurance = insurances.ToList();
                List<HealthCard> tempHealthCards = healthCards.ToList();
                int indexAddress = tempAddress.FindIndex(adr => adr.Id == selectedPatient.Address.Id);
                int indexInsurance = tempInsurance.FindIndex(ins => ins.Id == selectedPatient.InsuranceCompany.Id);
                int indexHealthCards = tempHealthCards.FindIndex(hlc => hlc.Id == selectedPatient.HealthCard.Id);
                cmbAddress.SelectedIndex = indexAddress;
                cmbInsuranceCompany.SelectedIndex = indexInsurance;
                cmbHealthCard.SelectedIndex = indexHealthCards;
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private bool IsTextual(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private void txtFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }

        private void txtLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }

        private void txtGender_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void txtSSN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }

        
    }
}

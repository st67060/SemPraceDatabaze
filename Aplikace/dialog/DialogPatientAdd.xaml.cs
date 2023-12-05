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
    /// Interakční logika pro DialogPatientAdd.xaml
    /// </summary>
    public partial class DialogPatientAdd : Window
    {
        public Patient patient;
        public Address address;
        public HealthCard healthCard;
        private DataAccess access;
        public DialogPatientAdd()
        {
            access = new DataAccess();
            patient = null;
            address = null;
            healthCard = null;
            InitializeComponent();
            btnConfirm.Content = "Create";
            cmbAnamnesis.ItemsSource = access.GetAllAnamnesis();
            cmbInsuranceCompany.ItemsSource = access.GetAllInsurances();
            MouseLeftButtonDown += (s, e) => DragMove();

        }
        public DialogPatientAdd(Patient patient)

        {
            access = new DataAccess();
            this.patient = patient;
            address = patient.Address;
            healthCard = patient.HealthCard;
            DataContext = patient;
            InitializeComponent();
            btnConfirm.Content = "Edit";
            MouseLeftButtonDown += (s, e) => DragMove();
            List<Anamnesis> anamneses = access.GetAllAnamnesis();
            cmbAnamnesis.ItemsSource = anamneses;
            List<Insurance> insurances = access.GetAllInsurances();
            cmbInsuranceCompany.ItemsSource = insurances;

            int indexAnamnesis = anamneses.FindIndex(ana => ana.Id == patient.HealthCard.Anamnesis.Id);
            int indexInsurance = insurances.FindIndex(ins => ins.Id == patient.InsuranceCompany.Id);
            cmbAnamnesis.SelectedIndex = indexAnamnesis;
            cmbInsuranceCompany.SelectedIndex = indexInsurance;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _); 
        }
        private bool IsTextNotEmpty(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(patient != null)
            {
                if (ValidateAddress() && ValidateHealthCard() && ValidatePatient())
                {
                    address = new Address(address.Id, txtCity.Text, int.Parse(txtPostalCode.Text), int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
                    healthCard = new HealthCard(healthCard.Id, (bool)chkSmokes.IsChecked, (bool)chkPregnancy.IsChecked, (bool)chkAlcohol.IsChecked, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                    patient = new Patient(patient.Id, txtFirstName.Text, txtLastName.Text, long.Parse(txtSocialSecurityNumber.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate, long.Parse(txtPhone.Text), txtEmail.Text, address, healthCard, (Insurance)cmbInsuranceCompany.SelectedItem);
                    DataAccess access = new DataAccess();
                    DialogResult = access.UpdatePatient(patient);
                    if (DialogResult == true)
                    {
                        Close();
                    }
                }

            }
            else
            {
                if (ValidateAddress() && ValidateHealthCard() && ValidatePatient())
                {
                    address = new Address(0, txtCity.Text, int.Parse(txtPostalCode.Text), int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
                    healthCard = new HealthCard(0, (bool)chkSmokes.IsChecked, (bool)chkPregnancy.IsChecked, (bool)chkAlcohol.IsChecked, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                    patient = new Patient(0, txtFirstName.Text, txtLastName.Text, long.Parse(txtSocialSecurityNumber.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate, long.Parse(txtPhone.Text), txtEmail.Text, address, healthCard, (Insurance)cmbInsuranceCompany.SelectedItem);
                    DataAccess access = new DataAccess();
                    DialogResult = access.InsertPatient(patient);
                    if (DialogResult == true)
                    {
                        Close();
                    }
                }

            }
            
            
        }

        private void txtSocialSecurityNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; 
               
            }

        }
        private bool ValidateAddress()
        {
            if (!IsTextNotEmpty(txtCity.Text) ||
                !IsTextNotEmpty(txtPostalCode.Text) ||
                !IsTextNotEmpty(txtStreetNumber.Text) ||
                !IsTextNotEmpty(txtCountry.Text) ||
                !IsTextNotEmpty(txtStreet.Text))
            {
                MessageBox.Show("Some fields for the ADDRESS are empty", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool ValidateHealthCard()
        {
            if (!IsTextNotEmpty(txtSport.Text) ||
                !IsTextNotEmpty(txtFillings.Text) ||
                cmbAnamnesis.SelectedItem == null
                )
            {
                MessageBox.Show("Some fields for the HEALTH CARD are empty", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool ValidatePatient()
        {
            if (!IsTextNotEmpty(txtFirstName.Text) ||
                !IsTextNotEmpty(txtLastName.Text) ||
                !IsTextNotEmpty(txtSocialSecurityNumber.Text) ||
                !IsTextNotEmpty(txtGender.Text) ||
                dpDateOfBirth.SelectedDate == null ||
                !IsTextNotEmpty(txtPhone.Text) ||
                !IsTextNotEmpty(txtEmail.Text) ||
                cmbInsuranceCompany.SelectedItem == null)
            {
                MessageBox.Show("Some fields for the PATIENT are empty", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;

            }
        }

        private void txtFillings_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;

            }
        }

        private void txtPostalCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;

            }
        }

        private void txtStreetNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;

            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

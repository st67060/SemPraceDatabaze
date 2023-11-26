using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
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
        public DialogPatientAdd()
        {
            patient = null;
            address = null;
            healthCard = null;
            InitializeComponent();
            cmbAnamnesis.ItemsSource = Enum.GetValues(typeof(Anamnesis));
            cmbInsuranceCompany.ItemsSource = Enum.GetValues(typeof(InsuranceCompany));

        }
        public DialogPatientAdd(Patient patient)

        {
            this.patient = patient;
            address = patient.Address;
            healthCard = patient.HealthCard;
            DataContext = patient;
            InitializeComponent();
            cmbAnamnesis.ItemsSource = Enum.GetValues(typeof(Anamnesis));
            cmbInsuranceCompany.ItemsSource = Enum.GetValues(typeof(InsuranceCompany));

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(patient != null)
            {
                address = new Address(address.Id, txtCity.Text, int.Parse(txtPostalCode.Text), int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
                healthCard = new HealthCard(healthCard.Id, (bool)chkSmokes.IsChecked, (bool)chkPregnancy.IsChecked, (bool)chkAlcohol.IsChecked, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                patient = new Patient(patient.Id, txtFirstName.Text, txtLastName.Text, long.Parse(txtSocialSecurityNumber.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate, long.Parse(txtPhone.Text), txtEmail.Text, address, healthCard, (InsuranceCompany)cmbInsuranceCompany.SelectedItem);
                DataAccess access = new DataAccess();
                DialogResult = access.UpdatePatient(patient);
                if (DialogResult == true)
                {
                    Close();
                }

            }
            else
            {
                address = new Address(0, txtCity.Text, int.Parse(txtPostalCode.Text), int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
                healthCard = new HealthCard(0, (bool)chkSmokes.IsChecked, (bool)chkPregnancy.IsChecked, (bool)chkAlcohol.IsChecked, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                patient = new Patient(0, txtFirstName.Text, txtLastName.Text, long.Parse(txtSocialSecurityNumber.Text), txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate, long.Parse(txtPhone.Text), txtEmail.Text, address, healthCard, (InsuranceCompany)cmbInsuranceCompany.SelectedItem);
                DataAccess access = new DataAccess();
                DialogResult = access.InsertPatient(patient);
                if (DialogResult == true)
                {
                    Close();
                }

            }
            
            
        }
        
    }
}

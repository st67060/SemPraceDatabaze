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
        public Patient Patient;
        public Address Address;
        public HealthCard HealthCard;
        public DialogPatientAdd()
        {
            InitializeComponent();
            cmbAnamnesis.ItemsSource = Enum.GetValues(typeof(Anamnesis));
            cmbInsuranceCompany.ItemsSource = Enum.GetValues(typeof(InsuranceCompany));

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Address = new Address(0, txtCity.Text, int.Parse(txtPostalCode.Text), int.Parse(txtStreetNumber.Text), txtCountry.Text, txtStreet.Text);
            HealthCard = new HealthCard(0, (bool)chkSmokes.IsChecked, (bool)chkPregnancy.IsChecked, (bool)chkAlcohol.IsChecked, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
            Patient = new Patient(0, txtFirstName.Text, txtLastName.Text, txtSocialSecurityNumber.Text, txtGender.Text, (DateTime)dpDateOfBirth.SelectedDate, txtPhone.Text, txtEmail.Text, Address, HealthCard,(InsuranceCompany)cmbInsuranceCompany.SelectedItem);
       DataAccess access = new DataAccess();
            access.InsertPatient(Patient,Address, HealthCard);
        }
        
    }
}

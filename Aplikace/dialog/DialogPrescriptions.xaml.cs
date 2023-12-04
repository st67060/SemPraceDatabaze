using Aplikace.data.Entity;
using Aplikace.data;
using Microsoft.VisualBasic;
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
    /// Interaction logic for DialogPrescriptions.xaml
    /// </summary>
    public partial class DialogPrescriptions : Window
    {
        DataAccess access;
        ObservableCollection<Prescription> precriptions;
        ObservableCollection<Patient> patients;
        ObservableCollection<Employee> employees;

        public DialogPrescriptions()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadPrescription();
        }

        private async void LoadPrescription ()
        {
            await Task.Run(() =>
            {
                precriptions = new ObservableCollection<Prescription>(access.GetAllPrescriptions().Result);
                patients = new ObservableCollection<Patient>(access.GetAllPatients().Result);
                employees = new ObservableCollection<Employee>(access.GetEmployees().Result);

                Dispatcher.Invoke(() =>
                {
                    dgPrescription.ItemsSource = precriptions;
                    cmbPatient.ItemsSource = patients;
                    cmbEmployee.ItemsSource = employees;
                });
            });
        }
        
        private void dgPrescription_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPrescription.SelectedItem != null)
            {
                var selectedPrescription = (Prescription)dgPrescription.SelectedItem;
                DataContext = selectedPrescription;
                List<Employee> tempEmployee = employees.ToList();
                List<Patient> tempPatient = patients.ToList();
                int indexEmployee = tempEmployee.FindIndex(emp => emp.Id == selectedPrescription.Employee.Id);
                int indexPatient = tempPatient.FindIndex(pat => pat.Id == selectedPrescription.Patient.Id);
                cmbPatient.SelectedIndex = indexPatient;
                cmbEmployee.SelectedIndex = indexEmployee;
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrescription.SelectedItem != null)
            {
                if (dpDate.SelectedDate != null)
                {
                    Prescription temp = (Prescription)dgPrescription.SelectedItem;
                    Prescription prescription = new Prescription(temp.ID, txtDrugName.Text, decimal.Parse(txtSupplement.Text),  (Employee)cmbEmployee.SelectedItem, (Patient)cmbPatient.SelectedItem, (DateTime)dpDate.SelectedDate);
                    access.UpdatePrescription(temp);
                    LoadPrescription();
                }
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrescription.SelectedItem != null)
            {
                var selectedPrescription = (Prescription)dgPrescription.SelectedItem;



                if (access.DeletePrescription(selectedPrescription))
                {
                    LoadPrescription();
                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            
            
                Prescription precription = new Prescription(0, txtDrugName.Text, decimal.Parse(txtSupplement.Text),  (Employee)cmbEmployee.SelectedItem, (Patient)cmbPatient.SelectedItem, (DateTime)dpDate.SelectedDate);
                access.InsertPrescription(precription);
                LoadPrescription();
            
        }
    }
}

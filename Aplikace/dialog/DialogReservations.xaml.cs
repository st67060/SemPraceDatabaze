using Aplikace.data;
using Aplikace.data.Entity;
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
using System.Xml.Linq;

namespace Aplikace.dialog
{
    public partial class DialogReservations : Window
    {
        
            DataAccess access;
            ObservableCollection<Reservation> reservations;
            ObservableCollection<Patient> patients;
            ObservableCollection<Employee> employees;

            public DialogReservations()
            {
                access = new DataAccess();

                InitializeComponent();
                MouseLeftButtonDown += (s, e) => DragMove();
                LoadReservations();
            }

            private void LoadReservations()
            {
                reservations = new ObservableCollection<Reservation>(access.GetAllReservations().Result);
                patients = new ObservableCollection<Patient>(access.GetAllPatients().Result);
                employees = new ObservableCollection<Employee>(access.GetEmployees().Result);

            }

            private void AddNew_Click(object sender, RoutedEventArgs e)
            {
                if (dpDate.SelectedDate != null && !string.IsNullOrWhiteSpace(txtNotes.Text) && cmbPatient.SelectedItem != null && cmbEmployee.SelectedItem != null)
                {
                    Reservation reservation = new Reservation(0, txtNotes.Text, (DateTime)dpDate.SelectedDate, (Patient)cmbPatient.SelectedItem, (Employee)cmbEmployee.SelectedItem);
                    access.InsertReservation(reservation);
                    LoadReservations();
                }
            }
        
        private void Modify_Click(object sender, RoutedEventArgs e)
            {
                if (dgReservations.SelectedItem != null)
                {
                    if (dpDate.SelectedDate != null && !string.IsNullOrWhiteSpace(txtNotes.Text) && cmbPatient.SelectedItem != null && cmbEmployee.SelectedItem != null)
                    {
                    Reservation selectedReservation = (Reservation)dgReservations.SelectedItem;
                    Patient selectedPatient = (Patient)cmbPatient.SelectedItem;
                    Employee selectedEmployee = (Employee)cmbEmployee.SelectedItem;
                    Reservation reservation = new Reservation(selectedReservation.Id, txtNotes.Text, dpDate.SelectedDate.Value, selectedPatient, selectedEmployee);
                    access.UpdateReservation(selectedReservation);
                    LoadReservations();
                    }
                }
            }

            private void Delete_Click(object sender, RoutedEventArgs e)
            {
                if (dgReservations.SelectedItem != null)
                {
                   access.DeleteReservation((Reservation)dgReservations.SelectedItem);
                    LoadReservations();
                }
            }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

     

        private void dgReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReservations.SelectedItem != null)
            {
                var selectedReservation = (Reservation)dgReservations.SelectedItem;
                DataContext = selectedReservation;

            }
        }
    }
}

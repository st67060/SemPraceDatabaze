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
                MouseLeftButtonDown += (s, e) => DragMove(); // позволяет перемещать окно
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
                    // access.InsertReservation(reservation);
                    LoadReservations();
                }
            }

            private void Modify_Click(object sender, RoutedEventArgs e)
            {
                if (dgReservations.SelectedItem != null)
                {
                    if (dpDate.SelectedDate != null && !string.IsNullOrWhiteSpace(txtNotes.Text) && cmbPatient.SelectedItem != null && cmbEmployee.SelectedItem != null)
                    {
                        Reservation temp = (Reservation)dgReservations.SelectedItem;
                        temp.Date = (DateTime)dpDate.SelectedDate;
                        temp.Notes = txtNotes.Text;
                        temp.Patient = (Patient)cmbPatient.SelectedItem;
                        temp.Employee = (Employee)cmbEmployee.SelectedItem;
                        // access.UpdateReservation(temp);
                        LoadReservations();
                    }
                }
            }

            private void Delete_Click(object sender, RoutedEventArgs e)
            {
                if (dgReservations.SelectedItem != null)
                {
                    // access.DeleteReservation((Reservation)dgReservations.SelectedItem);
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

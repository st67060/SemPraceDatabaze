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
    public partial class DialogReservations : Window
    {

        DataAccess access;
        ObservableCollection<Reservation> reservations;
        ObservableCollection<Patient> patients;
        ObservableCollection<Employee> employees;

        public DialogReservations()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove(); 
            LoadReservations();
        }

        private async void LoadReservations()
        {
            await Task.Run(() =>
            {
                reservations = new ObservableCollection<Reservation>(access.GetAllReservations().Result);
                patients = new ObservableCollection<Patient>(access.GetAllPatients().Result);
                employees = new ObservableCollection<Employee>(access.GetEmployees().Result);

                Dispatcher.Invoke(() =>
                {
                    dgReservations.ItemsSource = reservations;
                    cmbPatient.ItemsSource = patients;
                    cmbEmployee.ItemsSource = employees;
                });
            });


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
                    Reservation temp = (Reservation)dgReservations.SelectedItem;
                    Reservation reservation = new Reservation(temp.Id, txtNotes.Text, (DateTime)dpDate.SelectedDate, (Patient)cmbPatient.SelectedItem, (Employee)cmbEmployee.SelectedItem);
                    access.UpdateReservation(temp);
                    LoadReservations();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgReservations.SelectedItem != null)
            {
                var selectedReservation = (Reservation)dgReservations.SelectedItem;



                if (access.DeleteReservation(selectedReservation))
                {
                    LoadReservations();
                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void dgReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReservations.SelectedItem != null)
            {
                var selectedReservation = (Reservation)dgReservations.SelectedItem;
                DataContext = selectedReservation;
                
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

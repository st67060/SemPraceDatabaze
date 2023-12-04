using Aplikace.data;
using Aplikace.data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Aplikace.dialog
{

    public partial class DialogVisits : Window
    {
        DataAccess access;
        ObservableCollection<Visit> visits;
        ObservableCollection<Patient> patients;
        public DialogVisits()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadVisits();
            

        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                if (dpDate.SelectedDate != null && !string.IsNullOrWhiteSpace(txtDetails.Text) && cmbPatient.SelectedItem != null)
                {
                    Visit temp = (Visit)dgVisits.SelectedItem;
                    Visit visit = new Visit(temp.Id, (DateTime)dpDate.SelectedDate, txtDetails.Text, (Patient)cmbPatient.SelectedItem);
                    access.UpdateVisit(visit);
                    LoadVisits();
                }


            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate != null && string.IsNullOrWhiteSpace(txtDetails.Text) && cmbPatient.SelectedItem != null)
            {
                Visit visit = new Visit(0, (DateTime)dpDate.SelectedDate, txtDetails.Text, (Patient)cmbPatient.SelectedItem);
                access.InsertVisit(visit);
                LoadVisits();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                Visit selectedVisit = (Visit)dgVisits.SelectedItem;

                if (access.DeleteVisit(selectedVisit))
                {
                    LoadVisits();
                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgVisits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                var visit = (Visit)dgVisits.SelectedItem;
                DataContext = visit;
                List<Patient> tempPatient = patients.ToList();                
                int indexPatient = tempPatient.FindIndex(pat => pat.Id == visit.Patient.Id);
                cmbPatient.SelectedIndex = indexPatient;
            }
        }
        private void LoadVisits()
        {

            Task.Run(async () =>
            {
                visits = new ObservableCollection<Visit>(await access.GetAllVisits());
                patients = new ObservableCollection<Patient>(await access.GetAllPatients());


                Dispatcher.Invoke(() =>
                {

                    dgVisits.ItemsSource = visits;
                    cmbPatient.ItemsSource = patients;
                });
            });
        }

        

        private void closeButton_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

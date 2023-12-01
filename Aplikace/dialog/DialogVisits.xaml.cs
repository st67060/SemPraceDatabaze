using Aplikace.data;
using Aplikace.data.Entity;
using System;
using System.Collections.ObjectModel;
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
                       
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove(); // lze s dialogem hybat
            LoadVisits();
            

        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                if (dpDate.SelectedDate != null && string.IsNullOrWhiteSpace(txtDetails.Text) && cmbPatient.SelectedItem != null)
                {
                    Visit temp = (Visit)dgVisits.SelectedItem;
                    Visit visit = new Visit(temp.Id, (DateTime)dpDate.SelectedDate, txtDetails.Text, (Patient)cmbPatient.SelectedItem);
                    // access.UpdateVisit(Visit);
                    LoadVisits();
                }
                
                
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate != null && string.IsNullOrWhiteSpace(txtDetails.Text) && cmbPatient.SelectedItem != null)
            {
                Visit visit = new Visit(0, (DateTime)dpDate.SelectedDate, txtDetails.Text, (Patient)cmbPatient.SelectedItem);
                // access.InsertVisit();
                LoadVisits();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                // access.DeleteVisit((Patient)dgVisits.SelectedItem);
                LoadVisits();
            }
        }

        private void dgVisits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVisits.SelectedItem != null)
            {
                var visit = (Visit)dgVisits.SelectedItem;
                DataContext = visit;

            }
        }
        private void LoadVisits()
        {
            
            Task.Run(async () =>
            {
                visits = new ObservableCollection<Visit>( access.GetAllVisits().Result);
                patients = new ObservableCollection<Patient>( access.GetAllPatients().Result);


                Dispatcher.Invoke(() =>
                {
                    dgVisits.ItemsSource = visits;
                    cmbPatient.ItemsSource = patients;
                });
            });
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

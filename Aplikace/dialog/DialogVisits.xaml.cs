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
    /// <summary>
    /// Логика взаимодействия для DialogVisits.xaml
    /// </summary>
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
                DataContext = (Patient)dgVisits.SelectedItem;

            }
        }
        private void LoadVisits()
        {
            visits = new ObservableCollection<Visit>(access.GetAllVisits());
            patients = new ObservableCollection<Patient>(access.GetAllPatients());
            dgVisits.ItemsSource = visits;
            cmbPatient.ItemsSource = patients;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

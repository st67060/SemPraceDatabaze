using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.dialog;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Aplikace
{

    public partial class Main : Window
    {
        private User user;
        private DataList data;
        private DataAccess access;
        private Patient patient;
        public Main(User user)
        {
            data = new DataList();
            access = new DataAccess();
            this.user = user;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            DataContext = user;
            SetAccessToData(user);
            LoadListOfPatients();
            LoadListOfUsers();
            LoadListOfEmployees();
            LoadListOfLogs();
            LoadListOfRezervations();
            
        }


        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void SetAccessToData(User user)
        {
            mainTabControl.SelectedItem = mainMenuTabItem;
            PatientTabItem.Visibility = Visibility.Collapsed;
            UserTabItem.Visibility = Visibility.Collapsed;
            dataTabItem.Visibility = Visibility.Collapsed;
            settingsTabItem.Visibility = Visibility.Collapsed;
            listTabItem.Visibility = Visibility.Collapsed;
            calendarTabItem.Visibility = Visibility.Collapsed;

            if (user.Employee.Role == Role.Admin)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;
                dataTabItem.Visibility = Visibility.Visible;
                settingsTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Doctor || user.Employee.Role == Role.Nurse)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Employee)
            {
                userImage.Source = SetProfileImage(user);
                UserTabItem.Visibility = Visibility.Visible;


            }


        }

        private BitmapImage SetProfileImage(User user)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(user.Employee.Photo);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private void LoadListOfEmployees()
        {
            ObservableCollection<Employee> doctorsAndNurses = new ObservableCollection<Employee>();
            foreach (Employee emp in access.GetEmployees())
            {
                if ((int)emp.Role == 2 || (int)emp.Role == 3)
                {
                    doctorsAndNurses.Add(emp);
                }

            }
            data.Employees = doctorsAndNurses;
            employeeDataGrid.ItemsSource = data.Employees;

        }
        private void LoadListOfPatients()
        {
            data.Patients.Clear();
            data.Patients = new ObservableCollection<Patient>(access.GetAllPatients());
            patientDataGrid.ItemsSource = data.Patients;

        }
        private void LoadListOfUsers()
        {
            data.Users = new ObservableCollection<User>(access.GetAllUsers());
            usersDataGrid.ItemsSource = data.Users;
        }
        private void LoadListOfLogs()
        {


            data.Logs = new ObservableCollection<Log>(access.GetAllLogs());
            logsDataGrid.DataContext = data.Logs[0];
            logsDataGrid.ItemsSource = data.Logs;

        }
        private void LoadListOfRezervations()
        {
            data.Reservations.Clear();
            data.Reservations = new ObservableCollection<Reservation>(access.GetReservations());
            dgReservations.ItemsSource = data.Reservations;
            

        }


        private void patientDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (patientDataGrid.SelectedItem != null) {
                patient = (Patient)patientDataGrid.SelectedItem;
                addressDataStackPanel.DataContext = patient;
            }
            
        }

        private void emulateUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (usersDataGrid.SelectedIndex > 0)
            {
                User emulatedUser = usersDataGrid.SelectedItem as User;
                Main emulatedMain = new Main(emulatedUser);
                emulatedMain.Show();
            }

        }

        private void usersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (usersDataGrid.SelectedIndex > 0)
            {
                User temp = usersDataGrid.SelectedItem as User;
                if (temp.Employee.Role != Role.Admin)
                {
                    emulateUserButton.Visibility = Visibility.Visible;
                }
                else
                {
                    emulateUserButton.Visibility = Visibility.Hidden;
                }


            }
            else
            {
                emulateUserButton.Visibility = Visibility.Hidden;
            }

        }

        private void addPatient_Click(object sender, RoutedEventArgs e)
        {

            DialogPatientAdd dialogPatientAdd = new DialogPatientAdd();
            var result = dialogPatientAdd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                LoadListOfPatients();
            }
            else
            {

            }

        }

        private void deletePatient_Click(object sender, RoutedEventArgs e)
        {
            if(patientDataGrid.SelectedItem!=null)
                
            {
                Patient patient = (Patient)patientDataGrid.SelectedItem;
                if (access.DeletePatient(patient)) {
                    data.Patients.Remove(patient);
                }
            }
            
        }

        private void editPatient_Click(object sender, RoutedEventArgs e)
        {
            if (patientDataGrid.SelectedItem != null)

            {
                Patient patient = (Patient)patientDataGrid.SelectedItem;

                DialogPatientAdd dialogPatientAdd = new DialogPatientAdd(patient);
                var result = dialogPatientAdd.ShowDialog();

                if (result.HasValue && result.Value)
                {

                    LoadListOfPatients();
                }
                else
                {

                }
            }
            
        }

        private void settingsTabItem_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            LoadListOfLogs();
        }

        private void AddReservation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProcedure_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgReservations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            dgProcedures.ItemsSource = data.Reservations[dgReservations.SelectedIndex].Procedures;
        }

        private void btnAdressDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogAddress dialogAddress = new DialogAddress();
            dialogAddress.ShowDialog();
        }

        private void btnAnamnesisDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogAnamnesis dialogAnamnesis = new DialogAnamnesis();
            dialogAnamnesis.ShowDialog();
        }

        private void btnAllergyDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogAllergy dialogAllergy = new DialogAllergy();
            dialogAllergy.ShowDialog();
        }

        private void btnInsuranceDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogInsurance dialogInsurance = new DialogInsurance();
            dialogInsurance.ShowDialog();
        }

        private void btnPatientsDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogPatient dialogPatient = new DialogPatient();
            //dialogPatient.ShowDialog();
        }

        private void btnReservationDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogReservations dialogReservations = new DialogReservations();
            dialogReservations.ShowDialog();
        }

        private void btnVisitDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogVisits dialogVisits = new DialogVisits();
            dialogVisits.ShowDialog();
        }

        private void btnEmployeeDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogEmployee dialogEmployee = new DialogEmployee();
            //dialogEmployee.ShowDialog();
        }

        private void btnHealthCardDialod_Click(object sender, RoutedEventArgs e)
        {
            //DialogHealthCard dialogHealthCard = new DialogHealthCard();
            //dialogHealthCard.ShowDialog();
        }

        private void btnPrescriptionDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogPrescrtiption dialogPrescrtiption = new DialogPrescrtiption();
            //dialogPrescrtiption.ShowDialog();
        }

        private void btnProcedureDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogProcedure dialogProcedure = new DialogProcedure();
            //dialogProcedure.ShowDialog();
        }

        private void btnUserDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogUser dialogUser = new DialogUser();
            //dialogUser.ShowDialog();
        }

        private void btnRoleDialog_Click(object sender, RoutedEventArgs e)
        {
            //DialogRole dialogRole = new DialogRole();
            //dialogRole.ShowDialog();
        }
    }
}

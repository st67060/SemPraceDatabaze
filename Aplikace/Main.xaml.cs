using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            LoadListOfReservations();
            LoadListOfPrescriptions();
            
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
        private async void LoadListOfEmployees()
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
        private async void LoadListOfPatients()
        {
            data.Patients.Clear();
            await Task.Run(() =>
            {
                List<Patient> patients = access.GetAllPatients().Result;

                Dispatcher.Invoke(() =>
                {
                    data.Patients = new ObservableCollection<Patient>(patients);
                    patientDataGrid.ItemsSource = data.Patients;
                });
            });

        }
        private async void LoadListOfUsers()
        {
            await Task.Run(() =>
            {
                List<User> users = access.GetAllUsers();
                Dispatcher.Invoke(() =>
                {
                    data.Users = new ObservableCollection<User>(users);
                    usersDataGrid.ItemsSource = data.Users;
                });
            });
        }

        private async void LoadListOfReservations()
        {
            await Task.Run(() =>
            {
                List<Patient> patients = access.GetAllPatients().Result;
                Dispatcher.Invoke(() =>
                {
                    cmbPatients.ItemsSource = patients;
                });

                List<Reservation> reservations = access.GetReservations();
                Dispatcher.Invoke(() =>
                {
                    data.Reservations = new ObservableCollection<Reservation>(reservations);
                    dgReservations.ItemsSource = data.Reservations;
                });
            });
        }
        private async void LoadListOfPrescriptions()
        {
            await Task.Run(() =>
            {
                List<Patient> patients = access.GetAllPatients().Result;
                Dispatcher.Invoke(() =>
                {
                    cmbPatient.ItemsSource = patients;
                });

                List<Prescription> prescriptions = access.GetAllPrescriptions();
                Dispatcher.Invoke(() =>
                {
                    cmbPrescription.ItemsSource = prescriptions;
                });
            });
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


      
        private void AddNewReservation(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation(0, txtNotes.Text, (DateTime)dpDate.SelectedDate, (Patient)cmbPatients.SelectedItem, user.Employee);
            access.InsertReservation(reservation);
            LoadListOfReservations();
        }

        private void btnRemoveReservation_Click(object sender, RoutedEventArgs e)
        {
            if(dgReservations.SelectedItem != null)
            {
                access.DeleteReservation((Reservation)dgReservations.SelectedItem);
                LoadListOfReservations() ;

            }
        }

        private void dgReservations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgReservations.SelectedItem != null) {
                dgProcedures.ItemsSource = data.Reservations[dgReservations.SelectedIndex].Procedures;
            }
            
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

        private void btnLogsDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogLogs dialogLogs = new DialogLogs();
            dialogLogs.ShowDialog();
        }

        private void btnAddNewProcedure_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task btnAddPrescription_ClickAsync(object sender, RoutedEventArgs e)
        {

            
            
        }

        private void btnEditPrescription_Click(object sender, RoutedEventArgs e)
        {
            if(cmbPrescription.SelectedItem != null)
            {
                Prescription prescription = cmbPrescription.SelectedItem as Prescription;
                access.UpdatePrescription(prescription);
                LoadListOfPrescriptions();
            }
            
        }

        private void btnDeletePrescription_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPrescription.SelectedItem != null)
            {
                Prescription prescription = cmbPrescription.SelectedItem as Prescription;
                access.DeletePrescription(prescription);
                LoadListOfPrescriptions();
            }
        }

        private async void btnAddPrescription_Click(object sender, RoutedEventArgs e)
        {
            Prescription prescription = new Prescription(0, txtMedicationName.Text, int.Parse(txtCoPayment.Text), user.Employee, (Patient)cmbPatient.SelectedItem, (DateTime)dpDatePresctiption.SelectedDate);
            if (access.InsertPrescription(prescription))
            {
                lPrescriptionCreated.Visibility = Visibility.Visible;
                await Task.Delay(1500);
                lPrescriptionCreated.Visibility = Visibility.Hidden;
            }
        }

        private void cmbPrescription_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Prescription prescription = cmbPrescription.SelectedItem as Prescription;
            gpPrescription.DataContext = prescription;
            List<Patient> temp = data.Patients.ToList();
            cmbPatient.SelectedIndex = temp.FindIndex(patient => patient.Id == prescription.Patient.Id);

        }
    }
}

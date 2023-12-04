using Aplikace.data;
using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
            LoadListOfEmployees();


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
            if (SetProfileImage(user) != null)
            {
                userImage.Source = SetProfileImage(user);
            }

            if (user.Employee.Role == Role.Admin)
            {
                UserTabItem.Visibility = Visibility.Visible;
                dataTabItem.Visibility = Visibility.Visible;
                settingsTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Doctor || user.Employee.Role == Role.Nurse)
            {
                UserTabItem.Visibility = Visibility.Visible;
                listTabItem.Visibility = Visibility.Visible;
                calendarTabItem.Visibility = Visibility.Visible;
                PatientTabItem.Visibility = Visibility.Visible;

            }
            else if (user.Employee.Role == Role.Employee)
            {
                UserTabItem.Visibility = Visibility.Visible;


            }


        }

        private BitmapImage SetProfileImage(User user)
        {
            if (user.Employee.Photo != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(user.Employee.Photo);
                bitmapImage.EndInit();
                return bitmapImage;
            }
            else { return null; }

        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private async void LoadListOfEmployees()
        {
            List<Employee> employees = await access.GetEmployees();

            ObservableCollection<Employee> doctorsAndNurses = new ObservableCollection<Employee>();
            foreach (Employee emp in employees)
            {
                if ((int)emp.Role == 2 || (int)emp.Role == 3)
                {
                    doctorsAndNurses.Add(emp);
                }
            }
            Employee employee = await access.GetSuperior();
            DataContext = employee;
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

        private Task LoadListOfReservations()
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            Task.Run(() =>
            {
                List<Patient> patients = access.GetAllPatients().Result;
                Dispatcher.Invoke(() =>
                {
                    cmbPatients.ItemsSource = patients;
                });

                List<Reservation> reservations = access.GetReservations();
                List<Procedure> procedures = access.GetAllProcedures();
                Dispatcher.Invoke(() =>
                {
                    data.Reservations = new ObservableCollection<Reservation>(reservations);
                    data.Procedures = new ObservableCollection<Procedure>(procedures);
                    dgReservations.ItemsSource = data.Reservations;
                    cmbProcedure.ItemsSource = data.Procedures;
                });

                // Signalizujeme dokončení asynchronní operace
                taskCompletionSource.SetResult(null);
            });

            return taskCompletionSource.Task;
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

                List<Prescription> prescriptions = access.GetAllPrescriptions().Result;
                Dispatcher.Invoke(() =>
                {
                    cmbPrescription.ItemsSource = prescriptions;
                });
            });
        }



        private void patientDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (patientDataGrid.SelectedItem != null)
            {
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
            if (patientDataGrid.SelectedItem != null)

            {
                Patient patient = (Patient)patientDataGrid.SelectedItem;
                if (access.DeletePatient(patient))
                {
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
            if (cmbPatients.SelectedItem == null || dpDate.SelectedDate == null)
            {
                MessageBox.Show("Select Patient and Date", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Reservation reservation = new Reservation(0, txtNotes.Text, (DateTime)dpDate.SelectedDate, (Patient)cmbPatients.SelectedItem, user.Employee);
                if (access.InsertReservation(reservation))
                {
                    LoadListOfReservations();
                }
                else
                {
                    MessageBox.Show("Reservation for day" + dpDate.SelectedDate.Value.ToString("dd.MM.yyyy") + "already exists", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void btnRemoveReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dgReservations.SelectedItem != null)
            {
                access.DeleteReservation((Reservation)dgReservations.SelectedItem);
                LoadListOfReservations();

            }
            else
            {

                MessageBox.Show("No reservation selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgReservations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgReservations.SelectedItem != null)
            {
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
            DialogEmployee dialogEmployee = new DialogEmployee();
            dialogEmployee.ShowDialog();
        }

        private void btnHealthCardDialod_Click(object sender, RoutedEventArgs e)
        {
            //DialogHealthCard dialogHealthCard = new DialogHealthCard();
            //dialogHealthCard.ShowDialog();
        }

        private void btnPrescriptionsDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPrescriptions dialogPrescrtiptions = new DialogPrescriptions();
            dialogPrescrtiptions.ShowDialog();
        }
         
        private void btnProceduresDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogProcedures dialogProcedure = new DialogProcedures();
            dialogProcedure.ShowDialog();
        }

        private void btnUserDialog_Click(object sender, RoutedEventArgs e)
        {
            UserDialog userDialog = new UserDialog();
            userDialog.ShowDialog();
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

        private async void btnAddNewProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (dgReservations.SelectedItem != null && cmbProcedure.SelectedItem != null)
            {
                var selectedItem = dgReservations.SelectedIndex;
                try
                {
                    access.InsertProcedureToReservation((Reservation)dgReservations.SelectedItem, (Procedure)cmbProcedure.SelectedItem);
                    await LoadListOfReservations();
                    dgReservations.SelectedIndex = selectedItem;
                    dgProcedures.ItemsSource = data.Reservations[selectedItem].Procedures;
                }
                catch (Exception ex)
                {


                }

            }
            else
            {
                MessageBox.Show("No reservation or procedure selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnEditPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPrescription.SelectedItem != null)
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
            else
            {
                MessageBox.Show("Select prescription", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnAddPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMedicationName.Text) || string.IsNullOrEmpty(txtCoPayment.Text) || cmbPatient.SelectedItem == null || dpDatePresctiption.SelectedDate == null)
            {

                MessageBox.Show("Some Fields are empty", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Prescription prescription = new Prescription(0, txtMedicationName.Text, int.Parse(txtCoPayment.Text), user.Employee, (Patient)cmbPatient.SelectedItem, (DateTime)dpDatePresctiption.SelectedDate);
                if (access.InsertPrescription(prescription))
                {
                    lPrescriptionCreated.Visibility = Visibility.Visible;
                    await Task.Delay(1500);
                    lPrescriptionCreated.Visibility = Visibility.Hidden;
                }
            }

        }

        private void cmbPrescription_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Prescription prescription = cmbPrescription.SelectedItem as Prescription;
            gpPrescription.DataContext = prescription;
            List<Patient> temp = data.Patients.ToList();
            cmbPatient.SelectedIndex = temp.FindIndex(patient => patient.Id == prescription.Patient.Id);

        }




        private void listTabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadListOfPrescriptions();
        }

        private void calendarTabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadListOfReservations();
        }

        private void mainMenuTabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadListOfEmployees();
        }

        private void settingsTabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadListOfUsers();
        }


        private void medicalCard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadListOfPatients();
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(patientDataGrid.ItemsSource);
            collectionView.Filter = o =>
            {
                if (o is Patient patient)
                {
                    if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                        return true;

                    bool patientMatch = patient.FirstName.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.LastName.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.SocialSecurityNumber.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.Gender.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.DateOfBirth.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.Phone.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                                       patient.Email.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase);


                    bool addressMatch = patient.Address?.City.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                       patient.Address?.PostalCode.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                       patient.Address?.StreetNumber.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                       patient.Address?.Country.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                       patient.Address?.Street.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true;

                    bool healthCardMatch = patient.HealthCard?.Smokes.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                          patient.HealthCard?.Pregnancy.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                          patient.HealthCard?.Alcohol.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                          patient.HealthCard?.Sport.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                          patient.HealthCard?.Fillings.ToString().Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) == true;

                    return patientMatch || addressMatch || healthCardMatch;



                }

                return false;
            };
        }

        private void btnSysCatalogDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogSysCatalog dialogSysCatalog = new DialogSysCatalog();
            dialogSysCatalog.ShowDialog();
        }
    }
}

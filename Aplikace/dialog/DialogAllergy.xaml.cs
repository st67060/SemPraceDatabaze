using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Aplikace.data.Entity;
using Aplikace.data;
using System.Linq;

namespace Aplikace.dialog
{

    public partial class DialogAllergy : Window
    {
        DataAccess access;
        ObservableCollection<Alergy> allergies;

        public DialogAllergy()
        {
            access = new DataAccess();
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadAllergies();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgAlergie.SelectedItem != null)
            {
                Alergy selectedAlergy = (Alergy)dgAlergie.SelectedItem;
                Alergy allergy = new Alergy(selectedAlergy.Id, txtName.Text);
                access.UpdateAlergy(allergy);
                LoadAllergies();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                Alergy allergy = new Alergy(0, txtName.Text);
                access.InsertAlergy(allergy);
                LoadAllergies();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgAlergie.SelectedItem != null)
            {
                Alergy allergy = (Alergy)dgAlergie.SelectedItem;
                access.DeleteAlergy(allergy);
                LoadAllergies();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAllergies()
        {
            allergies = new ObservableCollection<Alergy>(access.GetAllAllergies());
            dgAlergie.ItemsSource = allergies;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void dgAlergie_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Alergy selectedAlergy = (Alergy)dgAlergie.SelectedItem;
            DataContext = selectedAlergy;
        }

        private bool IsTextual(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}


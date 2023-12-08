using Aplikace.data;
using Aplikace.data.Entity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aplikace.dialog
{
    public partial class DialogInsurance : Window
    {
        DataAccess access;
        ObservableCollection<Insurance> insurances;

        public DialogInsurance()
        {
            InitializeComponent();
            access = new DataAccess();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadInsurances();
        }

        private void LoadInsurances()
        {
            insurances = new ObservableCollection<Insurance>(access.GetAllInsurances());
            dgInsurance.ItemsSource = insurances;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgInsurance.SelectedItem != null)
            {
                Insurance selectedInsurance = (Insurance)dgInsurance.SelectedItem;
                selectedInsurance.Name = txtName.Text;
                selectedInsurance.Abbreviation = txtAbbreviation.Text;

                access.UpdateInsurance(selectedInsurance);
                LoadInsurances();
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
                Insurance newInsurance = new Insurance(0, txtName.Text, txtAbbreviation.Text);
                access.InsertInsurance(newInsurance);
                LoadInsurances();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInsurance.SelectedItem != null)
            {
                Insurance selectedInsurance = (Insurance)dgInsurance.SelectedItem;
                access.DeleteInsurance(selectedInsurance);
                LoadInsurances();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgInsurance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInsurance.SelectedItem is Insurance selectedInsurance)
            {
                txtName.Text = selectedInsurance.Name;
                txtAbbreviation.Text = selectedInsurance.Abbreviation;
            }
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

        private void txtAbbreviation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}

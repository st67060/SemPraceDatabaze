using Aplikace.data;
using Aplikace.data.Entity;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            Insurance newInsurance = new Insurance(0, txtName.Text, txtAbbreviation.Text);
            access.InsertInsurance(newInsurance);
            LoadInsurances();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInsurance.SelectedItem != null)
            {
                Insurance selectedInsurance = (Insurance)dgInsurance.SelectedItem;
                access.DeleteInsurance(selectedInsurance);
                LoadInsurances();
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
    }
}

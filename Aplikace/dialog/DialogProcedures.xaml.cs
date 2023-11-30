using Aplikace.data.Entity;
using Aplikace.data;
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
    /// Interakční logika pro DialogProcedures.xaml
    /// </summary>
    public partial class DialogProcedures : Window
    {
        DataAccess access;
        ObservableCollection<Procedure> procedures;

        public DialogProcedures()
        {
            access = new DataAccess();

            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadProcedures();
        }

        private void LoadProcedures()
        {
            procedures = new ObservableCollection<Procedure>(access.GetAllProcedures());
            dgProcedures.ItemsSource = procedures;
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && decimal.TryParse(txtPrice.Text, out var price))
            {
                var newProcedure = new Procedure(0, txtName.Text, price, chkCoveredByInsurance.IsChecked ?? false, txtProcedureSteps.Text);
                // access.InsertProcedure(newProcedure); 
                LoadProcedures();
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgProcedures.SelectedItem != null && !string.IsNullOrWhiteSpace(txtName.Text) && decimal.TryParse(txtPrice.Text, out var price))
            {
                var selectedProcedure = (Procedure)dgProcedures.SelectedItem;
                selectedProcedure.Name = txtName.Text;
                selectedProcedure.Price = price;
                selectedProcedure.CoveredByInsurance = chkCoveredByInsurance.IsChecked ?? false;
                selectedProcedure.ProcedureSteps = txtProcedureSteps.Text;

                // access.UpdateProcedure(selectedProcedure); 
                LoadProcedures();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProcedures.SelectedItem != null)
            {
                var selectedProcedure = (Procedure)dgProcedures.SelectedItem;
                // access.DeleteProcedure(selectedProcedure); 
                LoadProcedures();
            }
        }

        private void dgProcedures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProcedures.SelectedItem != null)
            {
                Procedure selectedProcedure = (Procedure)dgProcedures.SelectedItem;
                DataContext = selectedProcedure;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

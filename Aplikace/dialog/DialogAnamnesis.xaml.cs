using Aplikace.data;
using Aplikace.data.Entity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aplikace.dialog
{
    public partial class DialogAnamnesis : Window
    {
        DataAccess access;
        ObservableCollection<Anamnesis> anamnesises;

        public DialogAnamnesis()
        {
            access = new DataAccess();
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadAnamnesis();
        }

        private void LoadAnamnesis()
        {
            anamnesises = new ObservableCollection<Anamnesis>(access.GetAllAnamnesis());
            dgAnamnesis.ItemsSource = anamnesises;
        }


        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgAnamnesis.SelectedItem != null)
            {
                Anamnesis selectedAnamnesis = (Anamnesis)dgAnamnesis.SelectedItem;
                Anamnesis anamnesis = new Anamnesis(selectedAnamnesis.Id, txtAnamnesis.Text);
                access.UpdateAnamnesis(anamnesis);
                LoadAnamnesis();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAnamnesis.Text))
            {
                Anamnesis anamnesis = new Anamnesis(0, txtAnamnesis.Text);
                access.InsertAnamnesis(anamnesis);
                LoadAnamnesis();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgAnamnesis.SelectedItem != null)
            {
                Anamnesis selectedAnamnesis = (Anamnesis)dgAnamnesis.SelectedItem;
                access.DeleteAnamnesis(selectedAnamnesis);
                LoadAnamnesis();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void dgAnamnesis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAnamnesis.SelectedItem != null)
            {
                Anamnesis selectedAnamnesis = (Anamnesis)dgAnamnesis.SelectedItem;
                txtAnamnesis.Text = selectedAnamnesis.Name;
            }
        }

        private bool IsTextual(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private void txtAnamnesis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}


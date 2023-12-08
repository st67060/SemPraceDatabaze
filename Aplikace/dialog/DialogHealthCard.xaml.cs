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
using System.Xml.Linq;

namespace Aplikace.dialog
{

    public partial class DialogHealthCard : Window
    {


        DataAccess access;
        ObservableCollection<HealthCard> healthCards;
        ObservableCollection<Anamnesis> AnamnesisList;


        public DialogHealthCard()
        {
            access = new DataAccess();
            DataContext = this;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            LoadHealthCards();
        }

        private void dgHealthCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgHealthCards != null && dgHealthCards.SelectedItem != null)
            {
                var healthCard = (HealthCard)dgHealthCards.SelectedItem;
                if (healthCard != null && healthCard.Anamnesis != null && AnamnesisList != null)
                {
                    detailStackPanel.DataContext = healthCard;
                    List<Anamnesis> tempAnamnesis = AnamnesisList.ToList();
                    int indexAnamnesis = tempAnamnesis.FindIndex(a => a.Id == healthCard.Anamnesis.Id);
                    cmbAnamnesis.SelectedIndex = indexAnamnesis;
                }
            }
        }




        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSport.Text) && cmbAnamnesis.SelectedItem != null)
            {
                var newHealthCard = new HealthCard(0, chkSmokes.IsChecked ?? false, chkPregnancy.IsChecked ?? false, chkAlcohol.IsChecked ?? false, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                access.InsertHealthCard(newHealthCard);
                LoadHealthCards();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgHealthCards.SelectedItem != null)
            {
                HealthCard temp = (HealthCard)dgHealthCards.SelectedItem;
                HealthCard healthCards = new HealthCard(temp.Id, chkSmokes.IsChecked ?? false, chkPregnancy.IsChecked ?? false, chkAlcohol.IsChecked ?? false, txtSport.Text, int.Parse(txtFillings.Text), (Anamnesis)cmbAnamnesis.SelectedItem);
                access.UpdateHealthCard(healthCards);
                LoadHealthCards();
            }
            else
            {
                MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgHealthCards.SelectedItem != null)
            {
                HealthCard selectedHealthCard = (HealthCard)dgHealthCards.SelectedItem;

                if (access.DeleteHealthCard(selectedHealthCard))
                {
                    LoadHealthCards();

                }
                else
                {
                    MessageBox.Show("data integrity violation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void LoadHealthCards()
        {
            Task.Run(() =>
            {
                healthCards = new ObservableCollection<HealthCard>(access.GetAllHealthCards());
                AnamnesisList = new ObservableCollection<Anamnesis>(access.GetAllAnamnesis());


                Dispatcher.Invoke(() =>
                {
                    dgHealthCards.ItemsSource = healthCards;
                    cmbAnamnesis.ItemsSource = AnamnesisList;

                });
            });
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private bool IsTextual(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private void txtSport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextual(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void txtFillings_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}

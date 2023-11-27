﻿using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Aplikace.data.Entity;
using Aplikace.data;


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
                var temp = (Alergy)dgAlergie.SelectedItem;
                Alergy allergy = new Alergy(temp.Id, txtName.Text);
                //access.UpdateAllergy(allergy);
                LoadAllergies();
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                Alergy allergy = new Alergy(0, txtName.Text); 
                //access.InsertAllergy(allergy);
                LoadAllergies();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgAlergie.SelectedItem != null)
            {
                Alergy allergy = (Alergy)dgAlergie.SelectedItem;
                //access.DeleteAllergy(allergy);
                LoadAllergies();
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
    }
}


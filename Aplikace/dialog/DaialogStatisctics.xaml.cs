using Aplikace.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interakční logika pro DaialogStatisctics.xaml
    /// </summary>
    public partial class DaialogStatisctics : Window
    {
        public string mostCommonAllergy;
        public double smokingPatientsCount;
        public double averagePatientAge;
        private DataAccess access;

        
        public DaialogStatisctics()
        {
            access = new DataAccess();
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            mostCommonAllergy = access.GetMostCommonAllergy();
            smokingPatientsCount = access.GetSmokingPercentage();
            averagePatientAge = access.GetAveragePatientAge();
            this.DataContext = this;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        public string MostCommonAllergy
        {
            get { return mostCommonAllergy; }
            set
            {
                if (mostCommonAllergy != value)
                {
                    mostCommonAllergy = value;
                    OnPropertyChanged(nameof(MostCommonAllergy));
                }
            }
        }

        public double SmokingPatientsCount
        {
            get { return smokingPatientsCount; }
            set
            {
                if (smokingPatientsCount != value)
                {
                    smokingPatientsCount = value;
                    OnPropertyChanged(nameof(SmokingPatientsCount));
                }
            }
        }

        public double AveragePatientAge
        {
            get { return averagePatientAge; }
            set
            {
                if (averagePatientAge != value)
                {
                    averagePatientAge = value;
                    OnPropertyChanged(nameof(AveragePatientAge));
                }
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

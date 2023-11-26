using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Entity
{
    public class Prescription : INotifyPropertyChanged
    {
        private int id;
        private string drugName;
        private decimal supplement;
        private Employee employee;
        private Patient patient;
        private DateTime date;

        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }

        public string DrugName
        {
            get { return drugName; }
            set
            {
                if (drugName != value)
                {
                    drugName = value;
                    OnPropertyChanged(nameof(DrugName));
                }
            }
        }

        public decimal Supplement
        {
            get { return supplement; }
            set
            {
                if (supplement != value)
                {
                    supplement = value;
                    OnPropertyChanged(nameof(Supplement));
                }
            }
        }

        public Employee Employee
        {
            get { return employee; }
            set
            {
                if (employee != value)
                {
                    employee = value;
                    OnPropertyChanged(nameof(Employee));
                }
            }
        }

        public Patient Patient
        {
            get { return patient; }
            set
            {
                if (patient != value)
                {
                    patient = value;
                    OnPropertyChanged(nameof(Patient));
                }
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        // Other employee attributes can be added here
    }

    public class Patient
    {
        public int ID { get; set; }
        // Other patient attributes can be added here
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Aplikace.data.Entity
{
    public class Reservation : INotifyPropertyChanged
    {
        private int id;
        private string notes;
        private DateTime date;
        private Patient patient;
        private Employee employee;
        private ObservableCollection<Procedure> procedures;

        public Reservation(int id, string notes, DateTime date, Patient patient, Employee employee)
        {
            Id = id;
            Notes = notes;
            Date = date;
            Patient = patient;
            Employee = employee;
            Procedures = new ObservableCollection<Procedure>();
        }

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged(nameof(Notes));
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

        public ObservableCollection<Procedure> Procedures
        {
            get { return procedures; }
            set
            {
                if (procedures != value)
                {
                    procedures = value;
                    OnPropertyChanged(nameof(Procedures));
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

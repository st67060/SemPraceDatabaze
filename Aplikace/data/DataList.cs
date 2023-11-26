using Aplikace.data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data
{
    public class DataList
    {
        public ObservableCollection<Address> Addresses { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<HealthCard> HealthCards { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Procedure> Procedures { get; set; }
        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Visit> Visits { get; set; }
        public ObservableCollection<Alergy> Alergies { get; set; }

        public ObservableCollection<Log> Logs { get; set; }

        public DataList()
        {
            Addresses = new ObservableCollection<Address>();
            Employees = new ObservableCollection<Employee>();
            HealthCards = new ObservableCollection<HealthCard>();
            Patients = new ObservableCollection<Patient>();
            Procedures = new ObservableCollection<Procedure>();
            Reservations = new ObservableCollection<Reservation>();
            Users = new ObservableCollection<User>();
            Visits = new ObservableCollection<Visit>();
            Alergies = new ObservableCollection<Alergy>();
            Logs = new ObservableCollection<Log>();
        }
    }
}

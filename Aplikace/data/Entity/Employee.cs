using Aplikace.data.Enum;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Aplikace.data.Entity
{
    public class Employee : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string surname;
        private DateTime hireDate;
        private byte[] photo;
        private Role role;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Surname
        {
            get { return surname; }
            set { SetProperty(ref surname, value); }
        }

        public DateTime HireDate
        {
            get { return hireDate; }
            set { SetProperty(ref hireDate, value); }
        }

        public byte[] Photo
        {
            get { return photo; }
            set { SetProperty(ref photo, value); }
        }

        public Role Role
        {
            get { return role; }
            set { SetProperty(ref role, value); }
        }

        public Employee(int id, string name, string surname, DateTime hireDate, byte[] photo, Role role)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (surname == null)
                throw new ArgumentNullException(nameof(surname));
        
            Id = id;
            Name = name;
            Surname = surname;
            HireDate = hireDate;
            Photo = photo;
            Role = role;
        }

        public Employee(int id, string name, string surname, DateTime hireDate, Role role)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (surname == null)
                throw new ArgumentNullException(nameof(surname));

            Id = id;
            Name = name;
            Surname = surname;
            HireDate = hireDate;
            Role = role;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}

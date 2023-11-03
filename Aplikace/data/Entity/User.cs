using System;
using System.ComponentModel;

namespace Aplikace.data.Entity
{
    public class User : INotifyPropertyChanged
    {
        private string name;
        private string password;
        private Employee employee;

        public int Id { get; }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    if (value == null)
                        throw new ArgumentNullException("Name cannot be null");
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    if (value == null)
                        throw new ArgumentNullException("Password cannot be null");
                    password = value;
                    OnPropertyChanged(nameof(Password));
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
                    if (value == null)
                        throw new ArgumentNullException("Employee cannot be null");
                    employee = value;
                    OnPropertyChanged(nameof(Employee));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public User(int id, string name, string password, Employee employee)
        {
            if (name == null)
                throw new ArgumentNullException("Name cannot be null");
            if (password == null)
                throw new ArgumentNullException("Password cannot be null");
            if (employee == null)
                throw new ArgumentNullException("Employee cannot be null");

            Id = id;
            Name = name;
            Password = password;
            Employee = employee;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Entity
{
    public class Address : INotifyPropertyChanged
    {
        private int id;
        private string city;
        private string postalCode;
        private string streetNumber;
        private string country;
        private string street;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value); }
        }

        public string StreetNumber
        {
            get { return streetNumber; }
            set { SetProperty(ref streetNumber, value); }
        }

        public string Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }

        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }

        public Address(int id, string city, string postalCode, string streetNumber, string country, string street)
        {
            Id = id;
            City = city;
            PostalCode = postalCode;
            StreetNumber = streetNumber;
            Country = country;
            Street = street;
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
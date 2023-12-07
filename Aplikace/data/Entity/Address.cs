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
        private int postalCode;
        private int streetNumber;
        private string country;
        private string street;

        public Address(int id, string city, int postalCode, int streetNumber, string country, string street)
        {
            Id = id;
            City = city;
            PostalCode = postalCode;
            StreetNumber = streetNumber;
            Country = country;
            Street = street;
        }

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

        public int PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value); }
        }

        public int StreetNumber
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

        public override string ToString()
        {
          
            return $"{Street} {StreetNumber}, {PostalCode} {City}, {Country}";
        }
    }
}
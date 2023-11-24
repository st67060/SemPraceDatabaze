using Aplikace.data.Enum;
using System;
using System.ComponentModel;

namespace Aplikace.data.Entity
{
    public class Patient : INotifyPropertyChanged
    {
        public Patient(int id, string firstName, string lastName, string socialSecurityNumber, string gender, DateTime dateOfBirth, string phone, string email, Address address, HealthCard healthCard, InsuranceCompany insuranceCompany)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Phone = phone;
            Email = email;
            Address = address;
            InsuranceCompany = insuranceCompany;
            HealthCard = healthCard;
        }

        private int id;
        private string firstName;
        private string lastName;
        private string socialSecurityNumber;
        private string gender;
        private DateTime dateOfBirth;
        private string phone;
        private string email;
        private Address address;
        private InsuranceCompany insuranceCompany;
        private HealthCard healthCard;

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

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string SocialSecurityNumber
        {
            get { return socialSecurityNumber; }
            set
            {
                if (socialSecurityNumber != value)
                {
                    socialSecurityNumber = value;
                    OnPropertyChanged(nameof(SocialSecurityNumber));
                }
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (dateOfBirth != value)
                {
                    dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public Address Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public InsuranceCompany InsuranceCompany
        {
            get { return insuranceCompany; }
            set
            {
                if (insuranceCompany != value)
                {
                    insuranceCompany = value;
                    OnPropertyChanged(nameof(InsuranceCompany));
                }
            }
        }

        

        public HealthCard HealthCard
        {
            get { return healthCard; }
            set
            {
                if (healthCard != value)
                {
                    healthCard = value;
                    OnPropertyChanged(nameof(HealthCard));
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

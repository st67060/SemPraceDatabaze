using Aplikace.data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Entity
{
    public class Procedure : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private decimal price;
        private bool coveredByInsurance;
        private string procedureSteps;

        public Procedure(int id, string name, decimal price, bool coveredByInsurance, string procedureSteps)
        {
            Id = id;
            Name = name;
            Price = price;
            CoveredByInsurance = coveredByInsurance;
            ProcedureSteps = procedureSteps;
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

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public bool CoveredByInsurance
        {
            get { return coveredByInsurance; }
            set
            {
                if (coveredByInsurance != value)
                {
                    coveredByInsurance = value;
                    OnPropertyChanged(nameof(CoveredByInsurance));
                }
            }
        }

        public string ProcedureSteps
        {
            get { return procedureSteps; }
            set
            {
                if (procedureSteps != value)
                {
                    procedureSteps = value;
                    OnPropertyChanged(nameof(ProcedureSteps));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            return $"{name}";
        }
    }
}

using Aplikace.data.Enum;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Aplikace.data.Entity
{
    public class HealthCard : INotifyPropertyChanged
    {
        private int id;
        private bool smokes;
        private bool pregnancy;
        private bool alcohol;
        private string sport;
        private int fillings;
        private Anamnesis anamnesis;
        private ObservableCollection<Alergy> alergies;

        public HealthCard(int id, bool smokes, bool pregnancy, bool alcohol, string sport, int fillings, Anamnesis anamnesis)
        {
            Id = id;
            Smokes = smokes;
            Pregnancy = pregnancy;
            Alcohol = alcohol;
            Sport = sport;
            Fillings = fillings;
            Anamnesis = anamnesis;
            Alergies = new ObservableCollection<Alergy>();
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

        public bool Smokes
        {
            get { return smokes; }
            set
            {
                if (smokes != value)
                {
                    smokes = value;
                    OnPropertyChanged(nameof(Smokes));
                }
            }
        }

        public bool Pregnancy
        {
            get { return pregnancy; }
            set
            {
                if (pregnancy != value)
                {
                    pregnancy = value;
                    OnPropertyChanged(nameof(Pregnancy));
                }
            }
        }

        public bool Alcohol
        {
            get { return alcohol; }
            set
            {
                if (alcohol != value)
                {
                    alcohol = value;
                    OnPropertyChanged(nameof(Alcohol));
                }
            }
        }

        public string Sport
        {
            get { return sport; }
            set
            {
                if (sport != value)
                {
                    sport = value;
                    OnPropertyChanged(nameof(Sport));
                }
            }
        }

        public int Fillings
        {
            get { return fillings; }
            set
            {
                if (fillings != value)
                {
                    fillings = value;
                    OnPropertyChanged(nameof(Fillings));
                }
            }
        }

        public Anamnesis Anamnesis
        {
            get { return anamnesis; }
            set
            {
                if (anamnesis != value)
                {
                    anamnesis = value;
                    OnPropertyChanged(nameof(Anamnesis));
                }
            }
        }

      

        public ObservableCollection<Alergy> Alergies 
        {
            get { return alergies; }
            set
            {
                if (alergies != value)
                {
                    alergies = value;
                    OnPropertyChanged(nameof(Alergies));
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

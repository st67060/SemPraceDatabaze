using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Entity
{
    public enum ChangeType
    {
        Insert,
        Update,
        Delete
    }

    public class Log : INotifyPropertyChanged
    {
        private int id;
        private string tableName;
        private ChangeType changeType;
        private DateTime changeTime;

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

        public string TableName
        {
            get { return tableName; }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    OnPropertyChanged(nameof(TableName));
                }
            }
        }

        public ChangeType ChangeType
        {
            get { return changeType; }
            set
            {
                if (changeType != value)
                {
                    changeType = value;
                    OnPropertyChanged(nameof(ChangeType));
                }
            }
        }

        public DateTime ChangeTime
        {
            get { return changeTime; }
            set
            {
                if (changeTime != value)
                {
                    changeTime = value;
                    OnPropertyChanged(nameof(ChangeTime));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Log(int id, string tableName, ChangeType changeType, DateTime changeTime)
        {
            Id = id;
            TableName = tableName;
            ChangeType = changeType;
            ChangeTime = changeTime;
        }
    }
}

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
        private string before;
        private string after;

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
        public string Before
        {
            get { return before; }
            set
            {
                if (before != value)
                {
                    before = value;
                    OnPropertyChanged(nameof(Before));
                }
            }
        }
        public string After
        {
            get { return after; }
            set
            {
                if (after != value)
                {
                    after = value;
                    OnPropertyChanged(nameof(After));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Log(int id, string tableName, ChangeType changeType, DateTime changeTime,string before,string after)
        {
            Id = id;
            TableName = tableName;
            ChangeType = changeType;
            ChangeTime = changeTime;
            Before = before;
            After = after;
        }
    }
}

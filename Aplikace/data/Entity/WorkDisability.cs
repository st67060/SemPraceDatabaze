using Aplikace.data.Entity;
using System;
using System.ComponentModel;

public class WorkDisability : INotifyPropertyChanged
{
    private int id;
    private DateTime startDate;
    private DateTime endDate;
    private string occupation;
    private string reason;
    private Employee employee;

    public WorkDisability(int id, DateTime startDate, DateTime endDate, string occupation, string reason, Employee employee)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Occupation = occupation;
        Reason = reason;
        Employee = employee;
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

    public DateTime StartDate
    {
        get { return startDate; }
        set
        {
            if (startDate != value)
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    public DateTime EndDate
    {
        get { return endDate; }
        set
        {
            if (endDate != value)
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }

    public string Occupation
    {
        get { return occupation; }
        set
        {
            if (occupation != value)
            {
                occupation = value;
                OnPropertyChanged(nameof(Occupation));
            }
        }
    }

    public string Reason
    {
        get { return reason; }
        set
        {
            if (reason != value)
            {
                reason = value;
                OnPropertyChanged(nameof(Reason));
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
                employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

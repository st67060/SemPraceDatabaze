using Aplikace.data;
using Aplikace.data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aplikace.dialog
{
    /// <summary>
    /// Interakční logika pro DialogLogs.xaml
    /// </summary>
    public partial class DialogLogs : Window
    {
        Log log;
        DataAccess dataAccess;
        public DialogLogs()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
            log = null;
            DataContext = log;
            logsDataGrid.ItemsSource = dataAccess.GetAllLogs();
        }


        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

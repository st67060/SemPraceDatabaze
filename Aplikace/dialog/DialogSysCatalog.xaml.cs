using Aplikace.data.Entity;
using Aplikace.data;
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
    /// Interakční logika pro DialogSysCatalog.xaml
    /// </summary>
    public partial class DialogSysCatalog : Window
    {
        public DialogSysCatalog()
        {
            Catalog catalog;
            DataAccess dataAccess;
            InitializeComponent();
            dataAccess = new DataAccess();
            catalog = null;
            DataContext = catalog;
            logsDataGrid.ItemsSource = dataAccess.GetCatalog().Result;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

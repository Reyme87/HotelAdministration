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

namespace HotelAdministration.Views
{
    /// <summary>
    /// Логика взаимодействия для ClientsQueryWindow.xaml
    /// </summary>
    public partial class ClientsQueryWindow : Window
    {
        public ClientsQueryWindow()
        {
            InitializeComponent();
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow aw = new AdminWindow();
            aw.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = aw;
        }
    }
}

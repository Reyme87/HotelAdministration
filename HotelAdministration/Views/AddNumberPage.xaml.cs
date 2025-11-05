using HotelAdministration.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelAdministration.Views
{
    /// <summary>
    /// Логика взаимодействия для AddNumberPage.xaml
    /// </summary>
    public partial class AddNumberPage : Page
    {
        private readonly ManageViewModel _vm = ManageViewModel.Instance;
        public AddNumberPage()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }
    }
}

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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ManageViewModel _vm = ManageViewModel.Instance;
        public AdminPage()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void AddFloorButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFloorPage());
        }

        private void AddNumberButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNumberPage());
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployeePage());
        }
    }
}

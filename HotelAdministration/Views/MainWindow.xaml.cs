using HotelAdministration.Views;
using System.Windows;

namespace HotelAdministration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new WelcomePage();
        }
    }
}
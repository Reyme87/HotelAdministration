using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelAdministration.Views
{
    /// <summary>
    /// Логика взаимодействия для PasswordBoxControl.xaml
    /// </summary>
    public partial class PasswordBoxControl : UserControl
    {
        public string Password 
        { 
            get => (string)GetValue(PasswordProperty); 
            set => SetValue(PasswordProperty, value); 
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxControl), new PropertyMetadata());

        public PasswordBoxControl()
        {
            InitializeComponent();
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = pwdBox.Password;
        }
    }
}

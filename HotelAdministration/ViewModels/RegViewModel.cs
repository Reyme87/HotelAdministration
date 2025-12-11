using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HotelAdministration.Commands;
using HotelAdministration.Models;
using HotelAdministration.ViewModels.Base;
using HotelAdministration.Views;
using PasswordManager.Models;

namespace HotelAdministration.ViewModels
{
    public class RegViewModel : ViewModel
    {
        #region Элементы полей

        private string? _name;
        private string? _phone;
        private string? _email;
        private string? _login;
        private string? _password;

        private string _authLogin;
        private string _authPassword;

        public string? Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
            }
        }

        public string? Phone
        {
            get => _phone;
            set
            {
                Set(ref _phone, value);
            }
        }

        public string? Email
        {
            get => _email;
            set
            {
                Set(ref _email, value);
            }
        }

        public string? Login
        {
            get => _login;
            set
            {
                Set(ref _login, value);
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
            }
        }

        public string AuthLogin
        {
            get => _authLogin;
            set
            {
                Set(ref _authLogin, value);
            }
        }

        public string AuthPassword
        {
            get => _authPassword;
            set
            {
                Set(ref _authPassword, value);
            }
        }

        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();

        #endregion

        #region Команды

        #region RegisterCommand

        //Команда для регистрации администратора
        public ICommand RegisterCommand { get; }

        public async void OnRegisterCommandExecuted(object p)
        {
            //Разделение имени из поля ввода на части
            var nameParts = Name.Trim().Split(' ');
            string lastName = nameParts[0], firstName = nameParts[1], middleName = "";
            if (nameParts.Length > 2)
            {
                middleName = nameParts[2];
            }

            //Заполнение свойств сущности Administrator
            Administrator admin = new Administrator();
            admin.LastName = lastName;
            admin.FirstName = firstName;
            admin.MiddleName = middleName;
            admin.PhoneNumber = Phone;
            admin.Email = Email;
            admin.Login = Login;
            admin.PasswordHash = Password;

            //Сериализация данных в json-файл
            try
            {
                JsonController<Administrator>.LoadInfoAsync(admin, "admin.json");
                Name = "";
                Phone = "";
                Email = "";
                Login = "";
                Password = "";
            }
            catch
            {
                MessageBox.Show("Ошибка регистрации.");
            }
        }

        public bool CanRegisterCommandExecute(object p) => true;

        #endregion

        #region AuthCommand

        //Команда авторизации администратора
        public ICommand AuthCommand { get; }

        public async void OnAuthCommandExecuted(object p)
        {
            //Десериализация данных об администраторе из json-файла
            Administrator admin = JsonController<Administrator>.GetInfo<Administrator>("admin.json");

            try
            {
                //Проверка соответствия паролей и полей ввода. Переход на главное окно администратора в случае правильности введённых данных
                if ((Equals(AuthLogin, admin.Login) || Equals(AuthLogin, admin.Email)) && Equals(AuthPassword, admin.PasswordHash))
                {
                    AdminWindow aw = new AdminWindow();
                    aw.Show();
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = aw;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка авторизации!");
            }

        }

        public bool CanAuthCommandExecute(object p) => !Equals(AuthLogin, null) && !Equals(AuthPassword, null);

        #endregion

        #endregion

        public RegViewModel() 
        {
            #region Команды
            //Регистрация команд

            RegisterCommand = new RelayCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);

            AuthCommand = new RelayCommand(OnAuthCommandExecuted, CanAuthCommandExecute);

            #endregion
        }
    }
}

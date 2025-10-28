using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HotelAdministration.Commands;
using HotelAdministration.ViewModels.Base;

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


        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();

        #endregion

        #region Команды

        #region RegisterCommand

        public ICommand RegisterCommand { get; }

        public async void OnRegisterCommandExecuted(object p)
        {
            var nameParts = Name.Trim().Split(' ');
            string lastName = nameParts[0], firstName = nameParts[1], middleName = "";
            if (nameParts.Length > 2)
            {
                middleName = nameParts[2];
            }

            //Administrator admin = new Administrator();
            //admin.LastName = lastName;
            //admin.FirstName = firstName;
            //admin.MiddleName = middleName;
            //admin.PhoneNumber = Phone;
            //admin.Email = Email;
            //admin.Login = Login;
            //admin.PasswordHash = Password;

            //_context.Add(admin);
            //_context.SaveChanges();
        }

        public bool CanRegisterCommandExecute(object p) => true;

        #endregion

        #endregion

        public RegViewModel() 
        {
            #region Команды

            RegisterCommand = new RelayCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);

            #endregion
        }
    }
}

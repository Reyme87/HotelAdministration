using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HotelAdministration.Commands;
using HotelAdministration.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace HotelAdministration.ViewModels
{
    public class ManageViewModel : ViewModel
    {
        #region Элементы полей

        #region Floor

        private int _floorNumber;
        private int _totalRooms;

        public int FloorNumber
        {
            get => _floorNumber;
            set
            {
                Set(ref _floorNumber, value);
            }
        }

        public int TotalRooms
        {
            get => _totalRooms;
            set
            {
                Set(ref _totalRooms, value);
            }
        }

        #endregion

        #region Number

        private int _roomNumber;
        private int _floorId;
        private int _capacity;
        private int _price;

        public int RoomNumber
        {
            get => _roomNumber;
            set
            {
                Set(ref _roomNumber, value);
            }
        }

        public int FloorId
        {
            get => _floorId;
            set
            {
                Set(ref _floorId, value);
            }
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                Set(ref _capacity, value);
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                Set(ref _price, value);
            }
        }

        #endregion

        #region Employee

        private string _lastName;
        private string _firstName;
        private string _middleName;
        private string _phoneNumber;

        public string LastName
        {
            get => _lastName;
            set
            {
                Set(ref _lastName, value);
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                Set(ref _firstName, value);
            }
        }

        public string MiddleName
        {
            get => _middleName;
            set
            {
                Set(ref _middleName, value);
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                Set(ref _phoneNumber, value);
            }
        }

        #endregion

        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();
        private ObservableCollection<Floor> _floors;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Employee> _employees;

        private List<string> days = ["Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"];

        public ObservableCollection<Floor> Floors
        {
            get => _floors;
            set
            {
                Set(ref _floors, value);
            }
        }
        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                Set(ref _rooms, value);
            }
        }
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                Set(ref _employees, value);
            }
        }

        private Floor _selectedFloor;
        private Room _selectedRoom;
        private Employee _selectedEmployee;

        public Floor SelectedFloor
        {
            get => _selectedFloor;
            set
            {
                Set(ref _selectedFloor, value);
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                Set(ref _selectedRoom, value);
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                Set(ref _selectedEmployee, value);
            }
        }

        #endregion

        #region Команды

        #region AddFloorCommand

        public ICommand AddFloorCommand { get; }

        public void OnAddFloorCommandExecuted(object p)
        {
            Floor floor = new Floor();
            floor.FloorNumber = FloorNumber;
            floor.TotalRoomsNumber = TotalRooms;

            try
            {
                _context.Floors.Add(floor);
                _context.SaveChanges();
                Floors.Add(floor);

                FloorNumber = 0;
                TotalRooms = 0;
            }
            catch { }
        }

        public bool CanAddFloorCommandExecuted(object p) => true;

        #endregion

        #region AddRoomCommand

        public ICommand AddRoomCommand { get; }

        public void OnAddRoomCommandExecuted(object p)
        {
            Room room = new Room();
            room.RoomNumber = RoomNumber;

            room.FloorId = SelectedFloor.FloorId;

            room.Capacity = Capacity;
            room.PricePerNumber = Price;
            switch(Capacity)
            {
                case 1:
                    room.RoomType = "Одноместный";
                    break;
                case 2:
                    room.RoomType = "Двухместный";
                    break;
                case 3:
                    room.RoomType = "Трёхместный";
                    break;
                default:
                    break;
            }
            room.FreePlaces = Capacity;

            try
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();

                Rooms.Add(room);

                RoomNumber = 0;
                Capacity = 0;
                Price = 0;
            }
            catch { }
        }

        public bool CanAddRoomCommandExecute(object p) => true;

        #endregion

        #region AddEmployeeCommand

        public ICommand AddEmployeeCommand { get; }

        public void OnAddEmployeeCommandExecuted(object p)
        {
            Employee employee = new Employee();
            employee.LastName = LastName;
            employee.FirstName = FirstName;
            employee.MiddleName = MiddleName == null ? null : MiddleName;
            employee.PhoneNumber = PhoneNumber;

            int count = Employees.Count + 1;
            employee.CurrentFloorId = count % Floors.Count;
            employee.CurrentFloor = Floors[employee.CurrentFloorId];
            employee.CleaningDay = days[(count - 1) % days.Count];

            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                Employees.Add(employee);

                LastName = "";
                FirstName = "";
                MiddleName = "";
                PhoneNumber = "";
            }
            catch { }
            
        }

        public bool CanAddEmployeeCommandExecute(object p) => true;

        #endregion

        #endregion

        public ManageViewModel()
        {
            #region Команды

            AddFloorCommand = new RelayCommand(OnAddFloorCommandExecuted, CanAddFloorCommandExecuted);

            AddRoomCommand = new RelayCommand(OnAddRoomCommandExecuted, CanAddRoomCommandExecute);

            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);

            #endregion

            _context.Floors.Load();
            Floors = _context.Floors.Local.ToObservableCollection();

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();

            _context.Employees.Load();
            Employees = _context.Employees.Local.ToObservableCollection();
        }
    }
}

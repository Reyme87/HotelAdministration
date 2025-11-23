using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HotelAdministration.Commands;
using HotelAdministration.Models;
using HotelAdministration.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace HotelAdministration.ViewModels
{
    public class ManageViewModel : ViewModel
    {
        private static ManageViewModel _instance;
        public static ManageViewModel Instance => _instance ??= new ManageViewModel();

        #region Элементы полей

        #region Floor

        private int _floorNumber;
        private int _totalRooms;

        public int FloorNumber
        {
            get => _floorNumber;
            set
            {
                if (value > 0 && value <= 4)
                {
                    Set(ref _floorNumber, value);
                }
                else
                {
                    Set(ref _floorNumber, 0);
                }
            }
        }

        public int TotalRooms
        {
            get => _totalRooms;
            set
            {
                if (value > 0 && value <= 5)
                {
                    Set(ref _totalRooms, value);
                }
                else
                {
                    Set(ref _totalRooms, 0);
                }
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
                if (value > 0 && value <= 3)
                {
                    Set(ref _capacity, value);
                }
                else
                {
                    Set(ref _capacity, 0);
                }
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
                if (value != "")
                {
                    Set(ref _phoneNumber, value);
                }
            }
        }

        #endregion

        #region Info

        private int _totalPayedAmount;
        private int _totalFreeRoomsAmount;
        private int _totalFreePlacesAmount;
        private int _priceForOnePlace;

        private int _infoRoomNumber;
        private int _infoFloorNumber;

        public int TotalPayedAmount
        {
            get => _totalPayedAmount;
            set
            {
                Set(ref _totalPayedAmount, value);
            }
        }

        public int TotalFreeRoomsAmount
        {
            get => _totalFreeRoomsAmount;
            set
            {
                Set(ref _totalFreeRoomsAmount, value);
            }
        }

        public int TotalFreePlacesAmount
        {
            get => _totalFreePlacesAmount;
            set
            {
                Set(ref _totalFreePlacesAmount, value);
            }
        }

        public int PriceForOnePlace
        {
            get => _priceForOnePlace;
            set
            {
                Set(ref _priceForOnePlace, value);
            }
        }

        public int InfoRoomNumber
        {
            get => _infoRoomNumber;
            set
            {
                Set(ref _infoRoomNumber, value);
            }
        }

        public int InfoFloorNumber
        {
            get => _infoFloorNumber;
            set
            {
                Set(ref _infoFloorNumber, value);
            }
        }

        #endregion

        #region Queries
        private Visibility _visibilityParam = Visibility.Hidden;
        private Visibility _employeesVisibilityParam = Visibility.Hidden;


        private Room _queryRoomType;
        private Client _queryCity;

        private Client _queryClient;
        private string _queryDay;

        private Employee _matchingEmployee;

        public Visibility VisibilityParam
        {
            get => _visibilityParam;
            set
            {
                Set(ref _visibilityParam, value);
            }
        }
        public Visibility EmployeesVisibilityParam
        {
            get => _employeesVisibilityParam;
            set
            {
                Set(ref _employeesVisibilityParam, value);
            }
        }

        public Room QueryRoomType
        {
            get => _queryRoomType;
            set
            {
                Set(ref _queryRoomType, value);
            }
        }

        public Client QueryCity
        {
            get => _queryCity;
            set
            {
                Set(ref _queryCity, value);
            }
        }

        public Client QueryClient
        {
            get => _queryClient;
            set
            {
                Set(ref _queryClient, value);
            }
        }

        public string QueryDay
        {
            get => _queryDay;
            set
            {
                Set(ref _queryDay, value);
            }
        }

        public Employee MatchingEmployee
        {
            get => _matchingEmployee;
            set
            {
                Set(ref _matchingEmployee, value);
            }
        }

        #endregion

        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();
        private ObservableCollection<Floor> _floors;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Client> _clients;
        private ObservableCollection<Client> _matchingClients;

        private readonly List<string> _days = ["Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"];
        private readonly List<string> _statuses = ["Работает", "Убирает", "Отдыхает"];

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

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                Set(ref _clients, value);
            }
        }

        public ObservableCollection<Client> MatchingClients
        {
            get => _matchingClients;
            set
            {
                Set(ref _matchingClients, value);
            }
        }

        private Floor _selectedFloor;
        private Room _selectedRoom;
        private Employee _selectedEmployee;
        private Client _selectedClient;

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

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                Set(ref _selectedClient, value);
            }
        }

        public List<string> Days
        {
            get => _days;
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
                foreach (var floorItem in Floors)
                {
                    if (FloorNumber == floorItem.FloorNumber)
                    {
                        FloorNumber = 0;
                        TotalRooms = 0;
                        MessageBox.Show("Этаж с таким номером уже существует.");
                        return;
                    }
                }

                _context.Floors.Add(floor);
                _context.SaveChanges();
                Floors.Add(floor);

                FloorNumber = 0;
                TotalRooms = 0;
            }
            catch { }
        }

        public bool CanAddFloorCommandExecuted(object p) => !Equals(FloorNumber, 0) && !Equals(TotalRooms, 0);

        #endregion

        #region RemoveFloorCommand

        public ICommand RemoveFloorCommand { get; }

        public void OnRemoveFloorCommandExecuted(object p)
        {
            try
            {
                _context.Floors.Remove(SelectedFloor);
                _context.SaveChanges();
                Floors.Remove(SelectedFloor);
            }
            catch
            {
                MessageBox.Show("Невозможно удалить данные об этаже. На данном этаже работает сотрудник.");
            }
        }

        public bool CanRemoveFloorCommandExecute(object p) => !Equals(SelectedFloor, null);

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

        #region RemoveRoomCommand

        public ICommand RemoveRoomCommand { get; }

        public void OnRemoveRoomCommandExecuted(object p)
        {
            _context.Rooms.Remove(SelectedRoom);
            _context.SaveChanges();
            Rooms.Remove(SelectedRoom);
        }

        public bool CanRemoveRoomCommandExecute(object p) => !Equals(SelectedRoom, null);

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
            employee.CleaningDay = _days[(count - 1) % _days.Count];

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

        #region RemoveEmployeeCommand

        public ICommand RemoveEmployeeCommand { get; }

        public void OnRemoveEmployeeCommandExecuted(object p)
        {
            _context.Employees.Remove(SelectedEmployee);
            _context.SaveChanges();
            Employees.Remove(SelectedEmployee);
        }

        public bool CanRemoveEmployeeCommandExecute(object p) => !Equals(SelectedEmployee, null);

        #endregion

        #region GetPriceCommand

        public ICommand GetPriceCommand { get; }

        public void OnGetPriceCommandExecuted(object p)
        {
            PriceForOnePlace = QueryController.GetPlacePrice(InfoFloorNumber, InfoRoomNumber);
        }

        public bool CanGetPriceCommandExecute(object p) => true;

        #endregion

        #region FindClientByCityCommand

        public ICommand FindClientByCityCommand { get; }

        public void OnFindClientByCityCommandExecuted(object p)
        {
            MatchingClients.Clear();
            var tempList = QueryController.GetClientsByCity(QueryCity.City);

            foreach (var client in tempList)
            {
                MatchingClients.Add(client);
            }

            CheckClientsEmptiness();

            QueryCity = null;
        }

        public bool CanFindClientByCityCommandExecute(object p) => !Equals(QueryCity, null);

        #endregion

        #region FindClientByRoomCommand

        public ICommand FindClientByRoomCommand { get; }

        public void OnFindClientByRoomCommandExecuted(object p)
        {
            MatchingClients.Clear();
            var tempList = QueryController.GetClientsInFixedplacedRooms(QueryRoomType.RoomType);

            foreach (var client in tempList)
            {
                MatchingClients.Add(client);
            }

            CheckClientsEmptiness();

            QueryRoomType = null;
        }

        public bool CanFindClientByRoomCommandExecute(object p) => !Equals(QueryRoomType, null);

        #endregion

        #region FindEmployeeByDayCommand

        public ICommand FindEmployeeByDayCommand { get; }

        public void OnFindEmployeeByDayCommandExecuted(object p)
        {
            var tempList = QueryController.GetCleanerForClient(QueryClient.LastName, QueryClient.FirstName, QueryClient.MiddleName, QueryDay);

            if (tempList.Count != 0)
            {
                EmployeesVisibilityParam = Visibility.Hidden;
                MatchingEmployee = tempList[0];
            }
            else
            {
                EmployeesVisibilityParam = Visibility.Visible;
            }

            QueryClient = null;
            QueryDay = "";
        }

        public bool CanFindEmployeeByDayCommandExecute(object p) => !Equals(QueryClient, null) && !Equals(QueryDay, "");

        #endregion

        #endregion

        public ManageViewModel()
        {
            #region Команды

            AddFloorCommand = new RelayCommand(OnAddFloorCommandExecuted, CanAddFloorCommandExecuted);

            AddRoomCommand = new RelayCommand(OnAddRoomCommandExecuted, CanAddRoomCommandExecute);

            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);

            RemoveFloorCommand = new RelayCommand(OnRemoveFloorCommandExecuted, CanRemoveFloorCommandExecute);

            RemoveRoomCommand = new RelayCommand(OnRemoveRoomCommandExecuted, CanRemoveRoomCommandExecute);

            RemoveEmployeeCommand = new RelayCommand(OnRemoveEmployeeCommandExecuted, CanRemoveEmployeeCommandExecute);

            GetPriceCommand = new RelayCommand(OnGetPriceCommandExecuted, CanGetPriceCommandExecute);

            FindClientByCityCommand = new RelayCommand(OnFindClientByCityCommandExecuted, CanFindClientByCityCommandExecute);

            FindClientByRoomCommand = new RelayCommand(OnFindClientByRoomCommandExecuted, CanFindClientByRoomCommandExecute);

            FindEmployeeByDayCommand = new RelayCommand(OnFindEmployeeByDayCommandExecuted, CanFindEmployeeByDayCommandExecute);

            #endregion

            _context.Floors.Load();
            Floors = _context.Floors.Local.ToObservableCollection();

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();

            _context.Employees.Load();
            Employees = _context.Employees.Local.ToObservableCollection();

            _context.Clients.Load();
            Clients = _context.Clients.Local.ToObservableCollection();

            TotalPayedAmount = QueryController.GetTotalPayedAmount();

            TotalFreeRoomsAmount = QueryController.GetAvailableRoomsCount();

            TotalFreePlacesAmount = QueryController.GetFreePlacesCount();

            MatchingClients = [];
        }

        private void CheckClientsEmptiness()
        {
            if (MatchingClients.Count == 0)
            {
                VisibilityParam = Visibility.Visible;
            }
            else
            {
                VisibilityParam = Visibility.Hidden;
            }
        }
    }
}

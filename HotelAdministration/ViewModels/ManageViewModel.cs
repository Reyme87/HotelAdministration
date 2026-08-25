using HotelAdministration.Commands;
using HotelAdministration.Models;
using HotelAdministration.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace HotelAdministration.ViewModels
{
    public class ManageViewModel : ViewModel
    {
        //Создание единственного экземпляра ViewModel по принципу Singleton
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
                Set(ref _phoneNumber, value);
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
        private List<string> _categories = ["Одноместный", "Двухместный", "Трёхместный"];
        private string _singleCategory;

        private static DateTime _currentDateTime;
        private static DateOnly _today;
        private static CultureInfo _culture = new CultureInfo("ru-RU");

        public List<string> Categories
        {
            get => _categories;
        }

        public string SingleCategory
        {
            get => _singleCategory;
            set
            {
                Set(ref _singleCategory, value);
            }
        }

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
        //Команда добавления данных об этаже
        public ICommand AddFloorCommand { get; }

        public void OnAddFloorCommandExecuted(object p)
        {
            Floor floor = new Floor();
            floor.FloorNumber = FloorNumber;
            floor.TotalRoomsNumber = TotalRooms;

            try
            {
                //Перебор этажей с последующим добавлением новых и уведомлением о существующих
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

                FloorNumber = 0;
                TotalRooms = 0;
            }
            catch { }
        }

        public bool CanAddFloorCommandExecuted(object p) => !Equals(FloorNumber, 0) && !Equals(TotalRooms, 0);

        #endregion

        #region RemoveFloorCommand
        //Команда удаления данных об этаже
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
        //Команда добавления данных о комнате
        public ICommand AddRoomCommand { get; }

        public void OnAddRoomCommandExecuted(object p)
        {
            Room room = new Room();
            room.RoomNumber = RoomNumber;

            room.FloorId = SelectedFloor.FloorId;

            room.Capacity = Capacity;
            room.PricePerNumber = Price;
            switch (Capacity)
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
                foreach (var roomItem in Rooms)
                {
                    if (RoomNumber == roomItem.RoomNumber)
                    {
                        RoomNumber = 0;
                        Capacity = 0;
                        Price = 0;
                        MessageBox.Show("Комната с таким номером уже существует.");
                        return;
                    }
                }

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
        //Команда удаления данных о комнате
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
        //Команда добавления данных о сотруднике
        public ICommand AddEmployeeCommand { get; }

        public void OnAddEmployeeCommandExecuted(object p)
        {
            Employee employee = new Employee();
            employee.LastName = LastName;
            employee.FirstName = FirstName;
            employee.MiddleName ??= MiddleName;
            employee.PhoneNumber = PhoneNumber;

            int count = Employees.Count + 1;
            employee.CurrentFloorId = count % Floors.Count;
            employee.CurrentFloor = Floors[employee.CurrentFloorId];
            employee.CleaningDay = _days[(count - 1) % _days.Count];

            CheckCleaningState(ref employee);

            if (Employees.Any(e => e.PhoneNumber == PhoneNumber))
            {
                MessageBox.Show("Сотрудник с таким номером телефона уже существует!");
                return;
            }

            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();

                LastName = "";
                FirstName = "";
                MiddleName = "";
                PhoneNumber = "";
            }
            catch { }

        }

        public bool CanAddEmployeeCommandExecute(object p) => !Equals(LastName, "")
                                                           && !Equals(FirstName, "")
                                                           && !Equals(PhoneNumber, "");

        #endregion

        #region RemoveEmployeeCommand
        //Команда удаления данных о сотруднике (увольнение)
        public ICommand RemoveEmployeeCommand { get; }

        public void OnRemoveEmployeeCommandExecuted(object p)
        {
            _context.Employees.Remove(SelectedEmployee);
            _context.SaveChanges();
            Employees.Remove(SelectedEmployee);
        }

        public bool CanRemoveEmployeeCommandExecute(object p) => !Equals(SelectedEmployee, null);

        #endregion

        #region RemoveClientCommand
        //Команда удаления данных о клиенте

        public ICommand RemoveClientCommand { get; }

        public void OnRemoveClientCommandExecuted(object p)
        {
            _context.Clients.Remove(SelectedClient);
            _context.SaveChanges();
            Clients.Remove(SelectedClient);
        }

        public bool CanRemoveClientCommandExecute(object p) => !Equals(SelectedClient, null);

        #endregion

        #region GetPriceCommand
        //Команда подсчёта цены за одно место в выбранной комнате
        public ICommand GetPriceCommand { get; }

        public void OnGetPriceCommandExecuted(object p)
        {
            PriceForOnePlace = QueryController.GetPlacePrice(InfoFloorNumber, InfoRoomNumber);
        }

        public bool CanGetPriceCommandExecute(object p) => true;

        #endregion

        #region FindClientByCityCommand
        //Команда поиска клиента по городу
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
        //Команда поиска клиента по типу комнаты
        public ICommand FindClientByRoomCommand { get; }

        public void OnFindClientByRoomCommandExecuted(object p)
        {
            MatchingClients.Clear();
            var tempList = QueryController.GetClientsInFixedplacedRooms(SingleCategory);

            foreach (var client in tempList)
            {
                MatchingClients.Add(client);
            }

            CheckClientsEmptiness();

            QueryRoomType = null;
        }

        public bool CanFindClientByRoomCommandExecute(object p) => SingleCategory != null;

        #endregion

        #region FindEmployeeByDayCommand
        //Команда поиска сотрудника, убиравшего этаж указанного клиента в заданный день
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

        #region SetClientAsArrivedCommand
        //Команда отметки клиента как прибывшего
        public ICommand SetClientAsArrivedCommand { get; }

        public void OnSetClientAsArrivedCommandExecuted(object p)
        {
            //Сбор начального взноса по прибытии
            SelectedClient.HasArrived = true;
            SelectedClient.PayedAmount += SelectedClient.MoneyToPay;
            SelectedClient.MoneyToPay = 0;

            //Обновление БД
            _context.Clients.Where(c => c.ClientId == SelectedClient.ClientId)
                            .ExecuteUpdate(s =>
                            s.SetProperty(c => c.HasArrived, SelectedClient.HasArrived)
                             .SetProperty(c => c.PayedAmount, SelectedClient.PayedAmount)
                             .SetProperty(c => c.MoneyToPay, SelectedClient.MoneyToPay));
        }

        public bool CanSetClientAsArrivedCommandExecute(object p) => !Equals(SelectedClient, null) && !Equals(SelectedClient.HasArrived, true) && SelectedClient.ArrivalDate <= _today;

        #endregion

        #region SetClientAsCheckedOutCommand
        //Команда отметки клиента как съехавшего
        public ICommand SetClientAsCheckedOutCommand { get; }

        public void OnSetClientAsCheckedOutCommandExecuted(object p)
        {
            SelectedClient.HasCheckedOut = true;

            //Сравнение дат
            DateOnly checkOutDate = SelectedClient.CheckOutDate.CompareTo(_today) <= 0 ? SelectedClient.CheckOutDate : _today;

            TimeSpan diff = checkOutDate.ToDateTime(TimeOnly.MinValue) - SelectedClient.ArrivalDate.ToDateTime(TimeOnly.MinValue);
            int days = diff.Days;

            //Вычисление конечной стоимости проживания
            int moneyToPay = days * QueryController.GetPlacePrice(SelectedClient.BookedRoom.Floor.FloorNumber, SelectedClient.BookedRoom.RoomNumber);

            SelectedClient.PayedAmount += moneyToPay;

            //Обновление БД
            _context.Clients.Where(c => c.ClientId == SelectedClient.ClientId)
                            .ExecuteUpdate(s =>
                            s.SetProperty(c => c.HasCheckedOut, SelectedClient.HasCheckedOut)
                             .SetProperty(c => c.PayedAmount, SelectedClient.PayedAmount));

            _context.Rooms.Where(c => c.RoomId == SelectedClient.BookedRoomId)
                          .ExecuteUpdate(s =>
                          s.SetProperty(c => c.IsAvailable, true)
                           .SetProperty(c => c.IsBooked, false)
                           .SetProperty(c => c.FreePlaces, c => c.Capacity));

            _context.SaveChanges();

            //Изменение показателей статистики в окне информации
            TotalPayedAmount = QueryController.GetTotalPayedAmount();

            TotalFreeRoomsAmount = QueryController.GetAvailableRoomsCount();

            TotalFreePlacesAmount = QueryController.GetFreePlacesCount();
        }

        public bool CanSetClientAsCheckedOutCommandExecute(object p) => !Equals(SelectedClient, null)
                                                                     && !Equals(SelectedClient.HasCheckedOut, true)
                                                                     && Equals(SelectedClient.HasArrived, true);

        #endregion

        #endregion

        public ManageViewModel()
        {
            #region Команды
            //Регистрация команд

            AddFloorCommand = new RelayCommand(OnAddFloorCommandExecuted, CanAddFloorCommandExecuted);

            AddRoomCommand = new RelayCommand(OnAddRoomCommandExecuted, CanAddRoomCommandExecute);

            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);

            RemoveFloorCommand = new RelayCommand(OnRemoveFloorCommandExecuted, CanRemoveFloorCommandExecute);

            RemoveRoomCommand = new RelayCommand(OnRemoveRoomCommandExecuted, CanRemoveRoomCommandExecute);

            RemoveEmployeeCommand = new RelayCommand(OnRemoveEmployeeCommandExecuted, CanRemoveEmployeeCommandExecute);

            RemoveClientCommand = new RelayCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);

            GetPriceCommand = new RelayCommand(OnGetPriceCommandExecuted, CanGetPriceCommandExecute);

            FindClientByCityCommand = new RelayCommand(OnFindClientByCityCommandExecuted, CanFindClientByCityCommandExecute);

            FindClientByRoomCommand = new RelayCommand(OnFindClientByRoomCommandExecuted, CanFindClientByRoomCommandExecute);

            FindEmployeeByDayCommand = new RelayCommand(OnFindEmployeeByDayCommandExecuted, CanFindEmployeeByDayCommandExecute);

            SetClientAsArrivedCommand = new RelayCommand(OnSetClientAsArrivedCommandExecuted, CanSetClientAsArrivedCommandExecute);

            SetClientAsCheckedOutCommand = new RelayCommand(OnSetClientAsCheckedOutCommandExecuted, CanSetClientAsCheckedOutCommandExecute);

            #endregion

            //Загрузка данных в коллекции из БД
            _context.Floors.Load();
            Floors = _context.Floors.Local.ToObservableCollection();

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();

            _context.Employees.Load();
            Employees = _context.Employees.Local.ToObservableCollection();

            _context.Clients.Load();
            Clients = _context.Clients.Local.ToObservableCollection();

            //Загрузка статистики в окно информации
            TotalPayedAmount = QueryController.GetTotalPayedAmount();

            TotalFreeRoomsAmount = QueryController.GetAvailableRoomsCount();

            TotalFreePlacesAmount = QueryController.GetFreePlacesCount();

            MatchingClients = [];

            _currentDateTime = DateTime.Now;
            _today = DateOnly.FromDateTime(_currentDateTime);

            CheckEmployeesStatuses();
        }

        //Проверка наличия клиентов в списке
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

        //Метод проверки текущего статуса работника 
        private bool CheckCleaningState(ref Employee employee)
        {
            string russianCurrentDay = _culture.DateTimeFormat.GetDayName(_today.DayOfWeek);
            Console.WriteLine(russianCurrentDay);

            if (Equals(russianCurrentDay, employee.CleaningDay.ToLower()))
            {
                if (!Equals(employee.Status, _statuses[1]))
                {
                    employee.Status = _statuses[1];
                    return true;
                }
            }
            else
            {
                int currentDayIndex = 0;
                for (int i = 0; i < Days.Count; i++)
                {
                    if (Equals(Days[i].ToLower(), russianCurrentDay))
                    {
                        currentDayIndex = i;
                    }
                }

                int dayBeforeYesterdayIndex = 0;

                if (currentDayIndex == 0)
                {
                    dayBeforeYesterdayIndex = Days.Count - 2;
                }
                else if (currentDayIndex == 1)
                {
                    dayBeforeYesterdayIndex = Days.Count - 1;
                }
                else
                {
                    dayBeforeYesterdayIndex = currentDayIndex - 2;
                }

                if (Equals(employee.Status, _statuses[1]))
                {
                    employee.Status = _statuses[2];
                    return true;
                }
                if (Equals(employee.Status, _statuses[2]) && Equals(employee.CleaningDay.ToLower(), Days[dayBeforeYesterdayIndex].ToLower()))
                {
                    (string day, int floor) = FindNewCleaningDayAndFloor(employee.CurrentFloorId);
                    employee.Status = _statuses[0];
                    employee.CleaningDay = day;
                    employee.CurrentFloorId = floor;

                    return true;
                }
            }

            return false;
        }

        //Метод проверки и изменения статусов всех работников
        private void CheckEmployeesStatuses()
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                var employee = Employees[i];
                //Проверка и обновление БД в случае изменения статуса
                if (CheckCleaningState(ref employee))
                {
                    Employees[i] = employee;
                    _context.Employees.Where(e => e.EmployeeId == Employees[i].EmployeeId)
                                      .ExecuteUpdate(s =>
                                      s.SetProperty(e => e.Status, Employees[i].Status)
                                       .SetProperty(e => e.CurrentFloorId, Employees[i].CurrentFloorId)
                                       .SetProperty(e => e.CleaningDay, Employees[i].CleaningDay));
                }

            }
        }

        //Метод подбора нового дня и этажа для уборки
        private (string?, int) FindNewCleaningDayAndFloor(int currentFloor)
        {
            List<string> cleaningDays = new List<string>();
            List<int> floors = new List<int>();

            for (int i = 0; i < Employees.Count; i++)
            {
                cleaningDays.Add(Employees[i].CleaningDay.ToLower());
                floors.Add(Employees[i].CurrentFloorId);
            }

            string? day = null;

            for (int i = 0; i < Days.Count; i++)
            {
                if (!cleaningDays.Contains(Days[i].ToLower()))
                {
                    day = Days[i];
                }
            }

            int floor = currentFloor;

            for (int i = 0; i < Floors.Count; i++)
            {
                if (!floors.Contains(Floors[i].FloorId))
                {
                    floor = Floors[i].FloorId;
                }
            }

            return (day, floor);
        }
    }
}

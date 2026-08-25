using HotelAdministration.Commands;
using HotelAdministration.Models;
using HotelAdministration.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HotelAdministration.ViewModels
{
    internal class GuestViewModel : ViewModel
    {
        //Создание единственного экземпляра ViewModel по принципу Singleton
        private static GuestViewModel _instance;

        public static GuestViewModel Instance => _instance ??= new GuestViewModel();

        #region Элементы полей

        private static Visibility _visibility = Visibility.Hidden;

        private int? _priceForOnePlace = 0;

        private string _lastName;
        private string _firstName;
        private string? _middleName;
        private string _phoneNumber;
        private string _email;
        private string _city;
        private DateOnly _checkInDate;
        private DateOnly _checkOutDate;

        public Visibility VisibilityParam
        {
            get => _visibility;
            set
            {
                Set(ref _visibility, value);
            }
        }

        public int? PriceForOnePlace
        {
            get => _priceForOnePlace;
            set
            {
                Set(ref _priceForOnePlace, value);
            }
        }

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

        public string? MiddleName
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

        public string Email
        {
            get => _email;
            set
            {
                Set(ref _email, value);
            }
        }

        public string City
        {
            get => _city;
            set
            {
                Set(ref _city, value);
            }
        }

        public DateOnly CheckInDate
        {
            get => _checkInDate;
            set
            {
                Set(ref _checkInDate, value);
            }
        }

        public DateOnly CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                Set(ref _checkOutDate, value);
            }
        }

        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();

        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Room> _selectedRooms;
        private RoomPreview _selectedPreview;
        private Room _selectedRoomBooking;
        private int _iteration = 0;

        private ObservableCollection<Client> _clients;
        private Client _selectedClient;

        private static DateTime _currentDateTime;
        private static DateOnly _today;

        private ObservableCollection<RoomPreview> _previews { get; set; }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                Set(ref _rooms, value);
            }
        }

        public ObservableCollection<Room> SelectedRooms
        {
            get => _selectedRooms;
            set
            {
                Set(ref _selectedRooms, value);
            }
        }

        public RoomPreview SelectedPreview
        {
            get => _selectedPreview;
            set
            {
                Set(ref _selectedPreview, value);
            }
        }

        public Room SelectedRoomBooking
        {
            get => _selectedRoomBooking;
            set
            {
                Set(ref _selectedRoomBooking, value);
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

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                Set(ref _selectedClient, value);
            }
        }

        #endregion

        #region Команды

        #region SwipeRightCommand
        //Команда переключения панелей с информацией вправо
        public ICommand SwipeRightCommand { get; }

        public void OnSwipeRightCommandExecuted(object p)
        {
            _iteration++;
            if (_iteration >= _previews.Count)
            {
                _iteration = 0;
            }

            SelectedPreview = _previews[_iteration];
        }

        public bool CanSwipeRightCommandExecute(object p) => true;

        #endregion

        #region SwipeLeftCommand
        //Команда переключения панелей с информацией вправо
        public ICommand SwipeLeftCommand { get; }

        public void OnSwipLeftCommandExecuted(object p)
        {
            _iteration--;
            if (_iteration < 0)
            {
                _iteration = _previews.Count - 1;
            }

            SelectedPreview = _previews[_iteration];
        }

        public bool CanSwipLeftCommandExecute(object p) => true;

        #endregion

        #region WatchRoomsCommand
        //Команда отображения доступных комнат по выбранному типу
        public ICommand WatchRoomsCommand { get; }

        public void OnWatchRoomsCommandExecuted(object p)
        {
            //Поиск и запись в коллекцию соответствующих комнат
            SelectedRooms.Clear();
            foreach (var room in Rooms)
            {
                if (Equals(room.RoomType, SelectedPreview.Type) && Equals(room.IsBooked, false) && Equals(room.IsAvailable, true))
                {
                    SelectedRooms.Add(room);
                }
            }

            //Проверка наличия доступных комнат для отображения/скрытия уведомления
            if (SelectedRooms.Count == 0)
            {
                VisibilityParam = Visibility.Visible;
            }
            else
            {
                VisibilityParam = Visibility.Hidden;
            }
        }

        public bool CanWatchRoomsCommandExecute(object p) => true;

        #endregion

        #region PreBookRoomCommand
        //Команда выбора комнаты для дальнейшего бронирования
        public ICommand PreBookRoomCommand { get; }

        public void OnPreBookRoomCommandExecuted(object p)
        {
            int index = SelectedRooms.IndexOf((Room)p);
            SelectedRoomBooking = SelectedRooms[index];
        }

        public bool CanPreBookRoomCommandExecute(object p) => true;

        #endregion

        #region CalculatePriceCommand
        //Команда подсчёта цены за одно место в выбранной комнате
        public ICommand CalculatePriceCommand { get; }

        public void OnCalculatePriceCommandExecuted(object p)
        {
            PriceForOnePlace = QueryController.GetPlacePrice(SelectedRoomBooking.FloorId, SelectedRoomBooking.RoomNumber);
        }

        public bool CanCalculatePriceCommandExecute(object p) => !Equals(SelectedRoomBooking, null);

        #endregion

        #region AddClientCommand
        //Команда регистрации добавления клиента в список на заселение
        public ICommand AddClientCommand { get; }

        public void OnAddClientCommandExecuted(object p)
        {
            Client client = new Client();
            client.LastName = LastName;
            client.FirstName = FirstName;
            client.MiddleName ??= MiddleName;
            client.PhoneNumber = PhoneNumber;
            client.Email = Email;
            client.City = City;
            client.ArrivalDate = CheckInDate;
            client.CheckOutDate = CheckOutDate;

            try
            {
                if (CheckInDate > CheckOutDate || CheckInDate < _today)
                {
                    MessageBox.Show("Выберите корректную дату.");
                }

                Clients.Add(client);

                LastName = "";
                FirstName = "";
                MiddleName = null;
                PhoneNumber = "";
                Email = "";
                City = "";
            }
            catch { }
        }

        public bool CanAddClientCommandExecute(object p) => !Equals(LastName, null) && !Equals(FirstName, null) && !Equals(PhoneNumber, null) && !Equals(Email, null) && (Clients.Count + 1) <= SelectedRoomBooking.Capacity &&
                                                            !Equals(City, null) && !Equals(CheckInDate, null) && !Equals(CheckOutDate, null) && Clients.Count < SelectedRoomBooking.Capacity;

        #endregion

        #region RemoveClientCommand
        //Команда удаления клиента из списка на заселение
        public ICommand RemoveClientCommand { get; }

        public void OnRemoveClientCommandExecuted(object p)
        {
            Clients.Remove(SelectedClient);
        }

        public bool CanRemoveClientCommandExecute(object p) => !Equals(SelectedClient, null);

        #endregion

        #region BookRoomCommand
        //Команда бронирования комнаты
        public ICommand BookRoomCommand { get; }

        public void OnBookRoomCommandExecuted(object p)
        {
            try
            {
                List<Client> existingClients = new List<Client>();
                _context.Clients.Load();
                existingClients = _context.Clients.Local.ToList();

                //Перебор клиентов для последовательного добавления в БД
                foreach (var client in Clients)
                {
                    using var command = _context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "SELECT get_place_price(@floor_number, @room_number)";
                    command.Parameters.Add(new NpgsqlParameter("@floor_number", SelectedRoomBooking.FloorId));
                    command.Parameters.Add(new NpgsqlParameter("@room_number", SelectedRoomBooking.RoomNumber));

                    _context.Database.OpenConnection();

                    //Проверка существования клиента с последующим изменением его статуса пребывания в комнате 
                    var price = (int)command.ExecuteScalar();
                    if (existingClients.Any(s => s.FirstName == client.FirstName && s.LastName == client.LastName && s.MiddleName == client.MiddleName))
                    {
                        _context.Clients
                            .Where(c => c.ClientId == client.ClientId)
                            .ExecuteUpdate(s =>
                            s.SetProperty(c => c.MoneyToPay, price));

                        _context.Database.CloseConnection();
                    }
                    else
                    {
                        client.MoneyToPay = price;

                        _context.Database.CloseConnection();

                        client.BookedRoomId = SelectedRoomBooking.RoomId;
                        _context.Clients.Add(client);
                    }
                }

                //Обновление данных о комнатах в БД
                _context.Rooms
                        .Where(c => c.RoomId == SelectedRoomBooking.RoomId)
                        .ExecuteUpdate(s =>
                        s.SetProperty(c => c.IsBooked, true)
                        .SetProperty(c => c.IsAvailable, false)
                        .SetProperty(c => c.FreePlaces, SelectedRoomBooking.Capacity - Clients.Count));

                _context.SaveChanges();

                Clients.Clear();
                SelectedRooms.Remove(SelectedRoomBooking);
                SelectedRooms.Clear();

                Rooms.Clear();
                _context.Rooms.Load();
                Rooms = _context.Rooms.Local.ToObservableCollection();
            }
            catch { }
        }

        public bool CanBookRoomCommandExecute(object p) => !Equals(Clients.Count, 0);

        #endregion

        #endregion

        public GuestViewModel()
        {
            #region Команды
            //Регистрация команд

            SwipeRightCommand = new RelayCommand(OnSwipeRightCommandExecuted, CanSwipeRightCommandExecute);

            SwipeLeftCommand = new RelayCommand(OnSwipLeftCommandExecuted, CanSwipLeftCommandExecute);

            WatchRoomsCommand = new RelayCommand(OnWatchRoomsCommandExecuted, CanWatchRoomsCommandExecute);

            PreBookRoomCommand = new RelayCommand(OnPreBookRoomCommandExecuted, CanPreBookRoomCommandExecute);

            CalculatePriceCommand = new RelayCommand(OnCalculatePriceCommandExecuted, CanCalculatePriceCommandExecute);

            AddClientCommand = new RelayCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);

            RemoveClientCommand = new RelayCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);

            BookRoomCommand = new RelayCommand(OnBookRoomCommandExecuted, CanBookRoomCommandExecute);

            #endregion

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();

            //Создание списка с превью комнат
            _previews = [new RoomPreview()
            {
                PicSource = "/Resources/OnePlace.jpg",
                Type = "Одноместный",
                Capacity = 1,
                MinPrice = GetPrices(1).min,
                MaxPrice = GetPrices(1).max,
            },
            new RoomPreview()
            {
                PicSource = "/Resources/TwoPlaces.jpg",
                Type = "Двухместный",
                Capacity = 2,
                MinPrice = GetPrices(2).min,
                MaxPrice = GetPrices(2).max,
            },
            new RoomPreview()
            {
                PicSource = "/Resources/ThreePlaces.jpg",
                Type = "Трёхместный",
                Capacity = 3,
                MinPrice = GetPrices(3).min,
                MaxPrice = GetPrices(3).max,
            },
            ];
            SelectedPreview = _previews[_iteration];
            SelectedRooms = new ObservableCollection<Room>();

            Clients = new ObservableCollection<Client>();

            _currentDateTime = DateTime.Now;
            _today = DateOnly.FromDateTime(_currentDateTime);
        }

        //Метод подсчёта диапазона цен у комнат
        private (int min, int max) GetPrices(int capacity)
        {
            int minPrice = 1000000;
            int maxPrice = -1000000;
            foreach (var room in Rooms)
            {
                if (room.Capacity == capacity)
                {
                    minPrice = Math.Min(minPrice, room.PricePerNumber);
                    maxPrice = Math.Max(maxPrice, room.PricePerNumber);
                }
            }

            return (minPrice, maxPrice);
        }
    }
}

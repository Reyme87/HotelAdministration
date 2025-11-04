using HotelAdministration.Commands;
using HotelAdministration.Models;
using HotelAdministration.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelAdministration.ViewModels
{
    internal class GuestViewModel : ViewModel
    {
        private static GuestViewModel _instance;

        public static GuestViewModel Instance => _instance ??= new GuestViewModel();

        #region Элементы полей

        private string _lastName;
        private string _firstName;
        private string? _middleName;
        private string _phoneNumber;
        private string _email;
        private string _city;
        private DateOnly _checkInDate;
        private DateOnly _checkOutDate;

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

        public ICommand WatchRoomsCommand { get; }

        public void OnWatchRoomsCommandExecuted(object p)
        {
            SelectedRooms.Clear();
            foreach (var room in Rooms)
            {
                if (Equals(room.RoomType, SelectedPreview.Type) && Equals(room.IsBooked, false) && Equals(room.IsAvailable, true))
                {
                    SelectedRooms.Add(room);
                }
            }
        }

        public bool CanWatchRoomsCommandExecute(object p) => true;

        #endregion

        #region PreBookRoomCommand

        public ICommand PreBookRoomCommand { get; }

        public void OnPreBookRoomCommandExecuted(object p)
        {
            int index = SelectedRooms.IndexOf((Room)p);
            SelectedRoomBooking = SelectedRooms[index];
        }

        public bool CanPreBookRoomCommandExecute(object p) => true;

        #endregion

        #region AddClientCommand

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

        public bool CanAddClientCommandExecute(object p) => !Equals(LastName, null) && !Equals(FirstName, null) && !Equals(PhoneNumber, null) && !Equals(Email, null) &&
                                                            !Equals(City, null) && !Equals(CheckInDate, null) && !Equals(CheckOutDate, null) && CheckInDate <= CheckOutDate && CheckInDate >= _today;

        #endregion

        #region RemoveClientCommand

        public ICommand RemoveClientCommand { get; }

        public void OnRemoveClientCommandExecuted(object p)
        {
            Clients.Remove(SelectedClient);
        }

        public bool CanRemoveClientCommandExecute(object p) => !Equals(SelectedClient, null);

        #endregion

        #region BookRoomCommand

        public ICommand BookRoomCommand { get; }

        public void OnBookRoomCommandExecuted(object p)
        {
            try
            {
                foreach (var client in Clients)
                {
                    client.PayedAmount = (int)Math.Round(SelectedRoomBooking.PricePerNumber / Clients.Count * 1.0, 0);
                    client.BookedRoomId = SelectedRoomBooking.RoomId;
                    _context.Clients.Add(client);
                    _context.SaveChanges();
                }
            }
            catch { }
        }

        public bool CanBookRoomCommandExecute(object p) => !Equals(Clients.Count, 0);

        #endregion

        #endregion

        public GuestViewModel()
        {
            #region Команды

            SwipeRightCommand = new RelayCommand(OnSwipeRightCommandExecuted, CanSwipeRightCommandExecute);

            SwipeLeftCommand = new RelayCommand(OnSwipLeftCommandExecuted, CanSwipLeftCommandExecute);

            WatchRoomsCommand = new RelayCommand(OnWatchRoomsCommandExecuted, CanWatchRoomsCommandExecute);

            PreBookRoomCommand = new RelayCommand(OnPreBookRoomCommandExecuted, CanPreBookRoomCommandExecute);

            AddClientCommand = new RelayCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);

            RemoveClientCommand = new RelayCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);

            BookRoomCommand = new RelayCommand(OnBookRoomCommandExecuted, CanBookRoomCommandExecute);

            #endregion

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion

        #region Коллекции элементов

        private HotelContext _context = new HotelContext();
        public ObservableCollection<Floor> Floors { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private Floor _selectedFloor;

        public Floor SelectedFloor
        {
            get => _selectedFloor;
            set
            {
                Set(ref _selectedFloor, value);
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

            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public bool CanAddRoomCommandExecute(object p) => true;

        #endregion

        #endregion

        public ManageViewModel()
        {
            #region Команды

            AddFloorCommand = new RelayCommand(OnAddFloorCommandExecuted, CanAddFloorCommandExecuted);

            AddRoomCommand = new RelayCommand(OnAddRoomCommandExecuted, CanAddRoomCommandExecute);

            #endregion

            _context.Floors.Load();
            Floors = _context.Floors.Local.ToObservableCollection();

            _context.Rooms.Load();
            Rooms = _context.Rooms.Local.ToObservableCollection();
        }
    }
}

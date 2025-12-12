using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Windows;

namespace HotelAdministration.Models
{
    internal static class QueryController
    {
        private static HotelContext _context = new HotelContext();

        //Метод подсчёта доступных комнат
        public static int GetAvailableRoomsCount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_available_rooms_count()";
            _context.Database.OpenConnection();

            var result = command.ExecuteScalar();
            int availableRoomsCount = 0;

            if (result != null && result != DBNull.Value)
            {
                availableRoomsCount = (int)result;
            }

            _context.Database.CloseConnection();

            return availableRoomsCount;
        }

        //Метод поиска сотрудников, убиравших номер заданного клиента в указанный день
        public static List<Employee> GetCleanerForClient(string clientLastName, string clientFirstName, string clientMiddleName, string dayOfWeek)
        {
            try
            {
                var employees = _context.Database.SqlQueryRaw<Employee>("SELECT * FROM get_cleaner_for_client({0}, {1}, {2}, {3})",
                                                                        clientLastName, clientFirstName, clientMiddleName, dayOfWeek).ToList();

                foreach (var employee in employees)
                {
                    _context.Entry(employee)
                        .Reference(c => c.CurrentFloor)
                        .Load();
                }

                return employees;
            }
            catch
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Employee>();
            }
        }

        //Метод поиска клиентов, прибывших из указанного города
        public static List<Client> GetClientsByCity(string cityName)
        {
            try
            {
                var clients = _context.Database.SqlQueryRaw<Client>("SELECT * FROM get_clients_by_city({0})", cityName).ToList();

                foreach (var client in clients)
                {
                    _context.Entry(client)
                        .Reference(c => c.BookedRoom)
                        .Load();
                }

                return clients;
            }
            catch
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Client>();
            }
        }

        //Метод поиска клиентов, проживающий в комнатах указанного типа
        public static List<Client> GetClientsInFixedplacedRooms(string roomType)
        {
            try
            {
                var clients = _context.Database.SqlQueryRaw<Client>("SELECT * FROM get_clients_in_fixedplaced_rooms({0})", roomType).ToList();

                foreach (var client in clients)
                {
                    _context.Entry(client)
                        .Reference(c => c.BookedRoom)
                        .Load();
                }

                return clients;
            }
            catch 
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Client>();
            }
        }

        //Метод подсчёта свободных мест
        public static int GetFreePlacesCount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_free_places_count()";
            _context.Database.OpenConnection();

            var result = command.ExecuteScalar();
            int freePlacesCount = 0;

            if (result != null && result != DBNull.Value)
            {
                freePlacesCount = (int)result;
            }

            _context.Database.CloseConnection();

            return freePlacesCount;
        }

        //Метод подсчёта стоимости одного места в указанной комнате на заданном этаже
        public static int GetPlacePrice(int floorNumber, int roomNumber)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_place_price(@floor_number, @room_number)";
            command.Parameters.Add(new NpgsqlParameter("@floor_number", floorNumber));
            command.Parameters.Add(new NpgsqlParameter("@room_number", roomNumber));

            _context.Database.OpenConnection();

            var result = command.ExecuteScalar();
            int price = 0;

            if (result != null && result != DBNull.Value)
            {
                price = (int)result;
            }

            _context.Database.CloseConnection();

            return price;
        }

        //Метод подсчёта общей выплаченной клиентами суммы
        public static int GetTotalPayedAmount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_total_payed_amount()";

            _context.Database.OpenConnection();

            var result = command.ExecuteScalar();
            int payedAmount = 0;

            if (result != null && result != DBNull.Value)
            {
                payedAmount = (int)result;
            }

            _context.Database.CloseConnection();

            return payedAmount;
        }
    }
}

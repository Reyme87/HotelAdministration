using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Windows;

namespace HotelAdministration.Models
{
    internal static class QueryController
    {
        private static HotelContext _context = new HotelContext();

        public static int? GetAvailableRoomsCount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_available_rooms_count()";
            _context.Database.OpenConnection();

            int? availableRoomsCount = (int?)command.ExecuteScalar() ?? 0;

            _context.Database.CloseConnection();

            return availableRoomsCount;
        }

        public static List<Employee> GetCleanerForClient(string clientLastName, string clientFirstName, string clientMiddleName, string dayOfWeek)
        {
            try
            {
                var employees = _context.Database.SqlQueryRaw<Employee>("SELECT * FROM get_cleaner_for_client({0}, {1}, {2}, {3})",
                                                                        clientLastName, clientFirstName, clientMiddleName, dayOfWeek).ToList();

                return employees;
            }
            catch
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Employee>();
            }
        }

        public static List<Client> GetClientsByCity(string cityName)
        {
            try
            {
                var clients = _context.Database.SqlQueryRaw<Client>("SELECT * FROM get_clients_by_city({0})", cityName).ToList();

                return clients;
            }
            catch
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Client>();
            }
        }

        public static List<Client> GetClientsInFixedplacedRooms(string roomType)
        {
            try
            {
                var clients = _context.Database.SqlQueryRaw<Client>("SELECT * FROM get_clients_in_fixedplaced_rooms({0})", roomType).ToList();

                return clients;
            }
            catch 
            {
                MessageBox.Show($"Ошибка вызова функции.");
                return new List<Client>();
            }
        }

        public static int? GetFreePlacesCount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_free_places_count()";
            _context.Database.OpenConnection();

            int? freePlacesCount = (int?)command.ExecuteScalar() ?? 0;

            _context.Database.CloseConnection();

            return freePlacesCount;
        }

        public static int? GetPlacePrice(int floorNumber, int roomNumber)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_place_price(@floor_number, @room_number)";
            command.Parameters.Add(new NpgsqlParameter("@floor_number", floorNumber));
            command.Parameters.Add(new NpgsqlParameter("@room_number", roomNumber));

            _context.Database.OpenConnection();

            int? price = (int?)command.ExecuteScalar() ?? 0;

            _context.Database.CloseConnection();

            return price;
        }

        public static int? GetTotalPayedAmount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT get_total_payed_amount()";

            _context.Database.OpenConnection();

            int? payedAmount = (int?)command.ExecuteScalar() ?? 0;

            _context.Database.CloseConnection();

            return payedAmount;
        }
    }
}

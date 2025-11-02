using System;
using System.Collections.Generic;

namespace HotelAdministration;

public partial class Client
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public int BookedRoomId { get; set; }

    public int PayedAmount { get; set; }

    public DateOnly ArrivalDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    public virtual Room BookedRoom { get; set; } = null!;
}

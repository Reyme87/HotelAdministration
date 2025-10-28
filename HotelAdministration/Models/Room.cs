using System;
using System.Collections.Generic;

namespace HotelAdministration;

public partial class Room
{
    public int RoomId { get; set; }

    public int FloorId { get; set; }

    public int RoomNumber { get; set; }

    public string RoomType { get; set; } = null!;

    public int PricePerNumber { get; set; }

    public int Capacity { get; set; }

    public int FreePlaces { get; set; }

    public bool? IsBooked { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual Floor Floor { get; set; } = null!;
}

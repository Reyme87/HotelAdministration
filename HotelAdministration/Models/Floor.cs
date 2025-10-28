using System;
using System.Collections.Generic;

namespace HotelAdministration;

public partial class Floor
{
    public int FloorId { get; set; }

    public int FloorNumber { get; set; }

    public int? TotalRoomsNumber { get; set; }


    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}

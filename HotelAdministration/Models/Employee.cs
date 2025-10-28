using System;
using System.Collections.Generic;

namespace HotelAdministration;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int CurrentFloorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public virtual Floor CurrentFloor { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAdministration;

public partial class Employee
{
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("current_floor_id")]
    public int CurrentFloorId { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("day_of_cleaning")]
    public string? CleaningDay { get; set; }

    [NotMapped]
    public virtual Floor CurrentFloor { get; set; } = null!;
}

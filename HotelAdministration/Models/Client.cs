using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAdministration;

public partial class Client
{
    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("booked_room_id")]
    public int BookedRoomId { get; set; }

    [Column("payed_amount")]
    public int PayedAmount { get; set; }

    [Column("arrival_date")]
    public DateOnly ArrivalDate { get; set; }

    [Column("check_out_date")]
    public DateOnly CheckOutDate { get; set; }

    [Column("money_to_pay")]
    public int MoneyToPay { get; set; }

    [Column("has_arrived")]
    public bool HasArrived { get; set; }

    [Column("has_checked_out")]
    public bool HasCheckedOut { get; set; }

    [NotMapped]
    public virtual Room BookedRoom { get; set; } = null!;
}

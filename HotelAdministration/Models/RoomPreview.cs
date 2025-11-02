using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HotelAdministration.Models
{
    public class RoomPreview
    {
        public string Type { get; set; }
        public int Capacity { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        required public string PicSource { get; set; }
    }
}

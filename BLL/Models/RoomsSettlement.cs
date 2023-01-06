using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RoomsSettlement
    {
        public DateTime Date { get; set; }
        public string RoomId { get; set; }
        public string BookId { get; set; }
        public RoomCategory Category { get; set; }
        public bool IsSettlement { get; set; }
        public bool? IsPaymentComplete { get; set; }
    }
}

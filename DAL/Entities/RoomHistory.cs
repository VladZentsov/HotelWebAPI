using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class RoomHistory: BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string? RoomId { get; set; }
        public Room? Room { get; set; }
        public bool IsSettledInRoom { get; set; }


    }
}

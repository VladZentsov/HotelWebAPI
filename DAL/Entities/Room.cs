using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Room: BaseEntity
    {
        public RoomCategory Category { get; set; }
        public decimal Price { get; set; }
        public int VisitorsNumber { get; set; }
        public string? Description { get; set; }
        public ICollection<RoomHistory>? Histories { get; set; }
        public string? ImgName { get; set; }
        public ICollection<RoomImages>? RoomImages { get; set; }

    }
}

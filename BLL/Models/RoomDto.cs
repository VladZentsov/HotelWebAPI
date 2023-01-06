using BLL.ModelInterfaces;
using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RoomDto:BaseDto, IRoom
    {
        public RoomCategory Category { get; set; }
        public decimal Price { get; set; }
        public int VisitorsNumber { get; set; }
        public string? Description { get; set; }
        public string? imgName { get; set; }
        public DateTime? FirstDateForSettelment { get; set; }
    }
}

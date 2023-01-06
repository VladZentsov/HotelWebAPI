using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RoomHistoryFullInfo: RoomHistoryDto
    {
        public Room? Room { get; set; }
        public Customer? Customer { get; set; }
    }
}

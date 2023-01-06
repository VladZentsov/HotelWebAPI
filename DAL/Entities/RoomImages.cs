using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class RoomImages: BaseEntity
    {
        public string? RoomId { get; set; }
        public string? ImgName { get; set; }
    }
}

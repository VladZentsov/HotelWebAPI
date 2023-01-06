using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RoomImagesDto:BaseDto
    {
        public string RoomId { get; set; }
        public string? ImgName { get; set; }
    }
}

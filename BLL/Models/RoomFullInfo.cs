﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RoomFullInfo:RoomWithImages
    {
        public ICollection<(BookDto, CustomerDto)> BooksAndCustomersInfo { get; set; }
    }
}

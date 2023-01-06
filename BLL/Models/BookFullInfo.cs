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
    public class BookFullInfo : BookDto, ICustomer
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public RoomCategory? Category { get; set; }
        public int VisitorsNumber { get; set; }
        public string? Description { get; set; }
        public string? imgName { get; set; }
        public string Telnum { get; set; }
    }
}

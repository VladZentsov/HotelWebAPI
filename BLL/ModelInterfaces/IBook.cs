using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelInterfaces
{
    public interface IBook
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Price { get; set; }
        public bool? IsPaymentComplete { get; set; }
        public string? CustomerId { get; set; }
        public string? RoomId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CustomerFullInfo:CustomerDto
    {
        public ICollection<BookSimpleInfo>? Books { get; set; }
    }
}


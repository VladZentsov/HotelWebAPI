using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BaseDtoHelper
    {
        public static async Task<string> GetNextId(IEnumerable<string> ids)
        {
            if (ids == null || ids.Count() == 0)
                return "1";
            return (Convert.ToInt32(ids.Max(x => Convert.ToInt32(x))) + 1).ToString();
        }
    }
}

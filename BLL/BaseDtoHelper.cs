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
        public static async Task<string> GetNextId(IEnumerable<BaseEntity> baseEntities)
        {
            return (Convert.ToInt32(baseEntities.Max(x => Convert.ToInt32(x.Id))) + 1).ToString();
        }
    }
}

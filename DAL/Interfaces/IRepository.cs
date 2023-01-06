using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
        Task<TEntity> GetByIdWithDetailsAsync(string id);

        Task DeleteByIdAsync(string id);
    }
}

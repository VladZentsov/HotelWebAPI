using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(string id);

        Task AddAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        Task DeleteAsync(string modelId);
    }
}

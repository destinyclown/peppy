using Peppy.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.WebApi.Repositories
{
    public interface IPersonPepository : IRepositoryBase<Person, int>
    {
        Task<List<Person>> QueryListAsync();
    }
}
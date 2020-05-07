using Peppy.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.WebApi.Repositories
{
    public interface IPersonPepository : IRepositoryBase<AppDbContext, Person, int>
    {
        Task<List<Person>> QueryListAsync();
    }
}
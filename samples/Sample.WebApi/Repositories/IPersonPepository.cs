using Peppy.EntityFrameworkCore;

namespace Sample.WebApi.Repositories
{
    public interface IPersonPepository : IRepositoryBase<Person, int>
    {
    }
}
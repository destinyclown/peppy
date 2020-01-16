using Peppy.EntityFrameworkCore;
using Peppy.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.WebApi.Repositories
{
    public class PersonPepository : RepositoryBase<AppDbContext, Person, int>, IPersonPepository
    {
        public PersonPepository(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
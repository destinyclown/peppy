using Peppy.SqlSugarCore;
using Peppy.SqlSugarCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.WebApi.Repositories
{
    public class PersonPepository : RepositoryBase<Person, int>, IPersonPepository
    {
        public PersonPepository(IDbContextProvider dbContextProvider)
            : base(dbContextProvider)
        {
        }

        async Task<List<Person>> IPersonPepository.QueryListAsync()
        {
            return await QueryListAsync();
        }
    }
}
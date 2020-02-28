using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sample.WebApi.Repositories;

namespace Sample.WebApi.Handlers
{
    public class StudentHandler : IRequestHandler<Person, Person>, IEventHandler
    {
        private readonly IPersonPepository _personPepository;

        public StudentHandler(IPersonPepository personPepository)
        {
            _personPepository = personPepository;
        }

        public void Dispose()
        {
        }

        public async Task<Person> Handle(Person request, CancellationToken cancellationToken)
        {
            //Console.WriteLine("Student handle start");
            var result = await _personPepository.InsertAsync(request);
            return result;
        }
    }
}
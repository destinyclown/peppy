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
    public class StudentHandler : IRequestHandler<Student, string>, IEventHandler
    {
        private readonly IPersonPepository _personPepository;

        public StudentHandler(IPersonPepository personPepository)
        {
            _personPepository = personPepository;
        }

        public void Dispose()
        {
        }

        public async Task<string> Handle(Student request, CancellationToken cancellationToken)
        {
            //Console.WriteLine("Student handle start");
            //Console.WriteLine("2");
            await Task.Delay(1000, cancellationToken);
            return "test";
            //var result = await _personPepository.InsertAsync(request);
            //return new Person { Id = 2, Name = "test" };
        }
    }
}
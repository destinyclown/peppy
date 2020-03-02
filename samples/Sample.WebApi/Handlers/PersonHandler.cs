using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sample.WebApi.Repositories;

namespace Sample.WebApi.Handlers
{
    public class PersonHandler : INotificationHandler<Person>, IEventHandler
    {
        private readonly IPersonPepository _personPepository;

        public PersonHandler(IPersonPepository personPepository)
        {
            _personPepository = personPepository;
        }

        public void Dispose()
        {
        }

        public async Task Handle(Person request, CancellationToken cancellationToken)
        {
            Console.WriteLine("1");
            await Task.Delay(5000, cancellationToken);
            //var result = await _personPepository.InsertAsync(request);
            //return new Person { Id = 1, Name = "test" };
        }
    }
}
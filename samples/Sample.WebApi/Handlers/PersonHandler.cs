﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sample.WebApi.Repositories;

namespace Sample.WebApi.Handlers
{
    public class PersonHandler : IRequestHandler<Person, Person>, IEventHandler
    {
        private readonly IPersonPepository _personPepository;

        public PersonHandler(IPersonPepository personPepository)
        {
            _personPepository = personPepository;
        }

        public void Dispose()
        {
        }

        public async Task<Person> Handle(Person request, CancellationToken cancellationToken)
        {
            //Console.WriteLine("1");
            //await Task.Delay(5000, cancellationToken);
            var persons = await _personPepository.QueryListAsync();
            var person = persons.FirstOrDefault();
            person.Name = "test";
            var result = await _personPepository.UpdateAsync(person);
            return person;
        }
    }
}
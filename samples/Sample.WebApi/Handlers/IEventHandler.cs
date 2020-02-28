using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sample.WebApi.Handlers
{
    internal interface IEventHandler : IDisposable
    {
    }
}
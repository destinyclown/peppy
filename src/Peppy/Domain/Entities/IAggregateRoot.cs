using Peppy.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Domain.Entities
{
    /// <summary>
    /// Defines interface for aggregation root base type. All aggregation root in the system must implement this interface.
    /// </summary>
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {
    }

    /// <summary>
    /// Defines interface for aggregation root base type. All aggregation root in the system must implement this interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {
    }

    /// <summary>
    /// Generates domain events
    /// </summary>
    public interface IGeneratesDomainEvents
    {
        /// <summary>
        /// Domain events
        /// </summary>
        ICollection<IEventData> DomainEvents { get; }
    }
}
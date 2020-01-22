using Peppy.Events.Bus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Peppy.Domain.Entities
{
    /// <summary>
    /// Basic implementation of IAggregateRoot interface. An aggregate root can inherit this class of directly implement to IAggregateRoot interface.
    /// </summary>
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    /// <summary>
    /// Basic implementation of IAggregateRoot interface. An aggregate root can inherit this class of directly implement to IAggregateRoot interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        /// <summary>
        ///
        /// </summary>
        [NotMapped]
        public virtual ICollection<IEventData> DomainEvents { get; }

        /// <summary>
        ///
        /// </summary>
        public AggregateRoot()
        {
            DomainEvents = new Collection<IEventData>();
        }
    }
}
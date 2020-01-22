using Peppy.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore.Entities
{
    /// <summary>
    /// Basic implementation of IEntity interface. An entity can inherit this class of directly implement to IEntity interface.(SqlSugar)
    /// </summary>
    public class SqlSugarEntity : Entity
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public override int Id { get; set; }
    }

    /// <summary>
    /// Basic implementation of IEntity interface. An entity can inherit this class of directly implement to IEntity interface.(SqlSugar)
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class SqlSugarEntity<TPrimaryKey> : Entity<TPrimaryKey>
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public override TPrimaryKey Id { get; set; }
    }
}
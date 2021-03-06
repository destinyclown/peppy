﻿using Peppy.Dependency;
using Peppy.Domain.Entities;
using Peppy.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore.Repositories
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>, IScopedDependency
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}
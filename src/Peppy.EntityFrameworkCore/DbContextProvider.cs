﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EntityFrameworkCore
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : EFCroeDbContext
    {
        private readonly TDbContext _dbContext;

        public DbContextProvider(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
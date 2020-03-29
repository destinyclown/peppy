using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Peppy.EntityFrameworkCore
{
    public class EFCroeDbContext : DbContext
    {
        public EFCroeDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
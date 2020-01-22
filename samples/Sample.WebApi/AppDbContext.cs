using Microsoft.EntityFrameworkCore;
using Peppy.Domain.Entities;
using Peppy.SqlSugarCore.Entities;
using SqlSugar;

namespace Sample.WebApi
{
    [SugarTable("Persons")]
    public class Person : SqlSugarEntity<int>
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Id:{Id}";
        }
    }

    public class Person2
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Id:{Id}";
        }
    }

    public class AppDbContext : DbContext
    {
        public const string ConnectionString = "Server=192.168.6.45;Database=captest;UserId=root;Password=#7kfnymAM$Y9-Ntf;port=3306;Convert Zero Datetime=True;allowPublicKeyRetrieval=true";

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
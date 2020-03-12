using MediatR;
using Microsoft.EntityFrameworkCore;
using Peppy.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.WebApi
{
    //[SugarTable("Persons")]
    [Table("Persons")]
    public class Person : Entity<int>, IRequest<Person>
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Id:{Id}";
        }
    }

    public class PersonDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Student : IRequest<string>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Grade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public decimal Fraction { get; set; }
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
            //optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
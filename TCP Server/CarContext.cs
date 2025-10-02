using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
{
    internal class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public CarContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            Cars!.AddRange(
                new Car { Brand = "Toyota", Model = "Camry", Year = 2020 },
                new Car { Brand = "BMW", Model = "X5", Year = 2021 },
                new Car { Brand = "Audi", Model = "A6", Year = 2019 }
            );
            SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Cars;Integrated Security=True;Trust Server Certificate=True;");
        }
    }
}

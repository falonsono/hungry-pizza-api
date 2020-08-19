using Microsoft.EntityFrameworkCore;
using HungryPizza.API.Models;

namespace HungryPizza.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Flavour> Flavours { get; set; }
        public DbSet<PizzaFlavours> PizzaFlavours { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaFlavours>(entity =>
            {
                entity.HasKey(e => new { e.PizzaId, e.FlavourId });
            });
        }
    }
}

using HungryPizza.API.Data;
using HungryPizza.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HungryPizza.API.UnitTests
{
    public static class DbContextExtensions
    {
        public static void Populate(this DataContext dbContext)
        {
            dbContext.Flavours.Add(new Flavour
            {
                Name = "3 Queijos",
                Price = 50
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Frango com requeijão",
                Price = decimal.Parse("59.99")
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Mussarela",
                Price = decimal.Parse("42.5")
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Calabresa",
                Price = decimal.Parse("42.5")
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Pepperoni",
                Price = 55
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Portuguesa",
                Price = 45
            });

            dbContext.Flavours.Add(new Flavour
            {
                Name = "Veggie",
                Price = decimal.Parse("59.99")
            });

            dbContext.Clients.Add(new Client
            {
                Name =  "John Doe",
                CEP = "88000-100",
                Address = "Madre Benvenuta 123",
                PhoneNumber = "+5548997687373"
            });

            dbContext.Clients.Add(new Client
            {
                Name = "Jane Doe",
                CEP = "88000-100",
                Address = "Madre Benvenuta 123",
                PhoneNumber = "+5548997899393"
            });

            dbContext.SaveChanges();
        }
    }
}

using HungryPizza.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HungryPizza.API.UnitTests
{
    public class DataContextMocker
    {
        public static DataContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new DataContext(options);

            // Populate In Memory DB
            dbContext.Populate();

            return dbContext;
        }
    }
}

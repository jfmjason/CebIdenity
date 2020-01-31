using CI.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;

namespace CI.Infra.Data.Migrations.DesignTimeFactory
{
   internal class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        //for migration
        public AppDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            var constring = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var logger =  new Logger<AppDbContext>(new NullLoggerFactory());

            builder.UseSqlServer(constring
                , x => x.MigrationsHistoryTable(Constants.MIGRATIONHISTORYTABLENAME));

            return new AppDbContext(builder.Options, logger);
        }
    }
}


using CI.Core.Domain.Common;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CI.Infra.Data.Migrations.DesignTimeFactory
{
    internal class AppConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppConfigurationDbContext>
    {
        //for migration
        public AppConfigurationDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            var constring = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();

            builder.UseSqlServer(constring
                , x => x.MigrationsHistoryTable(Constants.MIGRATIONHISTORYTABLENAME));


            return new AppConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }
}

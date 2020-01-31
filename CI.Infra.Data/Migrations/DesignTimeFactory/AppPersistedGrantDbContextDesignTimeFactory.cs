

using CI.Core.Domain.Common;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CI.Infra.Data.Migrations.DesignTimeFactory
{
   internal class AppPersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppPersistedGrantDbContext>
    {
        //for migration
        public AppPersistedGrantDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            var constring = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();

            builder.UseSqlServer(constring
                , x => x.MigrationsHistoryTable(Constants.MIGRATIONHISTORYTABLENAME));


            return new AppPersistedGrantDbContext(builder.Options, new OperationalStoreOptions());
        }
    }
}

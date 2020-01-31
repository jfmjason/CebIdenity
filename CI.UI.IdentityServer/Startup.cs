using AutoMapper;
using CI.Core.Domain.Common;
using CI.Core.Domain.Models.Identity;
using CI.Core.Interface;
using CI.Infra.Data;
using CI.Infra.Data.Imp;
using CI.IServices;
using CI.Services;
using CI.UI.IdentityServer.AutoMapperProfile;
using CI.UI.IdentityServer.IdentityServer.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Reflection;

namespace CI.UI.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString , 
                x => x.MigrationsHistoryTable(Constants.MIGRATIONHISTORYTABLENAME)));


            services.AddIdentity<User, Role>()
                .AddCustomIdentityStores()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IdentityMapperProfile());
                mc.AddProfile(new ApiResourceMapperProfile());
                mc.AddProfile(new ClientMapperProfile());
                mc.AddProfile(new IdentityMapperProfile());
                mc.AddProfile(new IdentityResourceMapperProfile());
                mc.AddProfile(new PersistedGrantMapperProfile());

            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //logger singleton
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddClientStore<ClientStore>()
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddDeviceFlowStore<DeviceFlowStore>()
                .AddResourceStore<ResourceStore>()
                .AddCorsPolicyService<CorsPolicyService>()
                .AddAspNetIdentity<User>();




            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
            app.UseSerilogRequestLogging();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //update pending migrations 
                scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

                SeedData.EnsureSeedData(scope);
            }
        }

    }
}
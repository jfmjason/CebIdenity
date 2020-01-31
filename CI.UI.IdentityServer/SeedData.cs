using AutoMapper;
using CI.Core.Domain.Models.Identity;
using CI.Core.Domain.Models.IdentityServerConfiguration;
using CI.Infra.Data;
using CI.IServices;
using IdentityModel;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Linq;
using System.Security.Claims;
using is4Models = IdentityServer4.Models;
using is4Entity = CI.Core.Domain.Models.IdentityServerPersistedGrant;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceScope scope)
        {

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var clientService = scope.ServiceProvider.GetRequiredService<IService<Client>>();

            var identityResourceService = scope.ServiceProvider.GetRequiredService<IService<IdentityResource>>();

            var apiResourceService = scope.ServiceProvider.GetRequiredService<IService<ApiResource>>();

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            logger.LogInformation("TEST LOG");

            var alice = userMgr.FindByNameAsync("alice").Result;

                if (alice == null)
                {
                    alice = new User
                    {
                        UserName = "alice"
                    };
                    var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(alice, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Console.WriteLine("alice created");
                }
                else
                {
                    Console.WriteLine("alice already exists");
                }

                var bob = userMgr.FindByNameAsync("bob").Result;
                if (bob == null)
                {
                    bob = new User
                    {
                        UserName = "bob"
                    };
                    var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(bob, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Console.WriteLine("bob created");
                }
                else
                {
                    Console.WriteLine("bob already exists");
                }



            if (clientService.Count().GetAwaiter().GetResult() == 0)
            {
                clientService.Add(mapper.Map<List<Client>>(Config.GetClients()));
            }

            if (identityResourceService.Count().GetAwaiter().GetResult() > 0)
            {
               identityResourceService.Add(mapper.Map<List<IdentityResource>>(Config.GetIdentityResources()));
            }

            if (apiResourceService.Count().GetAwaiter().GetResult() == 0)
            {
                apiResourceService.Add(mapper.Map<List<ApiResource>>(Config.GetApis()));
            }

        }
    }
}


using CI.Core.Domain.Models.Identity;
using CI.UI.IdentityServer.Identity.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CI.UI.IdentityServer
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddCustomIdentityStores(this IdentityBuilder builder)
        {
            builder.Services.AddTransient<IUserStore<User>, UserStore>();
            builder.Services.AddTransient<IRoleStore<Role>, RoleStore>();
            return builder;
        }
    }
}

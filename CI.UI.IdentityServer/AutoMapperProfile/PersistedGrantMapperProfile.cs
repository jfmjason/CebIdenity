using AutoMapper;
using Entities = CI.Core.Domain.Models.IdentityServerPersistedGrant;
using Models = IdentityServer4.Models;

namespace CI.UI.IdentityServer.AutoMapperProfile
{
    public class PersistedGrantMapperProfile :Profile
    {
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrant, Models.PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}

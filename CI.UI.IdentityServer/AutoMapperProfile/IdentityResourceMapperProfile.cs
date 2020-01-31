using AutoMapper;
using System.Collections.Generic;
using Entities = CI.Core.Domain.Models.IdentityServerConfiguration;
using Models = IdentityServer4.Models;

namespace CI.UI.IdentityServer.AutoMapperProfile
{
    public class IdentityResourceMapperProfile : Profile
    {
        public IdentityResourceMapperProfile()
        {
            CreateMap<Entities.IdentityProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.IdentityResource, Models.IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new Models.IdentityResource())
                .ReverseMap();

            CreateMap<Entities.IdentityClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}

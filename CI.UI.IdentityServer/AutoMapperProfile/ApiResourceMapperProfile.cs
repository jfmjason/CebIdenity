using AutoMapper;
using System.Collections.Generic;
using Entities = CI.Core.Domain.Models.IdentityServerConfiguration;
using Models = IdentityServer4.Models;

namespace CI.UI.IdentityServer.AutoMapperProfile
{
    public class ApiResourceMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="ApiResourceMapperProfile"/>
        /// </summary>
        public ApiResourceMapperProfile()
        {
            CreateMap<Entities.ApiProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.ApiResource, Models.ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new Models.ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.ApiSecrets))
                .ReverseMap();

            CreateMap<Entities.ApiClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ApiSecret, Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<Entities.ApiScope, Models.Scope>(MemberList.Destination)
                .ConstructUsing(src => new Models.Scope())
                .ReverseMap();

            CreateMap<Entities.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}

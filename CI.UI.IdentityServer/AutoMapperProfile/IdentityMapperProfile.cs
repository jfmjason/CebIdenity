using AutoMapper;
using CI.Core.Domain.Models.Identity;
using CI.Core.Domain.Models.IdentityServerConfiguration;
using CI.UI.IdentityServer.DTO;

namespace CI.UI.IdentityServer.AutoMapperProfile
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            //CreateMap<User, User>();
            //CreateMap<User, User>();

            //CreateMap<Role, ApplicationRole>();
            //CreateMap<ApplicationRole, Role>();
            CreateMap<Client, ClientViewItemDTO>().ReverseMap();

        }
    }
}
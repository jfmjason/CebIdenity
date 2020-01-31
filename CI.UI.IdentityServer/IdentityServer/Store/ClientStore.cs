

using System.Threading.Tasks;
using CI.IServices;
using is4Models = IdentityServer4.Models;
using is4Entity = CI.Core.Domain.Models.IdentityServerConfiguration;
using IdentityServer4.Stores;
using AutoMapper;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer.IdentityServer.Store
{
    public class ClientStore : IClientStore
    {
        private IService<is4Entity.Client> _clientService;
        private IMapper _mapper;
        private ILogger<ClientStore> _logger;

        public ClientStore(IService<is4Entity.Client> clientService, IMapper mapper, ILogger<ClientStore>  logger) {
            _clientService = clientService;
            _mapper = mapper;
            _logger = logger;
        }

        public  Task<is4Models.Client> FindClientByIdAsync(string clientId)
        {
          
            var entity = _clientService.GetSingle(c => c.ClientId == clientId);

            var model = entity != null? _mapper.Map<is4Models.Client>(entity): new is4Models.Client();

            _logger.LogDebug("{0} found in database: {1}", clientId, !string.IsNullOrEmpty(model.ClientId));

            return Task.FromResult(model);
        }
    }
}

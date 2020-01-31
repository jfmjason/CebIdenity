using CI.Core.Domain.Models.IdentityServerConfiguration;
using IdentityServer4.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using CI.IServices;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer.IdentityServer.Store
{
    public class CorsPolicyService : ICorsPolicyService
    {

        private readonly IService<Client> _clientService;
        private readonly ILogger<CorsPolicyService> _logger;

        public CorsPolicyService(
            IService<Client> clientService,
            ILogger<CorsPolicyService> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var allowedCorsOriginCount = await _clientService.Count(client => client.ClientCorsOrigins.Any(x=> string.Compare(x.Origin, origin, StringComparison.OrdinalIgnoreCase) >= 0));

            var result = allowedCorsOriginCount > 0;
            var allowText = result ? "Allowing" : "Not allowing";

            _logger.LogInformation("{0} {1}", allowText, origin);

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using is4Models = IdentityServer4.Models;
using is4Entity = CI.Core.Domain.Models.IdentityServerConfiguration;
using IdentityServer4.Stores;
using Serilog;
using CI.IServices;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer.IdentityServer.Store
{
    public class ResourceStore : IResourceStore
    {
        private readonly ILogger<ResourceStore> _logger;
        private readonly IService<is4Entity.ApiResource> _apiResourceService;
        private readonly IService<is4Entity.IdentityResource> _identityResourceService;
        private readonly IMapper _mapper;



        public ResourceStore(IService<is4Entity.ApiResource> apiResourceService, IService<is4Entity.IdentityResource> identityResourceService, ILogger<ResourceStore> logger, IMapper mapper)
        {
            _apiResourceService = apiResourceService;
            _identityResourceService = identityResourceService;
            _logger = logger;
            _mapper = mapper;
        }


        public Task<is4Models.ApiResource> FindApiResourceAsync(string name)
        {
            is4Models.ApiResource apiResource = new is4Models.ApiResource();

            var resourceEntity = _apiResourceService.GetSingle(i => i.Name == name);

            if (resourceEntity != null)
            {
                _logger.LogDebug("Found {api} API resource in database", name);
                apiResource = _mapper.Map<is4Models.ApiResource>(resourceEntity);
            }
            else
            {
                _logger.LogDebug("Did not find {api} API resource in database", name);
            }

            return Task.FromResult(apiResource);
        }

        public Task<IEnumerable<is4Models.ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            List<is4Models.ApiResource> apiResources = new List<is4Models.ApiResource>();

            var names = scopeNames.ToArray();

            var entities = _apiResourceService.GetAll(x => x.ApiScopes.Any(s=>s.Name.Contains(x.Name)));

            if (entities.Count() > 0) {

                apiResources = _mapper.Map<List<is4Models.ApiResource>>(entities);
            }

            _logger.LogDebug("Found {scopes} API scopes in database", apiResources.SelectMany(x => x.Scopes).Select(x => x.Name));

            return Task.FromResult(apiResources.AsEnumerable());
        }

        public Task<IEnumerable<is4Models.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            List<is4Models.IdentityResource> identityResources = new List<is4Models.IdentityResource>();

            var names = scopeNames.ToArray();

            var entities = _identityResourceService.GetAll(x => x.Name.Contains(x.Name));

            if (entities.Count() > 0)
            {
                identityResources = _mapper.Map<List<is4Models.IdentityResource>>(entities);
            }

            _logger.LogDebug("Found {scopes} identity scopes in database", identityResources.Select(x => x.Name));

            return Task.FromResult(identityResources.AsEnumerable());
        }


        public Task<is4Models.Resources> GetAllResourcesAsync()
        {
            var identity = _identityResourceService.GetAll();

            var apis = _apiResourceService.GetAll();

            var result = new is4Models.Resources(
                _mapper.Map<List<is4Models.IdentityResource>>(identity),
                _mapper.Map<List<is4Models.ApiResource>>(apis));

            _logger.LogDebug("Found {scopes} as all scopes in database", result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name)));

            return Task.FromResult(result);
        }
    }
}

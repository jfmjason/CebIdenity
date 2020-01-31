using AutoMapper;
using CI.IServices;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using is4Models = IdentityServer4.Models;
using is4Entity = CI.Core.Domain.Models.IdentityServerPersistedGrant;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer.IdentityServer.Store
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IService<is4Entity.PersistedGrant> _persistedGrantService;
        private readonly ILogger<PersistedGrantStore> _logger;
        private readonly IMapper _mapper;

        public PersistedGrantStore(IService<is4Entity.PersistedGrant> persistedGrantService, ILogger<PersistedGrantStore> logger, IMapper mapper)
        {
            _persistedGrantService = persistedGrantService;
            _logger = logger;
            _mapper = mapper;
        }

        public Task StoreAsync(is4Models.PersistedGrant token)
        {
            var existing = _persistedGrantService.GetSingle(x => x.Key == token.Key);

            try
            {

                if (existing == null)
                {
                    _logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                    var persistedGrant = _mapper.Map<is4Entity.PersistedGrant>(token);
                    _persistedGrantService.Add(persistedGrant);
                }
                else
                {
                    _logger.LogDebug("{persistedGrantKey} found in database", token.Key);

                    _persistedGrantService.Update(existing);
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("exception updating {persistedGrantKey} persisted grant in database: {error}", token.Key, ex.Message);
            }


            return Task.FromResult(0);
        }

        public Task<is4Models.PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = _persistedGrantService.GetSingle(x => x.Key == key);

            var model = persistedGrant != null ? _mapper.Map<is4Models.PersistedGrant>(persistedGrant) : null;

            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

            return Task.FromResult(model);
        }

        public Task<IEnumerable<is4Models.PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = _persistedGrantService.GetAll(x => x.SubjectId == subjectId);

            var model = _mapper.Map<List<is4Models.PersistedGrant>>(persistedGrants);

            _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", persistedGrants.Count(), subjectId);

            return Task.FromResult(model.AsEnumerable());
        }

        public Task RemoveAsync(string key)
        {
            var persistedGrant = _persistedGrantService.GetSingle(x => x.Key == key);

            try
            {
                if (persistedGrant != null)
                {
                    _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

                    _persistedGrantService.Delete(persistedGrant);
                }
                else
                {
                    _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {error}", key, ex.Message);
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            var persistedGrants = _persistedGrantService.GetAll(x => x.SubjectId == subjectId && x.ClientId == clientId)?.ToList();

            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}", persistedGrants.Count, subjectId, clientId);

            try
            {
                _persistedGrantService.Delete(persistedGrants);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}: {error}", persistedGrants.Count, subjectId, clientId, ex.Message);
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var persistedGrants = _persistedGrantService.GetAll(x =>
                x.SubjectId == subjectId &&
                x.ClientId == clientId &&
                x.Type == type).ToList();

            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}", persistedGrants.Count, subjectId, clientId, type);

            try
            {
                _persistedGrantService.Delete(persistedGrants);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("exception removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}: {error}", persistedGrants.Count, subjectId, clientId, type, ex.Message);
            }

            return Task.FromResult(0);
        }
    }
}

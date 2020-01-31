using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Serilog;
using System;
using System.Threading.Tasks;
using is4Models = IdentityServer4.Models;
using is4Entity = CI.Core.Domain.Models.IdentityServerPersistedGrant;
using CI.IServices;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CI.UI.IdentityServer.IdentityServer.Store
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly IService<is4Entity.DeviceFlowCode> _deviceFlowCodeService;
        private readonly IPersistentGrantSerializer _serializer;
        private readonly ILogger<DeviceFlowStore> _logger;


        public DeviceFlowStore(
            IService<is4Entity.DeviceFlowCode> deviceFlowCodeService,
            IPersistentGrantSerializer serializer,
            ILogger<DeviceFlowStore>  logger)
        {
            _deviceFlowCodeService = deviceFlowCodeService;
            _serializer = serializer;
            _logger = logger;
        }

        public Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, is4Models.DeviceCode data)
        {
            _deviceFlowCodeService.Add(ToEntity(data, deviceCode, userCode));

            return Task.FromResult(0);
        }

        public Task<is4Models.DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var deviceFlowCodes = _deviceFlowCodeService.GetSingle(x => x.UserCode == userCode);
            var model = ToModel(deviceFlowCodes?.Data);

            _logger.LogDebug("{userCode} found in database: {userCodeFound}", userCode, model != null);

            return Task.FromResult(model);
        }

        public Task<is4Models.DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = _deviceFlowCodeService.GetSingle(x => x.DeviceCode == deviceCode);

            var model = ToModel(deviceFlowCodes?.Data);

            _logger.LogDebug("{deviceCode} found in database: {deviceCodeFound}", deviceCode, model != null);

            return Task.FromResult(model);
        }

        public Task UpdateByUserCodeAsync(string userCode, is4Models.DeviceCode data)
        {
            var existing = _deviceFlowCodeService.GetSingle(x => x.UserCode == userCode);
            if (existing == null)
            {
                _logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var entity = ToEntity(data, existing.DeviceCode, userCode);
            _logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            try
            {
                _deviceFlowCodeService.Update(entity);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("exception updating {userCode} user code in database: {error}", userCode, ex.Message);
            }

            return Task.FromResult(0);
        }

        public Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = _deviceFlowCodeService.GetSingle(x => x.DeviceCode == deviceCode);

            if (deviceFlowCodes != null)
            {
                _logger.LogDebug("removing {deviceCode} device code from database", deviceCode);

                try
                {
                    _deviceFlowCodeService.Delete(deviceFlowCodes);

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogInformation("exception removing {deviceCode} device code from database: {error}", deviceCode, ex.Message);
                }
            }
            else
            {
                _logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }

            return Task.FromResult(0);
        }

        private is4Entity.DeviceFlowCode ToEntity(is4Models.DeviceCode model, string deviceCode, string userCode)
        {
            if (model == null || deviceCode == null || userCode == null) return null;

            return new is4Entity.DeviceFlowCode
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = _serializer.Serialize(model)
            };
        }

        private is4Models.DeviceCode ToModel(string entity)
        {
            if (entity == null) return null;

            return _serializer.Deserialize<is4Models.DeviceCode>(entity);
        }
    }
}

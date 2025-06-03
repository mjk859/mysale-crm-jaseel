using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MySaleApp.Admin.UI.Services
{
    public class RedisTicketStore : ITicketStore
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisTicketStore> _logger;

        public RedisTicketStore(IDistributedCache cache, ILogger<RedisTicketStore> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task RemoveAsync(string key)
        {
            _logger.LogInformation("Removing auth ticket with key: {Key}", key);
            await _cache.RemoveAsync(key);
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _logger.LogInformation("Renewing auth ticket with key: {Key}", key);
            var data = Serialize(ticket);
            await _cache.SetAsync(key, data);
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            _logger.LogInformation("Retrieving auth ticket with key: {Key}", key);
            var data = await _cache.GetAsync(key);
            if (data == null)
            {
                _logger.LogWarning("No auth ticket found in Redis for key: {Key}", key);
                return null;
            }
            _logger.LogInformation("Auth ticket found in Redis for key: {Key}", key);
            return Deserialize(data);
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = $"auth-ticket-{Guid.NewGuid()}";
            _logger.LogInformation("Storing new auth ticket with generated key: {Key}", key);
            await RenewAsync(key, ticket);
            return key;
        }

        private static byte[] Serialize(AuthenticationTicket source)
            => TicketSerializer.Default.Serialize(source);

        private static AuthenticationTicket Deserialize(byte[] source)
            => TicketSerializer.Default.Deserialize(source);
    }
}

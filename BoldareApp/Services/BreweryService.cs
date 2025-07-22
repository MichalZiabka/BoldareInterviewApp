using BoldareApp.Dto;
using BoldareApp.Infrastructure.DbCache;
using BoldareApp.Infrastructure.Exceptions;
using BoldareApp.Models;
using BoldareApp.Queries;
using BoldareApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BoldareApp.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BreweryService> _logger;
        private readonly DbCacheContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string _cacheKey = "brewery_cache_key";
        private const string _breweryApiUrl = "https://api.openbrewerydb.org/v1/breweries";

        public BreweryService(
            HttpClient httpClient,
            IMemoryCache memoryCache,
            ILogger<BreweryService> logger, 
            DbCacheContext context)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
            _context = context;
        }
        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync()
        {
            if (_memoryCache.TryGetValue(_cacheKey, out IEnumerable<BreweryDto> cachedData))
            {
                _logger.LogInformation("Get data from in memory cache for query");
                return cachedData;
            }

            var mappedData = await ExecuteRequest(string.Empty);

            _memoryCache.Set(_cacheKey, mappedData, TimeSpan.FromMinutes(10));
            _logger.LogInformation("Set data to catche for query");

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync(QueryBase queryFilter)
        {
            var query = queryFilter.BuildQuery();
            var cacheKey = $"brewery_query_{query}";

            var cacheEntry = await _context.BreweryCache.FirstOrDefaultAsync(x => x.CacheKey == cacheKey);
            if (cacheEntry != null && cacheEntry.CachedAt > DateTime.UtcNow.AddMinutes(-10))
            {
                _logger.LogInformation("Get data from db cache for query: {Query}", query);
                return JsonSerializer.Deserialize<List<BreweryDto>>(cacheEntry.JsonData);
            }

            var mappedData = await ExecuteRequest(query);

            _context.BreweryCache.RemoveRange(_context.BreweryCache.Where(x => x.CacheKey == cacheKey));
            _context.BreweryCache.Add(new BreweryCacheModel
            {
                CacheKey = cacheKey,
                JsonData = JsonSerializer.Serialize(mappedData),
                CachedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            _logger.LogInformation("Set data db cache for query: {Query}", query);

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }

        private async Task<IEnumerable<BreweryDto>> ExecuteRequest(string query)
        {
            var uri = string.IsNullOrEmpty(query) ? _breweryApiUrl : $"{_breweryApiUrl}{query}";
            _logger.LogInformation("Requesting external API for query: {Uri}", uri);
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            IEnumerable<BreweryModel> data = null;
            try
            {
                data = JsonSerializer.Deserialize<IEnumerable<BreweryModel>>(responseBody);
            }
            catch (Exception)
            {
                throw new ExternalApiException();
            }

            var mappedData = data?.Select(x => new BreweryDto
            {
                Id = x.Id,
                Name = x.Name,
                City = x.City,
                PhoneNumber = x.PhoneNumber
            });

            return mappedData;
        }
    }
}

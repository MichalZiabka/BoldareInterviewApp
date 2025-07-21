using BoldareApp.Dto;
using BoldareApp.Models;
using BoldareApp.Services.Interfaces;
using BoldareApp.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BoldareApp.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private const string _cacheKey = "brewery_cache_key";
        private const string _breweryApiUrl = "https://api.openbrewerydb.org/v1/breweries";

        public BreweryService(
            HttpClient httpClient,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync()
        {
            if (_memoryCache.TryGetValue(_cacheKey, out IEnumerable<BreweryDto> cachedData))
            {
                return cachedData;
            }

            var uri = _breweryApiUrl;
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IEnumerable<BreweryModel>>(responseBody);

            var mappedData = data?.Select(x => new BreweryDto
            {
                Id = x.Id,
                Name = x.Name,
                City = x.City,
                PhoneNumber = x.PhoneNumber
            });

            _memoryCache.Set(_cacheKey, mappedData, TimeSpan.FromMinutes(10));

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync(QueryBase queryFilter)
        {
            var query = queryFilter.BuildQuery();
            var cacheKey = $"brewery_query_{query}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<BreweryDto> cachedData))
            {
                return cachedData;
            }

            var uri = string.IsNullOrEmpty(query) ? _breweryApiUrl : $"{_breweryApiUrl}{query}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IEnumerable<BreweryModel>>(responseBody);

            var mappedData = data?.Select(x => new BreweryDto
            {
                Id = x.Id,
                Name = x.Name,
                City = x.City,
                PhoneNumber = x.PhoneNumber
            });

            _memoryCache.Set(cacheKey, mappedData, TimeSpan.FromMinutes(1));

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }
    }
}

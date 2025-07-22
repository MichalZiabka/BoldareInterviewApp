using BoldareApp.Dto;
using BoldareApp.Infrastructure.Exceptions;
using BoldareApp.Models;
using BoldareApp.Queries;
using BoldareApp.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BoldareApp.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BreweryService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string _cacheKey = "brewery_cache_key";
        private const string _breweryApiUrl = "https://api.openbrewerydb.org/v1/breweries";

        public BreweryService(
            HttpClient httpClient,
            IMemoryCache memoryCache,
            ILogger<BreweryService> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
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
                _logger.LogInformation("Get data from catche for query: {Query}", query);
                return cachedData;
            }

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

            _memoryCache.Set(cacheKey, mappedData, TimeSpan.FromMinutes(10));
            _logger.LogInformation("Set data to catche for query: {Query}", query);

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }
    }
}

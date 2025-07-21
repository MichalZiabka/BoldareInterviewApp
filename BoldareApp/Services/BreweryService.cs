using BoldareApp.Controllers;
using BoldareApp.Dto;
using BoldareApp.Models;
using BoldareApp.Services.Interfaces;
using GeoCoordinatePortable;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BoldareApp.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private const string _memoryKey = "breweries";
        private const string _breweryApiUrl = "https://api.openbrewerydb.org/v1/breweries";

        public BreweryService(
            HttpClient httpClient,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync(Localization? localization)
        {
            if (_memoryCache.TryGetValue(_memoryKey, out IEnumerable<BreweryDto> cachedData))
            {
                return cachedData;
            }

            var response = await _httpClient.GetAsync(_breweryApiUrl);
            response.EnsureSuccessStatusCode(); 

            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IEnumerable<BreweryModel>>(responseBody);

            GeoCoordinate? userLocalication = null;
            if (localization != null)
            {
                userLocalication = new GeoCoordinate(localization.Latitude, localization.Longitude);
            }

            var mappedData = data?.Select(x => new BreweryDto
            {
                Id = x.Id,
                Name = x.Name,
                City = x.City,
                PhoneNumber = x.PhoneNumber,
                Distance = userLocalication != null && x.Latitude != null && x.Longitude != null ? userLocalication.GetDistanceTo(new GeoCoordinate(x.Latitude.Value, x.Longitude.Value)) : null
            });

            _memoryCache.Set(_memoryKey, mappedData, TimeSpan.FromMinutes(1));

            return mappedData ?? Enumerable.Empty<BreweryDto>();
        }
    }
}

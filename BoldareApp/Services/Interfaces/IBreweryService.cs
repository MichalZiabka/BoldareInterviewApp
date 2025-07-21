using BoldareApp.Dto;
using BoldareApp.Queries;

namespace BoldareApp.Services.Interfaces
{
    public interface IBreweryService
    {
        Task<IEnumerable<BreweryDto>> GetBreweriesAsync();

        Task<IEnumerable<BreweryDto>> GetBreweriesAsync(QueryBase queryFilter);
    }
}

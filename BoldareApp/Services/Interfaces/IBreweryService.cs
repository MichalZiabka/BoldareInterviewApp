using BoldareApp.Controllers;
using BoldareApp.Dto;

namespace BoldareApp.Services.Interfaces
{
    public interface IBreweryService
    {
        Task<IEnumerable<BreweryDto>> GetBreweriesAsync(Localization? localization = null);
    }
}

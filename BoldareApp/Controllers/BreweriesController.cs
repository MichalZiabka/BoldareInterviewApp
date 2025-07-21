using BoldareApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace BoldareApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/breweries")]
    public class BreweriesController : ControllerBase
    {
        private readonly ILogger<BreweriesController> _logger;
        private readonly IBreweryService _breweryService;

        public BreweriesController(
            ILogger<BreweriesController> logger,
            IBreweryService breweryService)
        {
            _logger = logger;
            _breweryService = breweryService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Returns a list of breweries from the database.")]
        public async Task<IActionResult> GetV1()
        {
            var data = await _breweryService.GetBreweriesAsync();
            return Ok(data);
        }

        [HttpGet]
        [EnableQuery]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Returns a list of breweries from the database.")]
        public async Task<IActionResult> GetV2([FromQuery] Localization? localization)
        {
            var data = await _breweryService.GetBreweriesAsync(localization);
            return Ok(data.AsQueryable());
        }
    }


    public class Localization
    {
        public int Latitude { get; set; }

        public int Longitude { get; set; }
    }
}


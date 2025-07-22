using BoldareApp.Queries;
using BoldareApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BoldareApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/breweries")]
    public class BreweriesController : ControllerBase
    {
        private readonly IBreweryService _breweryService;

        public BreweriesController(IBreweryService breweryService)
        {
            _breweryService = breweryService;
        }

        [HttpGet("filter")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Returns a list of breweries from source.")]
        public async Task<IActionResult> GetByFilterV1()
        {
            var data = await _breweryService.GetBreweriesAsync();
            return Ok(data);
        }

        [HttpGet("filter")]
        [Authorize]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Returns a list of breweries from source with filtering and pagination.")]
        public async Task<IActionResult> GetByFilterV2([FromQuery] FilterQuery query)
        {
            var data = await _breweryService.GetBreweriesAsync(query);
            return Ok(data);
        }

        [HttpGet("nearby")]
        [Authorize]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Gets a list of breweries by distance from an origin point.")]
        public async Task<IActionResult> GetByLocation([FromQuery] DistanceQuery query)
        {
            var data = await _breweryService.GetBreweriesAsync(query);
            return Ok(data);
        }

        [HttpGet("autocomplete")]
        [Authorize]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Gets a list of breweries by autocomplete search term.")]
        public async Task<IActionResult> GetByAutocomplete([FromQuery] AutocompleteQuery query)
        {
            var data = await _breweryService.GetBreweriesAsync(query);
            return Ok(data);
        }

        [HttpGet("search")]
        [Authorize]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets a list of breweries", Description = "Gets a list of breweries by search term.")]
        public async Task<IActionResult> GetByAutocomplete([FromQuery] SearchQuery query)
        {
            var data = await _breweryService.GetBreweriesAsync(query);
            return Ok(data);
        }
    }
}


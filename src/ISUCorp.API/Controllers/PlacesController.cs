using ISUCorp.API.Filters;
using ISUCorp.API.Resources;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ISUCorp.API.Controllers
{
    [Route("api/places")]
    [Produces("application/json")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        /// <summary>
        /// Gets a paginated list of places.
        /// </summary>
        /// <returns>Paginated list of places.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PlaceResource>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public IActionResult GetAllAsync([FromQuery] PlaceQueryResource queryResource)
        {
            var response = _placeService.ListAsync(queryResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets a place with the given identifier.
        /// </summary>
        /// <param name="id">Place's identifier.</param>
        /// <returns>PLace's details whether exists.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlaceResource), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _placeService.FindByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets a place with the given name.
        /// </summary>
        /// <param name="name">Name of the place.</param>
        /// <returns>Details of the place whether exists.</returns>
        [HttpGet("find_by_name/{name}")]
        [ProducesResponseType(typeof(PlaceResource), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var response = await _placeService.FindByNameAsync(name);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Adds a new place.
        /// </summary>
        /// <param name="savePlaceResource">Place information.</param>
        /// <returns>Whether the place was added.</returns>
        [HttpPost]
        [TypeFilter(typeof(ValidateSavePlaceAttribute))]
        [ProducesResponseType(typeof(PlaceResource), 201)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> PostAsync(
            [FromBody] SavePlaceResource savePlaceResource)
        {
            var response = await _placeService.AddAsync(savePlaceResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing place.
        /// </summary>
        /// <param name="id">Place identifier.</param>
        /// <param name="savePlaceResource">New place information.</param>
        /// <returns>Whether the place was updated.</returns>
        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateSavePlaceAttribute))]
        [ProducesResponseType(typeof(PlaceResource), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> PutAsync(int id,
            [FromBody] SavePlaceResource savePlaceResource)
        {
            var response = await _placeService.UpdateAsync(id, savePlaceResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Deletes an existing place.
        /// </summary>
        /// <param name="id">Place identifier.</param>
        /// <returns>Whether the place was deleted.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResource), 204)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _placeService.RemoveAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

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
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Gets a paginated list of reservations.
        /// </summary>
        /// <returns>Paginated list of reservations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ReservationResource>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public IActionResult GetAsync([FromQuery] ReservationQueryResource queryResource)
        {
            var response = _reservationService.ListAsync(queryResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets a reservation with the given identifier.
        /// </summary>
        /// <param name="id">Reservation's identifier.</param>
        /// <returns>Reservation's details whether exists.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationResource), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _reservationService.FindByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Adds a new reservation.
        /// </summary>
        /// <param name="saveReservationResource">Reservation information.</param>
        /// <returns>Whether the reservation was added.</returns>
        [HttpPost]
        [TypeFilter(typeof(ValidateSaveReservationAttribute))]
        [ProducesResponseType(typeof(ReservationResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync(
            [FromBody] SaveReservationResource saveReservationResource)
        {
            var response = await _reservationService.AddAsync(saveReservationResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Rates a reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <param name="rating">Rating.</param>
        /// <returns>Whether the reservation was rated.</returns>
        [HttpPost("{id}/rate/{rating}")]
        [ProducesResponseType(typeof(ReservationResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostRateAsync(int id, int rating)
        {
            var response = await _reservationService.RateAsync(id, rating);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Set a reservation as favorite.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <param name="favorite">Is favorite.</param>
        /// <returns>Whether was saved.</returns>
        [HttpPost("set_favorite/{id}")]
        [ProducesResponseType(typeof(ReservationResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostRateAsync(int id)
        {
            var response = await _reservationService.SetFavoriteAsync(id, true);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <param name="saveReservationResource">New reservation information.</param>
        /// <returns>Whether the reservation was updated.</returns>
        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateSaveReservationAttribute))]
        [ProducesResponseType(typeof(ReservationResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id,
            [FromBody] SaveReservationResource saveReservationResource)
        {
            var response = await _reservationService.UpdateAsync(id, saveReservationResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Deletes an existing reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>Whether the reservation was deleted.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResource), 204)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _reservationService.RemoveAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

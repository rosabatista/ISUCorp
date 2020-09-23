using ISUCorp.API.Filters;
using ISUCorp.API.Resources;
using ISUCorp.Core.Domain;
using ISUCorp.Services.Contracts.Responses;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ISUCorp.API.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Gets a paginated list of contacts.
        /// </summary>
        /// <returns>Paginated list of contacts.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ContactResource>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public IActionResult GetAsync([FromQuery] ContactQueryResource queryResource)
        {
            var response = _contactService.ListAsync(queryResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets a contact with the given identifier.
        /// </summary>
        /// <param name="id">Contact's identifier.</param>
        /// <returns>Contact's details whether exists.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResource), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _contactService.FindByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets a contact with the given identifier.
        /// </summary>
        /// <param name="name">Contact's name.</param>
        /// <returns>Contact's details whether exists.</returns>
        [HttpGet("find_by_name/{name}")]
        [ProducesResponseType(typeof(ContactResource), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var response = await _contactService.FindByNameAsync(name);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Adds a new contact.
        /// </summary>
        /// <param name="saveContactResource">Contact information.</param>
        /// <returns>Whether the contact was added.</returns>
        [HttpPost]
        [TypeFilter(typeof(ValidateSaveContactAttribute))]
        [ProducesResponseType(typeof(ContactResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync(
            [FromBody] SaveContactResource saveContactResource)
        {
            var response = await _contactService.AddAsync(saveContactResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="id">Contact identifier.</param>
        /// <param name="saveContactResource">New contact information.</param>
        /// <returns>Whether the contact was updated.</returns>
        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateSaveContactAttribute))]
        [ProducesResponseType(typeof(ContactResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, 
            [FromBody] SaveContactResource saveContactResource)
        {
            var response = await _contactService.UpdateAsync(id, saveContactResource);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Deletes an existing contact.
        /// </summary>
        /// <param name="id">Contact identifier.</param>
        /// <returns>Whether the contact was deleted.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResource), 204)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _contactService.RemoveAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

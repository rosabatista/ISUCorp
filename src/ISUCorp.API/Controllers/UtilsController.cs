using ISUCorp.Core.Domain;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISUCorp.API.Controllers
{
    [Route("api/utils")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IUtilService _utilService;

        public UtilsController(IUtilService utilService)
        {
            _utilService = utilService;
        }

        /// <summary>
        /// Gets a list of contact types.
        /// </summary>
        /// <returns>List of contact types.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<ContactTypeItem>>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public IActionResult GetAsync()
        {
            var response = _utilService.GetContactTypes();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

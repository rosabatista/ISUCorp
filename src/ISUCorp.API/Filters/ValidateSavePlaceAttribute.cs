using ISUCorp.API.Resources;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ISUCorp.API.Filters
{
    public class ValidateSavePlaceAttribute : IAsyncActionFilter
    {
        private readonly IPlaceService _placeService;

        public ValidateSavePlaceAttribute(IPlaceService contactService)
        {
            _placeService = contactService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SavePlaceResource savePlaceResource;

            if (context.ActionArguments.ContainsKey("savePlaceResource"))
            {
                savePlaceResource = (SavePlaceResource) context.ActionArguments["savePlaceResource"];
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ErrorResource("Wrong argument provided."));
                return;
            }

            DataResponse<PlaceResource> placeByIdResource = null;

            if (context.ActionArguments.ContainsKey("id"))
            {
                var placeId = (int)context.ActionArguments["id"];
                placeByIdResource = await _placeService.FindByIdAsync(placeId);

                if (!placeByIdResource.Success)
                {
                    context.Result = new NotFoundObjectResult(new ErrorResource("Place no found."));
                    return;
                }
            }

            var placeByNameResource = await _placeService.FindByNameAsync(savePlaceResource.Name);

            if (placeByNameResource.Success)
            {
                if (placeByIdResource == null ||
                    placeByNameResource.Data.Id != placeByIdResource.Data.Id)
                {
                    context.Result = new BadRequestObjectResult(
                     new ErrorResource("There is another place with this name."));
                    return;
                }
            }

            await next();
        }
    }
}

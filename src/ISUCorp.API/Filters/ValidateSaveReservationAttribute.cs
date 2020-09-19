using ISUCorp.API.Resources;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ISUCorp.API.Filters
{
    public class ValidateSaveReservationAttribute : IAsyncActionFilter
    {
        private readonly IReservationService _reservationService;
        private readonly IPlaceService _placeService;

        public ValidateSaveReservationAttribute(IReservationService reservationService,
            IPlaceService placeService)
        {
            _reservationService = reservationService;
            _placeService = placeService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SaveReservationResource saveReservationResource;

            if (context.ActionArguments.ContainsKey("saveReservationResource"))
            {
                saveReservationResource = 
                    (SaveReservationResource)context.ActionArguments["saveReservationResource"];
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ErrorResource("Wrong argument provided."));
                return;
            }

            if (context.ActionArguments.ContainsKey("id"))
            {
                var reservationId = (int)context.ActionArguments["id"];
                var reservationResource = await _reservationService.FindByIdAsync(reservationId);

                if (!reservationResource.Success)
                {
                    context.Result = new NotFoundObjectResult(new ErrorResource("Reservation no found."));
                    return;
                }
            }

            var placeResource = await _placeService.FindByIdAsync(saveReservationResource.PlaceId);

            if (!placeResource.Success)
            {
                context.Result = new NotFoundObjectResult(new ErrorResource("Place no found."));
                return;
            }

            await next();
        }
    }
}

using ISUCorp.API.Resources;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ISUCorp.API.Filters
{
    public class ValidateSaveContactAttribute : IAsyncActionFilter
    {
        private readonly IContactService _contactService;

        public ValidateSaveContactAttribute(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SaveContactResource saveContactResource;

            if (context.ActionArguments.ContainsKey("saveContactResource"))
            {
                saveContactResource = (SaveContactResource)context.ActionArguments["saveContactResource"];
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ErrorResource("Wrong argument provided."));
                return;
            }

            DataResponse<ContactResource> contactByIdResource = null;

            if (context.ActionArguments.ContainsKey("id"))
            {
                var contactId = (int)context.ActionArguments["id"];
                contactByIdResource = await _contactService.FindByIdAsync(contactId);

                if (!contactByIdResource.Success)
                {
                    context.Result = new NotFoundObjectResult(new ErrorResource("Contact no found."));
                    return;
                }
            }

            var contactByNameResource = await _contactService.FindByNameAsync(saveContactResource.Name);

            if (contactByNameResource.Success)
            {
                if (contactByIdResource == null || 
                    contactByNameResource.Data.Id != contactByIdResource.Data.Id)
                {
                    context.Result = new BadRequestObjectResult(
                     new ErrorResource("There is another contact with this name."));
                    return;
                }
            }

            await next();
        }
    }
}

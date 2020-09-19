using ISUCorp.Core.Domain;
using ISUCorp.Services.Resources.Requests;
using System;

namespace ISUCorp.Services.Mappers
{
    public class PlaceMapper
    {
        public static void Map(Place place, SavePlaceResource placeResource)
        {
            if (place == null || placeResource == null)
            {
                throw new ArgumentNullException("Wrong place or place resource provided.");
            }

            place.Name = placeResource.Name;
            place.Description = placeResource.Description;
        }
    }
}

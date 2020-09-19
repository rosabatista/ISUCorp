using AutoMapper;
using ISUCorp.Core.Domain;
using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using System.Collections.Generic;

namespace ISUCorp.Services.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Contact, ContactResource>();
            CreateMap<SaveContactResource, Contact>();
            CreateMap<PagedResponse<List<Contact>>, PagedResponse<List<ContactResource>>>();

            CreateMap<Place, PlaceResource>();
            CreateMap<SavePlaceResource, Place>();
            CreateMap<PagedResponse<List<Place>>, PagedResponse<List<PlaceResource>>>();

            CreateMap<Reservation, ReservationResource>();
            CreateMap<SaveReservationResource, Reservation>();
            CreateMap<PagedResponse<List<Reservation>>, PagedResponse<List<ReservationResource>>>();
        }
    }
}

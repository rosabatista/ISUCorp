using ISUCorp.Core.Domain;
using ISUCorp.Services.Resources.Requests;
using System;

namespace ISUCorp.Services.Mappers
{
    public class ReservationMapper
    {
        public static void Map(Reservation reservation, SaveReservationResource reservationResource)
        {
            if (reservation == null || reservationResource == null)
            {
                throw new ArgumentNullException("Wrong reservation or reservation resource provided.");
            }

            reservation.Date = reservationResource.Date;
            reservation.Notes = reservationResource.Notes;
            reservation.Rating = reservationResource.Rating;
            reservation.IsFavorite = reservation.IsFavorite;
        }
    }
}

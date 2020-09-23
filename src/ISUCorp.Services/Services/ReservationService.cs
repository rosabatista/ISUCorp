using AutoMapper;
using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contracts;
using ISUCorp.Infra.Specifications;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Exceptions;
using ISUCorp.Services.Extensions;
using ISUCorp.Services.Mappers;
using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISUCorp.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAsyncRepository<Reservation> _reservationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationService(IAsyncRepository<Reservation> reservationRepository,
            IContactRepository contactRepository,
            IPlaceRepository placeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _contactRepository = contactRepository;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds a reservation with the given identifier.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>reservation information whether exists.</returns>
        public async Task<DataResponse<ReservationResource>> FindByIdAsync(int id)
        {
            try
            {
                var reservation = await _reservationRepository.FindByIdExtendedAsync(
                    id, r => r.Contact, r => r.Place);

                if (reservation == null)
                {
                    throw new NotFoundException(nameof(Reservation), id);
                }

                var reservationResource = _mapper.Map<ReservationResource>(reservation);
                return new DataResponse<ReservationResource>(reservationResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ReservationResource>(ex.Message);
            }
        }

        /// <summary>
        /// Gets a paginated list of reservations.
        /// </summary>
        /// <param name="queryResource">Paginating, sorting and searching query parameters.</param>
        /// <returns>Paginated list of reservations.</returns>
        public PagedResponse<List<ReservationResource>> ListAsync(QueryResource queryResource)
        {
            try
            {
                if (queryResource == null)
                {
                    throw new ArgumentNullException(nameof(queryResource));
                }

                var reservations = _reservationRepository.DbSet()
                    .Include(r => r.Contact)
                    .Include(r => r.Place)
                    .Select(r => r);

                if (!string.IsNullOrWhiteSpace(queryResource.SearchBy))
                {
                    reservations = reservations.Where(c => 
                        c.Contact.Name.ToLower().Contains(queryResource.SearchBy.Trim().ToLower()) ||
                        c.Place.Name.ToLower().Contains(queryResource.SearchBy.Trim().ToLower()));
                }

                reservations = reservations.ApplyOrderToReservation(queryResource.SortOrder);

                var pagedResponse = PagedResponse<Reservation>.ToPagedResponse(
                    reservations, queryResource.PageNumber, queryResource.PageSize);

                return _mapper.Map<PagedResponse<List<ReservationResource>>>(pagedResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<ReservationResource>>(
                    ex.Message, queryResource.PageNumber, queryResource.PageSize);
            }
        }

        /// <summary>
        /// Adds a reservation.
        /// </summary>
        /// <param name="reservationResource">New reservation information</param>
        /// <returns>Reservation information.</returns>
        public async Task<DataResponse<ReservationResource>> AddAsync(
            SaveReservationResource reservationResource)
        {
            try
            {
                if (reservationResource == null)
                {
                    throw new ArgumentNullException(nameof(reservationResource));
                }

                var place = await _placeRepository.FindByIdAsync(reservationResource.PlaceId);

                if (place == null)
                {
                    throw new NotFoundException(nameof(Place), reservationResource.PlaceId);
                }

                var reservation = _mapper.Map<Reservation>(reservationResource);
                reservation.Place = place;

                if (reservationResource.Contact != null)
                {
                    var contact = await _contactRepository.FirstOrDefaultAsync(
                        new ContactsByNameSpec(reservationResource.Contact.Name));

                    if (contact != null)
                    {
                        ContactMapper.Map(contact, reservationResource.Contact);
                        reservation.Contact = contact;
                    }
                    else
                    {
                        reservation.Contact = _mapper.Map<Contact>(reservationResource.Contact);
                    }
                }

                await _reservationRepository.AddAsync(reservation);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<ReservationResource>(reservation);
                return new DataResponse<ReservationResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ReservationResource>(ex.Message);
            }
        }

        /// <summary>
        /// Updates a reservation.
        /// </summary>
        /// <param name="reservationId">Reservation identifier.</param>
        /// <param name="reservationResource">New reservation information</param>
        /// <returns>Reservation information.</returns>
        public async Task<DataResponse<ReservationResource>> UpdateAsync(int reservationId,
            SaveReservationResource reservationResource)
        {
            try
            {
                if (reservationResource == null)
                {
                    throw new ArgumentNullException(nameof(reservationResource));
                }

                var reservation = await _reservationRepository.FindByIdAsync(reservationId);

                if (reservation == null)
                {
                    throw new NotFoundException(nameof(Reservation), reservationId);
                }

                var place = await _placeRepository.FindByIdAsync(reservationResource.PlaceId);
                reservation.Place = place 
                    ?? throw new NotFoundException(nameof(Place), reservationResource.PlaceId);

                if (reservationResource.Contact != null)
                {
                    var contact = await _contactRepository.FirstOrDefaultAsync(
                        new ContactsByNameSpec(reservationResource.Contact.Name));

                    if (contact != null)
                    {
                        ContactMapper.Map(contact, reservationResource.Contact);
                        reservation.Contact = contact;
                    }
                    else
                    {
                        reservation.Contact = _mapper.Map<Contact>(reservationResource.Contact);
                    }
                }

                ReservationMapper.Map(reservation, reservationResource);
                _reservationRepository.Update(reservation);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<ReservationResource>(reservation);
                return new DataResponse<ReservationResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ReservationResource>(ex.Message);
            }
        }

        /// <summary>
        /// Removes a reservation.
        /// </summary>
        /// <param name="reservationId">Reservation identifier.</param>
        /// <returns>Whether the reservation was removed.</returns>
        public async Task<YesNoResponse> RemoveAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.FindByIdAsync(reservationId);

                if (reservation == null)
                {
                    throw new NotFoundException(nameof(Reservation), reservationId);
                }

                _reservationRepository.Remove(reservation);
                await _unitOfWork.SaveChangesAsync();

                return new YesNoResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new YesNoResponse(ex.Message);
            }
        }

        public async Task<YesNoResponse> RateAsync(int reservationId, int rating)
        {
            try
            {
                var reservation = await _reservationRepository.FindByIdAsync(reservationId);

                if (reservation == null)
                {
                    throw new NotFoundException(nameof(Reservation), reservationId);
                }

                reservation.Rating = rating;
                _reservationRepository.Update(reservation);
                await _unitOfWork.SaveChangesAsync();

                return new YesNoResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new YesNoResponse(ex.Message);
            }
        }

        public async Task<YesNoResponse> SetFavoriteAsync(int reservationId, bool favorite)
        {
            try
            {
                var reservation = await _reservationRepository.FindByIdAsync(reservationId);

                if (reservation == null)
                {
                    throw new NotFoundException(nameof(Reservation), reservationId);
                }

                reservation.IsFavorite = favorite;
                _reservationRepository.Update(reservation);
                await _unitOfWork.SaveChangesAsync();

                return new YesNoResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new YesNoResponse(ex.Message);
            }
        }
    }
}

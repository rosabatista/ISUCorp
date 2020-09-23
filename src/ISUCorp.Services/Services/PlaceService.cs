using AutoMapper;
using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contracts;
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
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaceService(IPlaceRepository placeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds a place with the given identifier.
        /// </summary>
        /// <param name="id">Place identifier.</param>
        /// <returns>Place information whether exists.</returns>
        public async Task<DataResponse<PlaceResource>> FindByIdAsync(int id)
        {
            try
            {
                var place = await _placeRepository.FindByIdAsync(id);

                if (place == null)
                {
                    throw new NotFoundException(nameof(Place), id);
                }

                var placeResource = _mapper.Map<Place, PlaceResource>(place);
                return new DataResponse<PlaceResource>(placeResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<PlaceResource>(ex.Message);
            }
        }

        /// <summary>
        /// Finds a place with the given name.
        /// </summary>
        /// <param name="name">Place name.</param>
        /// <returns>Place information whether exists.</returns>
        public async Task<DataResponse<PlaceResource>> FindByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var place = (await _placeRepository.SearchByName(name)).FirstOrDefault();
                var placeResource = _mapper.Map<Place, PlaceResource>(place);
                return new DataResponse<PlaceResource>(placeResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<PlaceResource>(ex.Message);
            }
        }

        /// <summary>
        /// Gets a paginated list of places.
        /// </summary>
        /// <param name="queryResource">Paginating, sorting and searching query parameters.</param>
        /// <returns>Paginated list of places.</returns>
        public PagedResponse<List<PlaceResource>> ListAsync(QueryResource queryResource)
        {
            try
            {
                if (queryResource == null)
                {
                    throw new ArgumentNullException(nameof(queryResource));
                }

                var places = _placeRepository.DbSet();

                if (!string.IsNullOrWhiteSpace(queryResource.SearchBy))
                {
                    places = places.Where(
                        c => c.Name.ToLower().Contains(queryResource.SearchBy.Trim().ToLower()));
                }

                places = places.ApplyOrder(queryResource.SortOrder);

                var pagedResponse = PagedResponse<Place>.ToPagedResponse(
                    places, queryResource.PageNumber, queryResource.PageSize);

                return _mapper.Map<PagedResponse<List<PlaceResource>>>(pagedResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<PlaceResource>>(
                    ex.Message, queryResource.PageNumber, queryResource.PageSize);
            }
        }

        /// <summary>
        /// Adds a place.
        /// </summary>
        /// <param name="placeResource">New place information</param>
        /// <returns>Place information.</returns>
        public async Task<DataResponse<PlaceResource>> AddAsync(SavePlaceResource placeResource)
        {
            try
            {
                if (placeResource == null)
                {
                    throw new ArgumentNullException(nameof(placeResource));
                }

                var place = _mapper.Map<Place>(placeResource);
                await _placeRepository.AddAsync(place);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<PlaceResource>(place);
                return new DataResponse<PlaceResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<PlaceResource>(ex.Message);
            }
        }

        /// <summary>
        /// Updates a place.
        /// </summary>
        /// <param name="placeId">Place identifier.</param>
        /// <param name="contactResource">New place information</param>
        /// <returns>Place information.</returns>
        public async Task<DataResponse<PlaceResource>> UpdateAsync(int placeId,
            SavePlaceResource placeResource)
        {
            try
            {
                if (placeResource == null)
                {
                    throw new ArgumentNullException(nameof(placeResource));
                }

                var place = await _placeRepository.FindByIdAsync(placeId);

                if (place == null)
                {
                    throw new NotFoundException(nameof(Place), placeId);
                }

                PlaceMapper.Map(place, placeResource);
                _placeRepository.Update(place);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<PlaceResource>(place);
                return new DataResponse<PlaceResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<PlaceResource>(ex.Message);
            }
        }

        /// <summary>
        /// Removes a place.
        /// </summary>
        /// <param name="contactId">Place identifier.</param>
        /// <returns>Whether the place was removed.</returns>
        public async Task<YesNoResponse> RemoveAsync(int contactId)
        {
            try
            {
                var place = await _placeRepository.FindByIdAsync(contactId);

                if (place == null)
                {
                    throw new NotFoundException(nameof(Place), contactId);
                }

                _placeRepository.Remove(place);
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

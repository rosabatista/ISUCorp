using AutoMapper;
using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contracts;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Exceptions;
using ISUCorp.Services.Extensions;
using ISUCorp.Services.Mappers;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISUCorp.Services.Services
{
    /// <summary>
    /// Represents a service to query or operate over <see cref="Contact"/> repository.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds a contact with the given identifier.
        /// </summary>
        /// <param name="id">Contact identifier.</param>
        /// <returns>Contact information whether exists.</returns>
        public async Task<DataResponse<ContactResource>> FindByIdAsync(int id)
        {
            try
            {
                var contact = await _contactRepository.FindByIdAsync(id);

                if (contact == null)
                {
                    throw new NotFoundException(nameof(Contact), id);
                }

                var contactResource = _mapper.Map<Contact, ContactResource>(contact);
                return new DataResponse<ContactResource>(contactResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ContactResource>(ex.Message);
            }
        }

        /// <summary>
        /// Finds a contact with the given name.
        /// </summary>
        /// <param name="name">Contact name.</param>
        /// <returns>Contact information whether exists.</returns>
        public async Task<DataResponse<ContactResource>> FindByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var contact = (await _contactRepository.SearchByName(name)).FirstOrDefault();
                var contactResource = _mapper.Map<Contact, ContactResource>(contact);
                return new DataResponse<ContactResource>(contactResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ContactResource>(ex.Message);
            }
        }

        /// <summary>
        /// Gets a paginated list of contacts.
        /// </summary>
        /// <param name="queryResource">Paginating, sorting and searching query parameters.</param>
        /// <returns>Paginated list of contacts.</returns>
        public PagedResponse<List<ContactResource>> ListAsync(QueryResource queryResource)
        {
            try
            {
                if (queryResource == null)
                {
                    throw new ArgumentNullException(nameof(queryResource));
                }

                var contacts = _contactRepository.DbSet();

                if (!string.IsNullOrWhiteSpace(queryResource.SearchBy))
                {
                    contacts = contacts.Where(
                        c => c.Name.ToLower().Contains(queryResource.SearchBy.Trim().ToLower()));
                }

                contacts = contacts.ApplyOrder(queryResource.SortOrder);

                var pagedResponse = PagedResponse<Contact>.ToPagedResponse(
                    contacts, queryResource.PageNumber, queryResource.PageSize);

                return _mapper.Map<PagedResponse<List<ContactResource>>>(pagedResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<ContactResource>>(
                    ex.Message, queryResource.PageNumber, queryResource.PageSize);
            }
        }

        /// <summary>
        /// Adds a contact.
        /// </summary>
        /// <param name="contactResource">New contact information</param>
        /// <returns>Contact information.</returns>
        public async Task<DataResponse<ContactResource>> AddAsync(SaveContactResource contactResource)
        {
            try
            {
                if (contactResource == null)
                {
                    throw new ArgumentNullException(nameof(contactResource));
                }

                var contact = _mapper.Map<SaveContactResource, Contact>(contactResource);
                await _contactRepository.AddAsync(contact);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<Contact, ContactResource>(contact);
                return new DataResponse<ContactResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ContactResource>(ex.Message);
            }
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contactId">Contact identifier.</param>
        /// <param name="contactResource">New contact information</param>
        /// <returns>Contact information.</returns>
        public async Task<DataResponse<ContactResource>> UpdateAsync(int contactId, 
            SaveContactResource contactResource)
        {
            try
            {
                if (contactResource == null)
                {
                    throw new ArgumentNullException(nameof(contactResource));
                }

                var contact = await _contactRepository.FindByIdAsync(contactId);

                if (contact == null)
                {
                    throw new NotFoundException(nameof(Contact), contactId);
                }

                ContactMapper.Map(contact, contactResource);
                _contactRepository.Update(contact);
                await _unitOfWork.SaveChangesAsync();

                var resource = _mapper.Map<Contact, ContactResource>(contact);
                return new DataResponse<ContactResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<ContactResource>(ex.Message);
            }
        }

        /// <summary>
        /// Removes a contact.
        /// </summary>
        /// <param name="contactId">Contact identifier.</param>
        /// <returns>Whether the contact was removed.</returns>
        public async Task<YesNoResponse> RemoveAsync(int contactId)
        {
            try
            {
                var contact = await _contactRepository.FindByIdAsync(contactId);

                if (contact == null)
                {
                    throw new NotFoundException(nameof(Contact), contactId);
                }

                _contactRepository.Remove(contact);
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

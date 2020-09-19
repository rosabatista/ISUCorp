using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Services.Contracts.Services
{
    public interface IReservationService
    {
        Task<DataResponse<ReservationResource>> FindByIdAsync(int id);
        PagedResponse<List<ReservationResource>> ListAsync(QueryResource queryResource);
        Task<DataResponse<ReservationResource>> AddAsync(
            SaveReservationResource reservationResource);
        Task<DataResponse<ReservationResource>> UpdateAsync(int reservationId,
            SaveReservationResource reservationResource);
        Task<YesNoResponse> RemoveAsync(int reservationId);
    }
}

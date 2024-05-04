using BookingSystem.Core.Model.Common;
using BookingSystem.Core.Model.Request;
using BookingSystem.Core.Model.Response;

namespace BookingSystem.Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<bool> IsBookingAvailable(DateTime startTime, DateTime endTime);
        Task<bool> CanBookSettlement();
        Task<Guid> CreateBooking(Booking booking);
    }
}

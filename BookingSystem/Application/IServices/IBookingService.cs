using BookingSystem.Application.Services;
using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Model.Response;

namespace BookingSystem.Application.IServices
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBooking(string startTime, string name);
        Task<bool> IsWithinBusinessHours(DateTime time);
        Task<bool> IsValidTime(string time);
    }
}

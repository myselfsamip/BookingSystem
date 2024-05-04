using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Model.Common;

namespace BookingSystem.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private static List<Booking> _bookings = new List<Booking>();
        private static int _maxSimultaneousSettlements = 4;

        public async Task<bool> IsBookingAvailable(DateTime startTime, DateTime endTime)
        {
            return !_bookings.Any(b => (startTime >= b.StartTime && startTime <= b.EndTime) || (endTime >= b.StartTime && endTime <= b.EndTime));
        }
        public async Task<bool> CanBookSettlement()
        {
            return _bookings.Count < _maxSimultaneousSettlements;
        }
        public async Task<Guid> CreateBooking(Booking booking)
        {
            booking.Id = Guid.NewGuid();
            _bookings.Add(booking);
            return booking.Id;
        }
    }
}

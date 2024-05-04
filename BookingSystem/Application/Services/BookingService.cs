using BookingSystem.Application.IServices;
using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Model.Common;
using BookingSystem.Core.Model.Response;
using System.Globalization;

namespace BookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task<BookingResponse> CreateBooking(string startTime, string name)
        {
            // Check if time is valid
            if (!await IsValidTime(startTime))
            {
                throw new BadHttpRequestException("Invalid booking time format. Please use HH:mm format.");
                //return new BookingResponse { IsSuccess = false, Message = "Invalid booking time format. Please use HH:mm format." };
                //return Result<Guid>.Fail("Booking time is outside business hours.");
            }

            var bookingTime = DateTime.Parse(startTime);
            DateTime endTime = bookingTime.AddHours(1);

            // Check if booking time is within business hours
            if (! await IsWithinBusinessHours(bookingTime))
            {
                throw new BadHttpRequestException("Booking time is outside business hours.");
                //return new BookingResponse {IsSuccess = false , Message = "Booking time is outside business hours."};
                //return Result<Guid>.Fail("Booking time is outside business hours.");
            }

            // Check if booking time is available
            if (!await _bookingRepository.IsBookingAvailable(bookingTime, endTime))
            {
                return new BookingResponse { IsSuccess = false, Message = "Booking time is already reserved." };
            }

            // Check if maximum simultaneous settlements reached
            if (!await _bookingRepository.CanBookSettlement())
            {
                return new BookingResponse { IsSuccess = false, Message = "Maximum simultaneous settlements reached. Please try again later." };
            }

            var booking = new Booking
            {
                StartTime = bookingTime,
                EndTime = endTime,
                Name = name
            };

            try
            {
                var bookingId = await _bookingRepository.CreateBooking(booking);
                return new BookingResponse { IsSuccess = true,ReturnObject = bookingId, Message = "Success." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the booking.");
                return new BookingResponse { IsSuccess = false, Message = "An error occurred while creating the booking. Please try again later." };
            }
        }

        public async Task<bool> IsWithinBusinessHours(DateTime time)
        {
            var businessStartTime = new TimeSpan(9, 0, 0);
            var businessEndTime = new TimeSpan(16, 0, 0);

            var bookingTime = time.TimeOfDay;

            return bookingTime >= businessStartTime && bookingTime <= businessEndTime;
        }
        public async Task<bool> IsValidTime(string time)
        {
            return DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

    }
}

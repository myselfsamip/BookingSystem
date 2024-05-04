using BookingSystem.Application.IServices;
using BookingSystem.Core.Model.Request;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookingSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Name is required.");
                }

                var result = await _bookingService.CreateBooking(request.BookingTime, request.Name);

                if (result.IsSuccess)
                {
                    return Ok(new { BookingId = result.ReturnObject });
                }
                else
                {
                    return Conflict(result.Message);
                }
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
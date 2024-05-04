namespace BookingSystem.Core.Model.Response
{
    public class BookingResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Guid ReturnObject { get; set; }

        public BookingResponse()
        {
            IsSuccess = true;
            Message = "";
           
        }
    }
}

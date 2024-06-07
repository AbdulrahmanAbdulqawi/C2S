namespace C2S.Data.Dtos
{
    public class BookingUpdateDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; } // "Pending", "Confirmed", "Cancelled"
    }
}

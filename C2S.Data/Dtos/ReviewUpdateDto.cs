namespace C2S.Data.Dtos
{
    public class ReviewUpdateDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; } // Typically on a scale of 1 to 5
        public string Comment { get; set; }
    }
}

namespace KosDrive.DTOs
{
    public class CreateRatingDto
    {
        public Guid ToUserId { get; set; }
        public Guid RideId { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
    }
}

namespace KosDrive.DTOs
{
    public class DisputeResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string UserFullName { get; set; }
        public Guid? RideId { get; set; }
    }
}

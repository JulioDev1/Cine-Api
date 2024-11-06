namespace CineApi.Model
{
    public class Chair
    {
        public Guid Id { get; set; }
        public bool Availibility { get; set; }
        public Guid? UserId { get; set; }
        public Guid MovieId { get; set; }
    }
}

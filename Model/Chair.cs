namespace CineApi.Model
{
    public class Chair
    {
        public Guid Id { get; set; }
        public bool Availibility { get; set; }
        public Guid userId { get; set; }
        public Guid movieId { get; set; }
        public User? Users { get; set; }
        public Movie? Movies { get; set; }
    }
}

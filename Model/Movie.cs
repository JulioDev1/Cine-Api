namespace CineApi.Model
{
    public class Movie
    {

        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int AgeRange { get; set; }
        public required string Genre { get; set; }
        public ICollection<User>? User { get; set; }
        public DateTime EventDay { get; set; }
        public ICollection<Chair>? Chairs { get; set; }
    }
}

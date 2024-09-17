namespace CineApi.Model
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; }
        public required string Password { get; set; }
        public ICollection<Movie>? Movies { get; set; }
        public ICollection<Chair>? Chairs { get; set; }
    }
    public class UserMovies
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }

    }
}

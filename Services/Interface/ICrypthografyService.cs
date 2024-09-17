namespace CineApi.Services.Interface
{
    public interface ICrypthografyService
    {
        string HashPassword(string password);
        bool ComparePassword(string password, string hashedPassword);
    }
}

using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IChairRepository
    {
        Task<Guid> ReserveChairForUser(Guid userId, Guid Id);
        Task<bool> ChairDisponibility(Guid id);
        Task<List<Chair>?> GetAllChairs(Guid movieId);
        Task<Chair> GetChairById(Guid id);
    }
}

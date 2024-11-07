using CineApi.Model;

namespace CineApi.Services.Interface
{
    public interface IChairService
    {
        Task<Guid> ReserveChairForUser(Guid userId, Guid Id);
        Task<List<Chair>?> GetAllChairs(Guid id);
        Task<Chair> GetChairById(Guid id);
        Task CancelChairForUser(Guid id, Guid userId);
    }
}

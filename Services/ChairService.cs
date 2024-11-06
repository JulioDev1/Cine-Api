using CineApi.Model;
using CineApi.Repositories.Interface;
using CineApi.Services.Interface;

namespace CineApi.Services
{
    public class ChairService : IChairService
    {
        private readonly IChairRepository chairRepository;

        public ChairService(IChairRepository chairRepository)
        {
            this.chairRepository = chairRepository;
        }

        public async Task<List<Chair>?> GetAllChairs(Guid id)
        {
            return await chairRepository.GetAllChairs(id);
        }

        public async Task<Chair> GetChairById(Guid id)
        {
          return await chairRepository.GetChairById(id);
           
        }

        public async Task<Guid> ReserveChairForUser(Guid userId, Guid Id)
        {
            var verifyChairAvailable = await chairRepository.ChairDisponibility(Id);

            if (verifyChairAvailable)
            {
                throw new Exception("chair is not available");
            }
            var chair = await chairRepository.ReserveChairForUser(userId, Id);

            return chair;

        }
    }
}

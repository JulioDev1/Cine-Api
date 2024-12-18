﻿using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IChairRepository
    {
        Task<Guid> ReserveChairForUser(Guid userId, Guid? id);
        Task<bool> ChairDisponibility(Guid id);
        Task<List<Chair>?> GetAllChairs(Guid movieId);
        Task cancelChair(Guid id);
        Task<Chair> GetChairById(Guid id);
    }
}

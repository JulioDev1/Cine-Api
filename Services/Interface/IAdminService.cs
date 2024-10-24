﻿using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Services.Interface
{
    public interface IAdminService
    {
        Task<Guid> CreateMovieAndChairs(MovieDto movieDto, Guid userId, int qtd);
        Task<Movie> UpdateMovieAssocieted(MovieDto movieDto, Guid movieId, Guid userId);
        Task<string> DeleteMovieAdmin(Guid Id, Guid userId);
        Task<List<Movie>> AllMoviesByAdmin(Guid userId);
        Task<Movie> GetMovieById(Guid Id, Guid userId);

    }
}

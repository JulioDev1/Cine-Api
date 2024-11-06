using CineApi.Context;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;
using System.Data;
using System.Transactions;

namespace CineApi.Repositories
{
    public class ChairsRepository : IChairRepository
    {

        private readonly AppDbContext appDbContext;
        private readonly IDbConnection connection;
        public ChairsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.connection = appDbContext.CreateConnection();
        }


        public async Task<bool> ChairDisponibility(Guid id)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    var chairDisponibility = @"SELECT availibility FROM chairs WHERE id = @Id";

                    var chair = await connection.QuerySingleOrDefaultAsync<bool>(chairDisponibility, new { Id = id });

                    transaction.Commit();

                    return chair;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<Chair>?> GetAllChairs(Guid id)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var allChairSections = @"SELECT * FROM chairs WHERE movieId = @MovieId";

                    var AllChairsList = await connection.QueryAsync<Chair>(allChairSections, new { MovieId = id });

                    transaction.Commit();

                    return AllChairsList.ToList();
                }
                catch (Exception ex) 
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<Chair> GetChairById(Guid id)
        {
            if (connection.State == ConnectionState.Closed)
            {
                    connection.Open();
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var getChairQuery = @"SELECT * FROM chairs WHERE Id = @Id";

                    var getChairById = await connection.QueryFirstOrDefaultAsync<Chair>(getChairQuery, new { Id = id });

                    transaction.Commit();

                    return getChairById;
                }
                catch (Exception ex) 
                { 
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<Guid> ReserveChairForUser(Guid userId, Guid? id)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    var setWatcherForChair = @"UPDATE chairs SET Availibility = @NewAvailability, 
                                               userId = @UserId WHERE id = @Id RETURNING userId";

                    var updateChairDisponibility = await connection.ExecuteScalarAsync<Guid>(setWatcherForChair, new { NewAvailability = true ,UserId = userId, Id = id });

                    transaction.Commit();
                
                    return updateChairDisponibility;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
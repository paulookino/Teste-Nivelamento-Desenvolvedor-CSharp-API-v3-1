using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task AddIdempotencia(Idempotencia idempotencia)
        {
            using var _conexao = new SqliteConnection(databaseConfig.Name);
            await _conexao.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) " +
                "VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
                idempotencia
            );
        }
    }
}

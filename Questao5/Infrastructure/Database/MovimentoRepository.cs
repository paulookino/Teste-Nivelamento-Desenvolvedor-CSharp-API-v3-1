using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<string> AdicionarMovimento(Movimento movimento)
        {
            using var _conexao = new SqliteConnection(databaseConfig.Name);
            await _conexao.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                $"VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                movimento);

            return movimento.IdMovimento;
        }

        public async Task<IEnumerable<Movimento>> ObterMovimentosPorIdContaCorrente(string idContaCorrente)
        {
            using var _conexao = new SqliteConnection(databaseConfig.Name);
            return  await _conexao.QueryAsync<Movimento>(
                "SELECT * FROM movimento WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = idContaCorrente });
        }
    }

}

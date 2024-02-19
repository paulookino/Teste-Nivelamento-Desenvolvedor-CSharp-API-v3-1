using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente> ObterContaCorrentePorId(string idContaCorrente)
        {
            using var _conexao = new SqliteConnection(databaseConfig.Name);
            return await _conexao.QuerySingleOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = idContaCorrente });
        }
    }

}

using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces
{
    public interface IMovimentoRepository
    {
        Task<string> AdicionarMovimento(Movimento movimento);
        Task<IEnumerable<Movimento>> ObterMovimentosPorIdContaCorrente(string idContaCorrente);
    }
}

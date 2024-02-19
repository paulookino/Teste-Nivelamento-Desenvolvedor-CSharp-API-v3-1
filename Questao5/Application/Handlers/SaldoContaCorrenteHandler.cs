using MediatR;
using Questao5.Application.Interfaces;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Handlers
{
    public class SaldoContaCorrenteHandler : IRequestHandler<SaldoContaCorrenteQuery, Result<SaldoContaCorrenteResponse>>
    {

        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public SaldoContaCorrenteHandler(IMovimentoRepository movimentoRepository,
            IContaCorrenteRepository contaCorrenteRepository)
        {
            this._movimentoRepository = movimentoRepository;
            this._contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<Result<SaldoContaCorrenteResponse>> Handle(SaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {

            // Validações de negócio
            var contaCorrente = await _contaCorrenteRepository.ObterContaCorrentePorId(request.IdContaCorrente);

            if (contaCorrente == null)
                return Result<SaldoContaCorrenteResponse>.Falha("Conta corrente não encontrada", TipoFalha.INVALID_ACCOUNT);

            if (contaCorrente.Ativo == 0)
                return Result<SaldoContaCorrenteResponse>.Falha("Conta corrente inativa", TipoFalha.INACTIVE_ACCOUNT);

            // Consultar movimentos
            var movimentos = await _movimentoRepository.ObterMovimentosPorIdContaCorrente(request.IdContaCorrente);

            // Calcular saldo
            decimal saldoAtual = movimentos.Sum(m => m.Valor);

            // Criar DTO de resposta
            var saldoDto = new SaldoContaCorrenteResponse
            {
                NumeroContaCorrente = contaCorrente.Numero.ToString(),
                NomeTitular = contaCorrente.Nome,
                DataResposta = DateTime.UtcNow,
                SaldoAtual = saldoAtual
            };

            return Result<SaldoContaCorrenteResponse>.Sucesso(saldoDto);
        }
    }

}

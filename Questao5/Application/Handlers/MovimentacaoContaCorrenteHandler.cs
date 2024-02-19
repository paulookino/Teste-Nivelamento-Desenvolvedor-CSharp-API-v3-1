using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoContaCorrenteHandler : IRequestHandler<MovimentacaoContaCorrenteRequest, Result<ResultadoMovimentacao>>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentacaoContaCorrenteHandler(IMovimentoRepository movimentoRepository,
            IContaCorrenteRepository contaCorrenteRepository,
            IIdempotenciaRepository idempotenciaRepository)
        {
            this._movimentoRepository = movimentoRepository;
            this._contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<Result<ResultadoMovimentacao>> Handle(MovimentacaoContaCorrenteRequest request, CancellationToken cancellationToken)
        {

            // Validações de negócio
            if (string.IsNullOrEmpty(request.IdRequisicao))
            {
                throw new Exception("Id de Requisição deve ser enviado.");
            }

            if (request.Valor <= 0)
            {
                throw new Exception("Valor inválido. O valor deve ser positivo.");
            }

            if (request.TipoMovimento != TipoMovimento.Credito && request.TipoMovimento != TipoMovimento.Debito)
            {
                throw new Exception("Tipo de movimento inválido. Apenas 'Credito' ou 'Debito' são aceitos.");
            }

            var contaCorrente = await _contaCorrenteRepository.ObterContaCorrentePorId(request.IdContaCorrente);

            if (contaCorrente == null)
            {
                throw new Exception("Conta corrente não encontrada. Tipo: INVALID_ACCOUNT.");
            }

            if (contaCorrente.Ativo == 0)
            {
                throw new Exception("Conta corrente inativa. Tipo: INACTIVE_ACCOUNT.");
            }

            // Persistência na tabela MOVIMENTO
            var movimento = new Movimento(
                request.IdContaCorrente,
                request.TipoMovimento,
                request.Valor
            );

            await _movimentoRepository.AdicionarMovimento(movimento);

            var retorno = Result<ResultadoMovimentacao>.Sucesso(new ResultadoMovimentacao { IdMovimento = movimento.IdMovimento });

            var newIdempotencia = new Idempotencia(
                request.IdRequisicao,
                JsonSerializer.Serialize(request),
                JsonSerializer.Serialize(retorno)
                );

            await _idempotenciaRepository.AddIdempotencia(newIdempotencia);

            return retorno;
        }
    }

}

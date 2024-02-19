using NSubstitute;
using NUnit.Framework;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Tests.Handlers
{
    [TestFixture]
    public class MovimentacaoContaCorrenteHandlerTests
    {
        [Test]
        public async Task Handle_ValidRequest_SuccessResult()
        {
            // Arrange
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            var handler = new MovimentacaoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository, idempotenciaRepository);

            var request = new MovimentacaoContaCorrenteRequest
            {
                IdRequisicao = "F475F943-7067-ED11-A06B-7E5DFA4A16C8",
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                TipoMovimento = TipoMovimento.Credito,
                Valor = 100.0m
            };

            contaCorrenteRepository.ObterContaCorrentePorId(request.IdContaCorrente)
                .Returns(new ContaCorrente
                {
                    IdContaCorrente = request.IdContaCorrente,
                    Ativo = 1
                });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Sucess);
            Assert.IsNotNull(result.Dados);
        }

        [Test]
        public void Handle_InvalidRequest_ThrowsException()
        {
            // Arrange
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            var handler = new MovimentacaoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository, idempotenciaRepository);

            var request = new MovimentacaoContaCorrenteRequest
            {
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}

using NSubstitute;
using NUnit.Framework;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Tests.Application.Handlers
{
    [TestFixture]
    public class MovimentacaoContaCorrenteHandlerTests
    {
        [Test]
        public void Handle_ValidRequest_ReturnsResultadoMovimentacao()
        {
            // Arrange
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            contaCorrenteRepository.ObterContaCorrentePorId(Arg.Any<string>()).Returns(new ContaCorrente { Ativo = 1 });

            var handler = new MovimentacaoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository, idempotenciaRepository);

            // Act
            var request = new MovimentacaoContaCorrenteRequest
            {
                IdRequisicao = "F475F943-7067-ED11-A06B-7E5DFA4A16C8",
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                TipoMovimento = TipoMovimento.Credito,
                Valor = 100.00m
            };

            var result = handler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Dados);

            contaCorrenteRepository.Received().ObterContaCorrentePorId(Arg.Any<string>());
            movimentoRepository.Received().AdicionarMovimento(Arg.Any<Movimento>());
        }

        [Test]
        public void Handle_InvalidRequest_ThrowsException()
        {
            // Arrange
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            var handler = new MovimentacaoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository, idempotenciaRepository);

            // Act & Assert
            var invalidRequest = new MovimentacaoContaCorrenteRequest
            {
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                TipoMovimento = (TipoMovimento)4,
                Valor = -50.00m
            };

            Assert.ThrowsAsync<Exception>(() => handler.Handle(invalidRequest, CancellationToken.None));

            contaCorrenteRepository.DidNotReceive().ObterContaCorrentePorId(Arg.Any<string>());
            movimentoRepository.DidNotReceive().AdicionarMovimento(Arg.Any<Movimento>());
        }
    }
}

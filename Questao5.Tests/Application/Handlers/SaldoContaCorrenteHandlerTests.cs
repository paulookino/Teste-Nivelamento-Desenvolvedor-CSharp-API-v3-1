using NUnit.Framework;
using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Questao5.Tests.Application.Handlers
{
    [TestFixture]
    public class SaldoContaCorrenteHandlerTests
    {
        [Test]
        public void Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();

            contaCorrenteRepository.ObterContaCorrentePorId(Arg.Any<string>()).Returns(new ContaCorrente { Ativo = 1 });

            var movimentos = new List<Movimento>
            {
                new Movimento(null, TipoMovimento.Credito, 100.00m ),
                new Movimento(null, TipoMovimento.Debito, 50.00m)
            };
            movimentoRepository.ObterMovimentosPorIdContaCorrente(Arg.Any<string>()).Returns(movimentos);

            var handler = new SaldoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository);

            // Act
            var request = new SaldoContaCorrenteQuery
            {
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };

            var result = handler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucess);
            Assert.IsNotNull(result.Dados);

            contaCorrenteRepository.Received().ObterContaCorrentePorId(Arg.Any<string>());
            movimentoRepository.Received().ObterMovimentosPorIdContaCorrente(Arg.Any<string>());
        }

        [Test]
        public void Handle_InvalidRequest_ReturnsFailureResult()
        {
            // Arrange
            var movimentoRepository = Substitute.For<IMovimentoRepository>();
            var contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            var handler = new SaldoContaCorrenteHandler(movimentoRepository, contaCorrenteRepository);

            // Act
            var invalidRequest = new SaldoContaCorrenteQuery
            {
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };

            var result = handler.Handle(invalidRequest, CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Sucess);

            movimentoRepository.DidNotReceive().ObterMovimentosPorIdContaCorrente(Arg.Any<string>());
        }
    }
}

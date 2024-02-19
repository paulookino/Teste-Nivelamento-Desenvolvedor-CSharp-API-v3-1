using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Questao5.Application;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Controllers;
using Questao5.Domain.Enumerators;

namespace Questao5.Tests.Controllers
{
    [TestFixture]
    public class ContaCorrenteControllerTests
    {
        [Test]
        public async Task ObterSaldo_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            

            var saldoQuery = new SaldoContaCorrenteQuery { IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9" };
            var saldoResponse = Result<SaldoContaCorrenteResponse>.Sucesso(
                new SaldoContaCorrenteResponse
                {
                    NumeroContaCorrente = "123456",
                    NomeTitular = "John Doe",
                    DataResposta = DateTime.UtcNow,
                    SaldoAtual = 100.00m
                });


            mediator.Send(Arg.Any<SaldoContaCorrenteQuery>(), Arg.Any<CancellationToken>()).Returns(saldoResponse);

            var controller = new ContaCorrenteController(mediator);

            // Act
            var result = await controller.ObterSaldo(saldoQuery.IdContaCorrente) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);

        }

        [Test]
        public async Task ObterSaldo_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            

            var saldoQuery = new SaldoContaCorrenteQuery { IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9" };
            var saldoResponse = Result<SaldoContaCorrenteResponse>.Falha("Conta corrente não encontrada", 
                TipoFalha.INVALID_ACCOUNT);

            mediator.Send(Arg.Any<SaldoContaCorrenteQuery>(), Arg.Any<CancellationToken>()).Returns(saldoResponse);

            var controller = new ContaCorrenteController(mediator);

            // Act
            var result = await controller.ObterSaldo(saldoQuery.IdContaCorrente) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(result.Value);

        }
    }
}

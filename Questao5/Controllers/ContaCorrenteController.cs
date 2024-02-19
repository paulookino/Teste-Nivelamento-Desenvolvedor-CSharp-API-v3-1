using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Controllers
{
    /// <summary>
    /// API de Conta Corrente
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém saldo da conta corrente
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <returns></returns>
        [HttpGet("{idContaCorrente}/saldo")]
        public async Task<IActionResult> ObterSaldo(string idContaCorrente)
        {
            var query = new SaldoContaCorrenteQuery { IdContaCorrente = idContaCorrente };
            var resultado = await _mediator.Send(query);

            if (resultado.Sucess)
                return Ok(resultado.Dados);
            else
                return BadRequest(new { erro = resultado.MensagemErro, tipoFalha = resultado.TipoFalha });
        }
    }

}

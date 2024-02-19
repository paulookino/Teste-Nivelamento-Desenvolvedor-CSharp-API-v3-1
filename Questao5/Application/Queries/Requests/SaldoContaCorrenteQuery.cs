using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoContaCorrenteQuery : IRequest<Result<SaldoContaCorrenteResponse>>
    {
        public string IdContaCorrente { get; set; }
    }
}

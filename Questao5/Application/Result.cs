using Questao5.Domain.Enumerators;

namespace Questao5.Application
{
    public class Result<T>
    {
        public bool Sucess { get; }
        public T Dados { get; }
        public string MensagemErro { get; }
        public TipoFalha? TipoFalha { get; }

        private Result(bool sucesso, T dados, string mensagemErro, TipoFalha? tipoFalha)
        {
            Sucess = sucesso;
            Dados = dados;
            MensagemErro = mensagemErro;
            TipoFalha = tipoFalha;
        }

        public static Result<T> Sucesso(T dados)
        {
            return new Result<T>(true, dados, null, null);
        }

        public static Result<T> Falha(string mensagemErro, TipoFalha tipoFalha)
        {
            return new Result<T>(false, default, mensagemErro, tipoFalha);
        }
    }
}

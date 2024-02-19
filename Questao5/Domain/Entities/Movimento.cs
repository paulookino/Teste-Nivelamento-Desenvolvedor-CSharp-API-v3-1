using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }

        public Movimento()
        {
                
        }

        public Movimento(string idContaCorrente, TipoMovimento tipoMovimento, decimal valor)
        {
            IdMovimento = Guid.NewGuid().ToString();
            IdContaCorrente = idContaCorrente;
            DataMovimento = DateTime.UtcNow.ToString("dd/MM/yyyy");
            TipoMovimento = tipoMovimento == Enumerators.TipoMovimento.Credito ? "C" : "D";
            Valor = tipoMovimento == Enumerators.TipoMovimento.Credito ? valor : -valor;
        }
    }
}

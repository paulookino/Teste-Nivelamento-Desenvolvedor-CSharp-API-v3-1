using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria
    {
        private readonly int numeroConta;
        private string titular;
        private double saldo;

        public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0.0)
        {
            this.numeroConta = numeroConta;
            this.titular = titular;
            this.saldo = depositoInicial;
        }

        public void Deposito(double valor)
        {
            saldo += valor;
            Console.WriteLine($"Dados da conta atualizados:\nConta {numeroConta}, Titular: {titular}, Saldo: $ {saldo:F2}");
        }

        public void Saque(double valor)
        {
            if (valor + 3.50 > saldo)
            {
                Console.WriteLine("Saldo insuficiente para realizar o saque.");
            }
            else
            {
                saldo -= (valor + 3.50);
                Console.WriteLine($"Dados da conta atualizados:\nConta {numeroConta}, Titular: {titular}, Saldo: $ {saldo:F2}");
            }
        }

        public void AtualizarTitular(string novoTitular)
        {
            titular = novoTitular;
            Console.WriteLine($"Titular atualizado para: {titular}");
        }
    }

}

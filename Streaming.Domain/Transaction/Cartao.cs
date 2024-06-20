using Streaming.Domain.Transaction.Exceptions;
using Streaming.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streaming.Domain.Transaction
{
    public class Cartao
    {
        private const int TRANSACTION_TIME_INTERVAL = -2;
        private const int TRANSACTION_MERCHANT_REPEAT = 1;

        public Guid Id { get; set; }
        public Boolean Ativo { get; set; }
        public Decimal Limite { get; set; }
        public String Numero { get; set; }
        public List<Transacao> Transacoes { get; set; } = new List<Transacao>();

        public void CriarTransacao(string merchant, decimal valor, string descricao)
        {
            CartaoException validationErrors = new CartaoException();

            this.IsCartaoAtivo(validationErrors);

            Transacao transacao = new Transacao
            {
                Merchant = merchant,
                Valor = valor,
                Descricao = descricao,
                DtTransacao = DateTime.Now
            };

            this.VerificarLimiteDisponivel(transacao, validationErrors);

            this.ValidarTransacao(transacao, validationErrors);

            validationErrors.ValidateAndThrow();

            transacao.Id = Guid.NewGuid();
            this.Limite -= transacao.Valor;
            this.Transacoes.Add(transacao);
        }

        private void IsCartaoAtivo(CartaoException validationErrors)
        {
            if (!this.Ativo)
            {
                validationErrors.AddError(new BusinessValidation
                {
                    ErrorDescription = "Cartão não está ativo",
                    ErrorName = nameof(Cartao)
                });
            }
        }

        private void VerificarLimiteDisponivel(Transacao transacao, CartaoException validationErrors)
        {
            if (transacao.Valor > this.Limite)
            {
                validationErrors.AddError(new BusinessValidation
                {
                    ErrorDescription = "Cartão não possui limite para esta transação",
                    ErrorName = nameof(Cartao)
                });
            }
        }

        private void ValidarTransacao(Transacao transacao, CartaoException validationErrors)
        {
            var ultimasTransacao = this.Transacoes.Where(x => x.DtTransacao >= DateTime.Now.AddMinutes(TRANSACTION_TIME_INTERVAL)).ToList();

            if (ultimasTransacao.Count >= 3)
            {
                validationErrors.AddError(new BusinessValidation
                {
                    ErrorDescription = "Cartão utilizado muitas vezes em um período curto",
                    ErrorName = nameof(Cartao)
                });
            }

            bool transacaoMerchantRepetida = ultimasTransacao.Any(x => x.Merchant.ToUpper() == transacao.Merchant.ToUpper() && x.Valor == transacao.Valor);

            if (transacaoMerchantRepetida)
            {
                validationErrors.AddError(new BusinessValidation
                {
                    ErrorDescription = "Transação duplicada para o mesmo cartão e mesmo merchant",
                    ErrorName = nameof(Cartao)
                });
            }
        }
    }
}

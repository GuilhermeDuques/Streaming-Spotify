using Streaming.Domain.Transaction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Cria uma nova transação associada a este cartão.
        public void CriarTransacao(string merchant, decimal valor, string descricao)
        {
            CartaoException validationErrors = new CartaoException();

            this.IsCartaoAtivo(validationErrors);

            Transacao transacao = new Transacao();
            transacao.Merchant = merchant;
            transacao.Valor = valor;
            transacao.Descricao = descricao;
            transacao.DtTransacao = DateTime.Now;

            this.VerificarLimiteDisponivel(transacao, validationErrors);

            this.ValidarTransacao(transacao, validationErrors);

            validationErrors.ValidateAndThrow();

            transacao.Id = Guid.NewGuid();

            this.Limite = this.Limite - transacao.Valor;

            this.Transacoes.Add(transacao);
        }

        // Verifica se o cartão está ativo.
        private void IsCartaoAtivo(CartaoException validationErrors)
        {
            if (this.Ativo == false)
            {
                validationErrors.AddError(new Core.BusinessValidation()
                {
                    ErrorDescription = "Cartão não está ativo",
                    ErrorName = nameof(Cartao)
                });
            }
        }

        // Verifica se há limite disponível para a transação.
        private void VerificarLimiteDisponivel(Transacao transacao, CartaoException validationErrors)
        {
            if (transacao.Valor > this.Limite)
            {
                validationErrors.AddError(new Core.BusinessValidation()
                {
                    ErrorDescription = "Cartão não possui limite para esta transação",
                    ErrorName = nameof(Cartao)
                });
            }
        }

        // Valida a transação em relação a regras específicas do cartão.
        private void ValidarTransacao(Transacao transacao, CartaoException validationErrors)
        {
            var ultimasTransacao = this.Transacoes.Where(x => x.DtTransacao >= DateTime.Now.AddMinutes(TRANSACTION_TIME_INTERVAL));

            if (ultimasTransacao?.Count() >= 3)
            {
                validationErrors.AddError(new Core.BusinessValidation()
                {
                    ErrorDescription = "Cartão utilizado muitas vezes em um período curto",
                    ErrorName = nameof(Cartao)
                });
            }

            var transacaoMerchantRepetida = ultimasTransacao.Where(x => x.Merchant.ToUpper() == transacao.Merchant.ToUpper()
                                                                   && x.Valor == transacao.Valor).Count() == TRANSACTION_MERCHANT_REPEAT;

            if (transacaoMerchantRepetida)
            {
                validationErrors.AddError(new Core.BusinessValidation()
                {
                    ErrorDescription = "Transação duplicada para o mesmo cartão e mesmo merchant",
                    ErrorName = nameof(Cartao)
                });
            }
        }
    }
}
using Streaming.Domain.Core;
using System;
using System.Collections.Generic;

namespace Streaming.Domain.Transaction.Exceptions
{
    public class CartaoException : BusinessException
    {
        public CartaoException() : base() { }

        public CartaoException(BusinessValidation validation) : base(validation) { }

        public void AddError(BusinessValidation validationError)
        {
            base.AddError(validationError);
        }

        public void ValidateAndThrow()
        {
            base.ValidateAndThrow();
        }
    }
}

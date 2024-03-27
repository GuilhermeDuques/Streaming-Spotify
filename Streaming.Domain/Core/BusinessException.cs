using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Domain.Core
{
    public class BusinessException : System.Exception
    {
        public List<BusinessValidation> Errors { get; set; } = new List<BusinessValidation>();
        public BusinessException() { }

        public BusinessException(BusinessValidation validation)
        {
            this.Errors.Add(validation);
        }

        public void AddError(BusinessValidation validation)
        {
            this.Errors.Add(validation);
        }

        public void ValidateAndThrow()
        {
            if (Errors.Count > 0)
                throw this;
        }

    }


    public class BusinessValidation
    {
        public string ErrorName { get; set; } = "Erros de validação";
        public string ErrorDescription { get; set; }
    }
}

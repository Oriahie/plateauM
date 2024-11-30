using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message) : base(message)
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}

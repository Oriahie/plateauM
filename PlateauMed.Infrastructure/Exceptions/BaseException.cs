using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Exceptions
{
    public class BaseException : Exception
    {
        public System.Net.HttpStatusCode HttpStatusCode { get; set; } = System.Net.HttpStatusCode.InternalServerError;
        public BaseException(string message) : base(message) { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string email, string body, string subject);
    }
}

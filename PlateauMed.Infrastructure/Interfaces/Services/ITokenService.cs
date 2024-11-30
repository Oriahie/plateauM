using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserModel user);
    }
}

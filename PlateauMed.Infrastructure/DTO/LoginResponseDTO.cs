using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UserModel User { get; set; }
    }
}

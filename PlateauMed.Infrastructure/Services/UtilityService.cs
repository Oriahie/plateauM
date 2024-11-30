using Microsoft.AspNetCore.Http;
using PlateauMed.Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Services
{
    public class UtilityService : IUtilityService
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public UtilityService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string UserId()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                return userId;
            }
            catch (Exception)
            {
                return "system";
            }

        }

        public string UserName()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? "system";
                return userId;
            }
            catch (Exception)
            {
                return "system";
            }

        }
    }
}

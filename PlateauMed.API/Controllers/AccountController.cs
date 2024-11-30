using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace PlateauMed.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager) => _accountManager = accountManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login([Required][FromBody] LoginRequestDTO model)
        {

            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var token = await _accountManager.Login(model);

            return Ok( new ResponseModel<LoginResponseDTO>
            {
                Data = token,
                Status = true,
                Message = "Login Successful"
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([Required][FromBody] RegisterDTO model)
        {

            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _accountManager.Register(model);

            return Ok(new ResponseModel<UserModel>
            {
                Data = response,
                Status = true,
                Message = "Created successfully"
            });
        }
    }
}

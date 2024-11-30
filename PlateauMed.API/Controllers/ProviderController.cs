using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace PlateauMed.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProviderController : BaseController
    {
        private readonly IAppointmentManager _appointmentManager;
        private readonly IProviderManager _providerManager;

        public ProviderController(IAppointmentManager appointmentManager, IProviderManager providerManager)
        {
            _appointmentManager = appointmentManager;
            _providerManager = providerManager;
        }


        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.GetForProvider();

            return Ok(new ResponseModel<List<AppointmentModel>>
            {
                Data = response,
                Status = true,
                Message = "Retrieved successfully"
            });
        }


        [HttpPut("status/{appointmentId}")]
        public async Task<IActionResult> UpdateStatus([Required] Guid appointmentId,
                                                      [Required][FromQuery] AppointmentStatus status)
        {
            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.UpdateStatus(appointmentId, status);

            return Ok(new ResponseModel<AppointmentModel>
            {
                Data = response,
                Status = true,
                Message = "Updated successfully"
            });
        }


        [HttpPut("availability")]
        public async Task<IActionResult> SetAvailability([Required][FromBody] ProviderAvailabilityDTO model)
        {
            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _providerManager.SetAvailability(model);

            return Ok(new ResponseModel<ProviderModel>
            {
                Data = response,
                Status = true,
                Message = "Updated successfully"
            });
        }
    }
}

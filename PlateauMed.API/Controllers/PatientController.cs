using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace PlateauMed.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IProviderManager _providerManager;
        private readonly IAppointmentManager _appointmentManager;

        public PatientController(IProviderManager providerManager, IAppointmentManager appointmentManager)
        {
            _providerManager = providerManager;
            _appointmentManager = appointmentManager;
        }


        [HttpPost("appointment")]
        public async Task<IActionResult> BookAppointment([Required][FromBody] AppointmentDTO model)
        {
            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.BookAppointment(model);

            return Ok(new ResponseModel<AppointmentModel>
            {
                Data = response,
                Status = true,
                Message = "Updated successfully"
            });
        }

        [HttpGet("providers")]
        public async Task<IActionResult> GetProviders()
        {
            var response = await _providerManager.GetAll();

            return Ok(new ResponseModel<List<ProviderModel>>
            {
                Data = response,
                Status = true,
                Message = "Retrieved successfully"
            });
        }



        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var response = await _appointmentManager.GetForPatient();

            return Ok(new ResponseModel<List<AppointmentModel>>
            {
                Data = response,
                Status = true,
                Message = "Retrieved successfully"
            });
        }

        [HttpPut("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment([Required] Guid appointmentId)
        {
            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.CancelAppointment(appointmentId);

            return Ok(new ResponseModel<AppointmentModel>
            {
                Data = response,
                Status = true,
                Message = "Cancelled successfully"
            });
        }

    }
}

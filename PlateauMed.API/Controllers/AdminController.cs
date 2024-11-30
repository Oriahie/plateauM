using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Models;
using PlateauMed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace PlateauMed.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IAppointmentManager _appointmentManager;

        public AdminController(IAppointmentManager appointmentManager) => _appointmentManager = appointmentManager;


        [HttpGet("metrics")]
        public async Task<IActionResult> GetReport(
            [Required][FromQuery] string datefrom,
            [Required][FromQuery] string dateTo)
        {

            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.GetAppointmentMetrics(datefrom.ConvertToDate(), dateTo.ConvertToDate());

            return Ok(new ResponseModel<AppointmentMetricsModel>
            {
                Data = response,
                Status = true,
                Message = "Retrieved successfully"
            });
        }


        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments(
        [Required][FromQuery] string query,
        [Required][FromQuery] FilterParam filter = FilterParam.None)
        {

            if (!ModelState.IsValid) GetModelStateError(ModelState);
            var response = await _appointmentManager.Get(filter, query);

            return Ok(new ResponseModel<List<AppointmentModel>>
            {
                Data = response,
                Status = true,
                Message = "Retrieved successfully"
            });
        }

    }
}

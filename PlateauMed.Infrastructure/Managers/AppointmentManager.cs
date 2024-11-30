using PlateauMed.Core;
using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlateauMed.Infrastructure.Managers
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUtilityService _utilityService;
        private readonly IProviderRepository _providerRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly INotificationManager _notificationManager;

        public AppointmentManager(IAppointmentRepository appointmentRepository,
                                  IUtilityService utilityService,
                                  IProviderRepository providerRepository,
                                  INotificationManager notificationManager,
                                  IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _utilityService = utilityService;
            _providerRepository = providerRepository;
            _notificationManager = notificationManager;
            _patientRepository = patientRepository;
        }

        public async Task<AppointmentModel> CancelAppointment(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointment(appointmentId);

            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);

            var patient = await _patientRepository.GetByUserId(userId);

            if (appointment.PatientId != patient.PatientID)
                throw new ValidationException("Appointment not assigned to patient");

            var res = await _appointmentRepository.UpdateAppointmentStatus(appointmentId, AppointmentStatus.Cancelled);

            var (patientEmail, providerEmail) = await _appointmentRepository.GetProviderPatientEmail(appointmentId);

            var notificationList = new List<NotificationLogModel>()
            {
                new() {
                    CreatedDate = DateTimeOffset.Now,
                    Type  = NotificationType.AppointmentCancellation,
                    Status = NotificationStatus.NotDelivered,
                    Message = $"Appointment Cancelled",
                    Email = patientEmail
                },
                new() {
                    CreatedDate = DateTimeOffset.Now,
                    Type  = NotificationType.AppointmentCancellation,
                    Status = NotificationStatus.NotDelivered,
                    Message = $"Appointment Cancelled",
                    Email = providerEmail
                }
            };

            await _notificationManager.Notify(notificationList);

            return res;
        }

        public async Task<List<AppointmentModel>> Get(FilterParam filter, string query)
        {
            return await _appointmentRepository.GetAppointments(filter, query);
        }

        public async Task<List<AppointmentModel>> GetForProvider()
        {
            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);

            var provider = await _providerRepository.GetProviderByUserId(userId);
            return await _appointmentRepository.GetAppointments(provider.ProviderId);
        }

        public async Task<AppointmentMetricsModel> GetAppointmentMetrics(DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {

            var startDate = new DateTimeOffset(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0, default);
            var endDate = new DateTimeOffset(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59, default);

            return await _appointmentRepository.GetMetrics(startDate, endDate);
        }

        public async Task<AppointmentModel> UpdateStatus(Guid appointmentId, AppointmentStatus status)
        {
            var appointment = await _appointmentRepository.GetAppointment(appointmentId);

            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);

            var provider = await _providerRepository.GetProviderByUserId(userId);

            if (appointment.ProviderId != provider.ProviderId)
                throw new ValidationException("Appointment not assigned to provider");

            var res = await _appointmentRepository.UpdateAppointmentStatus(appointmentId, status);

            var (patientEmail, providerEmail) = await _appointmentRepository.GetProviderPatientEmail(appointmentId);

            var notificationList = new List<NotificationLogModel>()
            {
                new() {
                    CreatedDate = DateTimeOffset.Now,
                    Type = status.Map(),
                    Status = NotificationStatus.NotDelivered,
                    Message = $"Appointment Status Updated : {status}",
                    Email = patientEmail
                },
                new() {
                    CreatedDate = DateTimeOffset.Now,
                    Type = status.Map(),
                    Status = NotificationStatus.NotDelivered,
                    Message = $"Appointment Status Updated : {status}",
                    Email = providerEmail
                }
            };

            await _notificationManager.Notify(notificationList);

            return res;
        }

        public async Task<List<AppointmentModel>> GetForPatient()
        {
            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);

            var patient = await _patientRepository.GetByUserId(userId);
            return await _appointmentRepository.GetAppointments(patient.PatientID);
        }

        public async Task<AppointmentModel> BookAppointment(AppointmentDTO model)
        {
            var provider = await _providerRepository.GetProvider(model.ProviderId);

            var appointmentTime = model.AppointmentDate.TimeOfDay;
            // Validate if appointment falls within provider's working hours
            if (appointmentTime < provider.StartTime || appointmentTime > provider.CloseTime)
                throw new ValidationException("Appointment is outside provider's working hours");

            var breakEnd = provider.BreakStartTime.Add(TimeSpan.FromHours(provider.BreakDuration));

            // Validate if appointment overlaps with provider's break time
            if (appointmentTime >= provider.BreakStartTime && appointmentTime < breakEnd)
                throw new ValidationException("Appointment falls withing provider's break period");

            //validate if any overlapping appointment exists
            if (await _appointmentRepository.HasOverlappingAppointments(provider.ProviderId, model.AppointmentDate))
                throw new ValidationException("Please select a different date");

            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);

            var patient = await _patientRepository.GetByUserId(userId);


            return await _appointmentRepository.CreateAppointment(model.Map(patient.PatientID));
        }
    }
}

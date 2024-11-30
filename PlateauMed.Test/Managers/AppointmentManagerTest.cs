using FluentAssertions;
using NSubstitute;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Test.Managers
{
    public class AppointmentManagerTest
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUtilityService _utilityService;
        private readonly IProviderRepository _providerRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly INotificationManager _notificationManager;
        private readonly AppointmentManager _appointmentManager;

        public AppointmentManagerTest()
        {
            _appointmentRepository = Substitute.For<IAppointmentRepository>();
            _utilityService = Substitute.For<IUtilityService>();
            _providerRepository = Substitute.For<IProviderRepository>();
            _patientRepository = Substitute.For<IPatientRepository>();
            _notificationManager = Substitute.For<INotificationManager>();

            _appointmentManager = new AppointmentManager(
                _appointmentRepository,
                _utilityService,
                _providerRepository,
                _notificationManager,
                _patientRepository
            );
        }

        [Fact]
        public async Task CancelAppointment_ShouldThrowValidationException_WhenAppointmentDoesNotBelongToPatient()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var patientId = Guid.NewGuid();

            var appointment = new AppointmentModel { Id = appointmentId, PatientId = Guid.NewGuid() };

            _utilityService.UserId().Returns(userId.ToString());
            _appointmentRepository.GetAppointment(appointmentId).Returns(appointment);
            _patientRepository.GetByUserId(userId).Returns(new PatientModel { PatientID = patientId });

            // Act
            var act = async () => await _appointmentManager.CancelAppointment(appointmentId);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Appointment not assigned to patient");
        }

        [Fact]
        public async Task BookAppointment_ShouldThrowValidationException_WhenAppointmentIsOutsideWorkingHours()
        {
            // Arrange
            var providerId = Guid.NewGuid();
            var appointmentDate = DateTime.Now.AddDays(1);

            var provider = new ProviderModel
            {
                ProviderId = providerId,
                StartTime = TimeSpan.FromHours(9),
                CloseTime = TimeSpan.FromHours(17),
                BreakStartTime = TimeSpan.FromHours(12),
                BreakDuration = 1
            };

            var appointmentDTO = new AppointmentDTO
            {
                ProviderId = providerId,
                AppointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 18, 0, 0)
            };

            _providerRepository.GetProvider(providerId).Returns(provider);

            // Act
            var act = async () => await _appointmentManager.BookAppointment(appointmentDTO);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Appointment is outside provider's working hours");
        }

        [Fact]
        public async Task BookAppointment_ShouldThrowValidationException_WhenAppointmentOverlapsWithBreakTime()
        {
            // Arrange
            var providerId = Guid.NewGuid();
            var appointmentDate = DateTime.Now.AddDays(1);

            var provider = new ProviderModel
            {
                ProviderId = providerId,
                StartTime = TimeSpan.FromHours(9),
                CloseTime = TimeSpan.FromHours(17),
                BreakStartTime = TimeSpan.FromHours(12),
                BreakDuration = 1
            };

            var appointmentDTO = new AppointmentDTO
            {
                ProviderId = providerId,
                AppointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 12, 30, 0)
            };

            _providerRepository.GetProvider(providerId).Returns(provider);

            // Act
            var act = async () => await _appointmentManager.BookAppointment(appointmentDTO);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Appointment falls withing provider's break period");
        }
    }
}

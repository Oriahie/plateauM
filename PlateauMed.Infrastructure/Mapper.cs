using PlateauMed.Core;
using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure
{
    public static class Mapper
    {
        public static PatientModel Map(this Patient model)
        {
            if (model == null) return null;
            return new PatientModel
            {
                CreatedDate = model.CreatedDate,
                PatientID = model.Id,
                DateOfBirth = model.DateOfBirth,
                Name = $"{model.User?.FirstName} {model.User?.LastName}"
            };
        }

        public static UserModel Map(this User model)
        {
            if (model == null) return null;
            return new UserModel
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserId = model.Id,
                UserType = model.UserType
            };
        }


        public static UserModel Map(this RegisterDTO model)
        {
            if (model == null) return null;
            return new UserModel
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = model.UserType
            };
        }


        public static User Map(this UserModel model)
        {
            if (model == null) return null;
            return new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = model.UserType
            };
        }


        public static NotificationLog Map(this NotificationLogModel model)
        {
            if (model == null) return null;
            return new NotificationLog
            {
                CreatedDate = model.CreatedDate,
                Email = model.Email,
                Id = model.Id,
                Message = model.Message,
                Status = model.Status,
                Type = model.Type
            };
        }

        public static ProviderModel Map(this Provider model)
        {
            if (model == null) return null;
            return new ProviderModel
            {
                ProviderId = model.Id,
                BreakDuration = model.BreakDurationInMiniutes,
                BreakStartTime = model.BreakStartTime,
                CloseTime = model.CloseTime,
                CreatedDate = model.CreatedDate,
                StartTime = model.StartTime,
                ProviderName = $"{model.User?.FirstName} {model.User?.LastName}",
            };
        }


        public static AppointmentModel Map(this AppointmentDTO model, Guid patientId)
        {
            if (model == null) return null;
            return new AppointmentModel
            {
                AppointmentDate = model.AppointmentDate,
                PatientId = patientId,
                ProviderId = model.ProviderId,
                Reason = model.Reason,
                Status = AppointmentStatus.Pending
            };

        }
        public static NotificationType Map(this AppointmentStatus model)
        {
            return model switch
            {
                AppointmentStatus.Pending => NotificationType.AppointmentCreated,
                AppointmentStatus.Confirmed => NotificationType.AppointmentConfirmation,
                AppointmentStatus.Cancelled => NotificationType.AppointmentCancellation,
                _ => NotificationType.None,
            };
        }

        public static Provider Map(this ProviderModel model, Guid userId)
        {
            if (model == null) return null;
            return new Provider
            {
                BreakDurationInMiniutes = model.BreakDuration,
                BreakStartTime = model.BreakStartTime,
                CloseTime = model.CloseTime,
                CreatedDate = model.CreatedDate,
                StartTime = model.StartTime,
                UserId = userId
            };
        }

        public static Appointment Map(this AppointmentModel model)
        {
            if (model == null) return null;
            return new Appointment
            {
                AppointmentDate = model.AppointmentDate,
                PatientId = model.PatientId,
                ProviderId = model.ProviderId,
                Reason = model.Reason,
                Status = model.Status
            };
        }


        public static AppointmentModel Map(this Appointment model)
        {
            if (model == null) return null;
            return new AppointmentModel
            {
                Id = model.Id,
                CreatedDate = model.CreatedDate,
                PatientName = $"{model.Patient?.User?.FirstName} {model.Patient?.User?.LastName}",
                ProviderName = $"{model.Provider?.User?.FirstName} {model.Provider?.User?.LastName}",
                AppointmentDate = model.AppointmentDate,
                PatientId = model.PatientId,
                ProviderId = model.ProviderId,
                Reason = model.Reason,
                Status = model.Status
            };
        }
    }
}

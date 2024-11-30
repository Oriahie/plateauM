using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Core
{
    public enum UserType
    {
        Patient,
        Provider,
        Administrator
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public enum NotificationStatus
    {
        NotDelivered,
        Delivered
    }

    public enum FilterParam
    {
        None,
        Provider,
        Date,
        Patient
    }

    public enum NotificationType
    {
        AppointmentCreated,
        AppointmentConfirmation,
        AppointmentCancellation,
        AppointmentReminder,
        None
    }
}

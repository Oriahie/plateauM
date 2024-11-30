# PlateauMed
This is a healthcare management system


## Getting Started

To run the project update the appsettings.json files

## Design
- Registration: Users can register as either a patient or a provider.
- Users can login using their email and password
- Providers 
## Appointments
- Booking: Patients can book appointments with providers based on availability, avoiding conflicts such as overlapping appointments or provider break times.
- Cancellation: Patients can cancel their appointments and both the provider and patient will be notified.
- Provider Schedules: Providers can set their availability, including work hours and break periods.
- Metrics: Providers can view appointment statistics over specified time ranges.

## Notifications
Notifications are sent using service bus to enable seamless message delivery
Supports email notifications for various events:
- Appointment bookings.
- Appointment cancellations.
- Appointment status updates.
Notifications are sent to both patients and providers.



## Running Tests

Tests are written using:

- xUnit: Unit testing framework.
- NSubstitute: Mocking framework.
- FluentAssertions: For fluent and readable assertions.

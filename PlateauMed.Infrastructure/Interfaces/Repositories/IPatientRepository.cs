using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        //add patient
        Task<PatientModel> AddPatient(PatientModel patient, Guid UserId);

        //get by patientId
        Task<PatientModel> GetPatient(Guid PatientId);
        Task<PatientModel> GetByUserId(Guid userId);
    }
}

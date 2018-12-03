using PatientDemographics.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.Data
{
    public interface IPatientRepository
    {
        IEnumerable<PatientRecord> GetAllPatientRecords();
        PatientRecord GetPatientRecordById(int id);
        void AddPatientRecord(PatientRecord patientRecord);
        bool Save();
    }
}

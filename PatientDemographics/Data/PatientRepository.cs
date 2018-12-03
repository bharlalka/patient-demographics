using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PatientDemographics.Data.Entities;

namespace PatientDemographics.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext ctx;
        private readonly ILogger<PatientRepository> logger;

        public PatientRepository(PatientContext ctx, ILogger<PatientRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public void AddPatientRecord(PatientRecord patientRecord)
        {
            ctx.Add(patientRecord);
        }

        public IEnumerable<PatientRecord> GetAllPatientRecords()
        {
            return ctx.PatientRecords
                .ToList();
        }

        public PatientRecord GetPatientRecordById(int id)
        {
            return ctx.PatientRecords
                .Where(p => p.Id == id)
                .FirstOrDefault();

        }

        public bool Save()
        {
            return ctx.SaveChanges() > 0;
        }
    }
}

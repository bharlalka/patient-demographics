using Microsoft.EntityFrameworkCore;
using PatientDemographics.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.Data
{
    public class PatientContext : DbContext
    {
        public DbSet<PatientRecord> PatientRecords { get; set; }

        public PatientContext(DbContextOptions<PatientContext> options) : base(options)
        {

        }
    }
}

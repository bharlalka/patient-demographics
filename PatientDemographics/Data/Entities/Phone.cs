using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.Data.Entities
{
    public class Phone
    {
        public PhoneNumberType PhoneType { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }

    public enum PhoneNumberType
    {
        Home,
        Work,
        Mobile
    }
}

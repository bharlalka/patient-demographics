using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.Data.Entities
{
    public class Phone
    {
        public PhoneNumberType PhoneType { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum PhoneNumberType
    {
        Home,
        Work,
        Mobile
    }
}

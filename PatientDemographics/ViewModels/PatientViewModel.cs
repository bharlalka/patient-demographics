using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.ViewModels
{
    public class PatientViewModel
    {
        public string Forename { get; set; }

        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }
    }
}

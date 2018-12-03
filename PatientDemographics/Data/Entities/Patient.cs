using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientDemographics.Data.Entities
{
    public class Patient
    {
        [Required]
        [MinLength(3, ErrorMessage = "Forename too short")]
        [MaxLength(50, ErrorMessage = "Forename too long")]
        public string Forename { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Surname too short")]
        [MaxLength(50, ErrorMessage = "Surname too long")]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }
        public List<Phone> Phones { get; set; }
    }
}

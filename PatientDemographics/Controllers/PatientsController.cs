using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatientDemographics.Data;
using PatientDemographics.Data.Entities;
using PatientDemographics.ViewModels;

namespace PatientDemographics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository repository;
        private readonly ILogger<PatientsController> logger;
        private readonly IMapper mapper;

        public PatientsController(IPatientRepository repository, ILogger<PatientsController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = repository.GetAllPatientRecords();
                List<Patient> patients = new List<Patient>();

                if (results != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));
                    foreach (var result in results)
                    {
                        using (var stringReader = new StringReader(result.Record))
                        {
                            var patient = (Patient)xmlSerializer.Deserialize(stringReader);
                            patients.Add(patient);
                        }
                    }
                }

                return Ok(mapper.Map<IEnumerable<Patient>, IEnumerable<PatientViewModel>>(patients));
            }
            catch(Exception exp)
            {
                logger.LogError($"Failed to get patient records: {exp}");
                return BadRequest("Failed to get patient records");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var patientrecord = repository.GetPatientRecordById(id);

                if(patientrecord != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));
                    using (var stringReader = new StringReader(patientrecord.Record))
                    {
                        var patient = (Patient)xmlSerializer.Deserialize(stringReader);
                        return Ok(mapper.Map<Patient, PatientViewModel>(patient));
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception exp)
            {
                logger.LogError($"Failed to get patient records: {exp}");
                return BadRequest("Failed to get patient records");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Patient patient)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, patient);
                        var patientDetails = textWriter.ToString();

                        var patientRecord = new PatientRecord()
                        {
                            Record = patientDetails
                        };
                        repository.AddPatientRecord(patientRecord);

                        if(repository.Save())
                        {
                            return Created($"/api/patients/{patientRecord.Id}", mapper.Map<Patient, PatientViewModel>(patient));
                        }
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"Failed to save new patient record: {ex}");
            }

            return BadRequest("Failed to save new patient record");
        }
    }
}
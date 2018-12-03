using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientDemographics.Controllers;
using PatientDemographics.Data;
using PatientDemographics.Data.Entities;
using PatientDemographics.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Xunit;

namespace PatientDemographicsTest
{
    public class PatientsControllerTests
    {
        [Fact]
        public void Values_Initial_Get()
        {
            //Arrange
            List<Patient> patients = new List<Patient>()
            {
                new Patient(){ Forename = "Patient1", Surname = "Surname1", Gender = "Male"},
                new Patient(){ Forename = "Patient2", Surname = "Surname2", Gender = "Female", DateOfBirth = Convert.ToDateTime("2003-11-16")},
                new Patient(){
                    Forename = "Patient3",
                    Surname = "Surname3",
                    Gender = "Female",
                    DateOfBirth = Convert.ToDateTime("1983-11-16"),
                    Phones = new List<Phone>(){
                    new Phone(){ PhoneType = PhoneNumberType.Work, PhoneNumber = "001-003-324-2345"},
                    new Phone(){ PhoneType = PhoneNumberType.Mobile, PhoneNumber = "001-234-224-2344"}
                } }
            };

            List<PatientRecord> patientRecords = new List<PatientRecord>();
            foreach(var patient in patients)
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
                    patientRecords.Add(patientRecord);
                }
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(x => x.GetAllPatientRecords()).Returns(patientRecords);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);

            //Act
            IActionResult results = controller.Get();

            //Assert
            var okResult = results as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Values_Patient_Add()
        {
            //Arrange
            var patient = new Patient();
            var phones = new List<Phone>();
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Home,
                PhoneNumber = "001-024-045-1234"
            });
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Work,
                PhoneNumber = "001-024-045-1232"
            });

            patient.Forename = "John";
            patient.Surname = "Mathews";
            patient.DateOfBirth = Convert.ToDateTime("2002-02-14");
            patient.Gender = "Male";
            patient.Phones = phones;

            var patientRecord = new PatientRecord();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, patient);
                var patientDetails = textWriter.ToString();
                patientRecord.Id = 1;
                patientRecord.Record = patientDetails;
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.AddPatientRecord(patientRecord));
            mockRepo.Setup(r => r.Save()).Returns(true);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);


            //Act
            IActionResult results = controller.Post(patient);

            //Assert
            var createdResult = results as CreatedResult;

            // assert
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void Values_Patient_Forename_Less_Than_3_Add()
        {
            //Arrange
            var patient = new Patient();
            var phones = new List<Phone>();
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Home,
                PhoneNumber = "001-024-045-1234"
            });
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Work,
                PhoneNumber = "001-024-045-1232"
            });

            patient.Forename = "Jo";
            patient.Surname = "Mathews";
            patient.DateOfBirth = Convert.ToDateTime("2002-02-14");
            patient.Gender = "Male";
            patient.Phones = phones;

            var patientRecord = new PatientRecord();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, patient);
                var patientDetails = textWriter.ToString();
                patientRecord.Id = 1;
                patientRecord.Record = patientDetails;
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.AddPatientRecord(patientRecord));
            mockRepo.Setup(r => r.Save()).Returns(true);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);
            controller.ModelState.AddModelError("Description", "Forename too short");

            //Act
            IActionResult results = controller.Post(patient);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(results);
            Assert.Equal(new SerializableError(controller.ModelState), actionResult.Value);
        }

        [Fact]
        public void Values_Patient_Forename_More_Than_50_Add()
        {
            //Arrange
            var patient = new Patient();
            var phones = new List<Phone>();
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Home,
                PhoneNumber = "001-024-045-1234"
            });
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Work,
                PhoneNumber = "001-024-045-1232"
            });

            patient.Forename = "Jofsdhsagdjagjjevuytuygjjhgjhguyguyafyfdshvdnavdjevedjhg";
            patient.Surname = "Mathews";
            patient.DateOfBirth = Convert.ToDateTime("2002-02-14");
            patient.Gender = "Male";
            patient.Phones = phones;

            var patientRecord = new PatientRecord();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, patient);
                var patientDetails = textWriter.ToString();
                patientRecord.Id = 1;
                patientRecord.Record = patientDetails;
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.AddPatientRecord(patientRecord));
            mockRepo.Setup(r => r.Save()).Returns(true);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);
            controller.ModelState.AddModelError("Description", "Forename too long");

            //Act
            IActionResult results = controller.Post(patient);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(results);
            Assert.Equal(new SerializableError(controller.ModelState), actionResult.Value);
        }


        [Fact]
        public void Values_Patient_Surname_Less_Than_2_Add()
        {
            //Arrange
            var patient = new Patient();
            var phones = new List<Phone>();
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Home,
                PhoneNumber = "001-024-045-1234"
            });
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Work,
                PhoneNumber = "001-024-045-1232"
            });

            patient.Surname = "J";
            patient.Forename = "Mathews";
            patient.DateOfBirth = Convert.ToDateTime("2002-02-14");
            patient.Gender = "Male";
            patient.Phones = phones;

            var patientRecord = new PatientRecord();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, patient);
                var patientDetails = textWriter.ToString();
                patientRecord.Id = 1;
                patientRecord.Record = patientDetails;
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.AddPatientRecord(patientRecord));
            mockRepo.Setup(r => r.Save()).Returns(true);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);
            controller.ModelState.AddModelError("Description", "Surname too short");

            //Act
            IActionResult results = controller.Post(patient);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(results);
            Assert.Equal(new SerializableError(controller.ModelState), actionResult.Value);
        }

        [Fact]
        public void Values_Patient_Surname_More_Than_50_Add()
        {
            //Arrange
            var patient = new Patient();
            var phones = new List<Phone>();
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Home,
                PhoneNumber = "001-024-045-1234"
            });
            phones.Add(new Phone()
            {
                PhoneType = PhoneNumberType.Work,
                PhoneNumber = "001-024-045-1232"
            });

            patient.Surname = "Jofsdhsagdjagjjevuytuygjjhgjhguyguyafyfdshvdnavdjevedjhg";
            patient.Forename = "Mathews";
            patient.DateOfBirth = Convert.ToDateTime("2002-02-14");
            patient.Gender = "Male";
            patient.Phones = phones;

            var patientRecord = new PatientRecord();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Patient));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, patient);
                var patientDetails = textWriter.ToString();
                patientRecord.Id = 1;
                patientRecord.Record = patientDetails;
            }

            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.AddPatientRecord(patientRecord));
            mockRepo.Setup(r => r.Save()).Returns(true);
            var repository = mockRepo.Object;

            var mapper = new Mock<IMapper>().Object;

            Mapper.Reset();
            Mapper.Initialize(m =>
                m.CreateMap<Patient, PatientViewModel>().ReverseMap());

            var mock = new Mock<ILogger<PatientsController>>();
            var logger = mock.Object;

            var controller = new PatientsController(repository, logger, mapper);
            controller.ModelState.AddModelError("Description", "Surname too long");

            //Act
            IActionResult results = controller.Post(patient);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(results);
            Assert.Equal(new SerializableError(controller.ModelState), actionResult.Value);
        }


    }
}

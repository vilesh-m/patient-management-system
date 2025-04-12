using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientAPI.Controllers;
using PatientSystem.Services.Interfaces;
using PatientSystem.Services.Models;

namespace PatientManagementAPI.Tests
{
    public class PatientControllerTests
    {
        private readonly Mock<IPatientRepository> _mockDbService;
        private readonly PatientController _controller;

        public PatientControllerTests()
        {
            _mockDbService = new Mock<IPatientRepository>();
            _controller = new PatientController(_mockDbService.Object);
        }

        [Fact]
        public void GetAllPatients_ReturnsAllPatients()
        {
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "John", LastName = "Doe", Age = 30, Gender = "Male" },
                new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", Age = 25, Gender = "Female" }
            };

            _mockDbService.Setup(db => db.GetAllPatients()).Returns(patients);

            var result = _controller.GetAllPatients() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(patients, result.Value);
        }

        [Fact]
        public void AddPatient_WithValidData_ReturnsCreatedAtAction()
        {
            var patientRequest = new PatientRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                Gender = "Male"
            };

            _mockDbService.Setup(db => db.AddPatient(It.IsAny<Patient>())).Returns(1);

            var result = _controller.AddPatient(patientRequest) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("AddPatient", result.ActionName);
            
            var patient = result.Value as Patient;
            Assert.Equal("John", patient.FirstName);
            Assert.Equal("Doe", patient.LastName);
            Assert.Equal(30, patient.Age);
            Assert.Equal("Male", patient.Gender);
        }

        public class DeletePatientTestData
        {
            public static IEnumerable<object[]> PatientDeleteTestData =>
                new List<object[]>
                {
                    new object[] 
                    { 
                        1, 
                        new Patient { Id = 1, FirstName = "John", LastName = "Doe", Age = 30, Gender = "Male" },
                        true, 
                        true,
                        200 
                    },
                    new object[] 
                    { 
                        1, 
                        null, 
                        false,
                        false,
                        404 
                    }
                };
        }

        [Theory]
        [MemberData(nameof(DeletePatientTestData.PatientDeleteTestData), MemberType = typeof(DeletePatientTestData))]
        public void DeletePatient_ReturnsExpectedResult(int patientId, Patient patientToReturn, bool deleteResult, bool shouldSucceed, int expectedStatusCode)
        {
            _mockDbService.Setup(db => db.GetPatient(patientId)).Returns(patientToReturn);
            if (patientToReturn != null)
            {
                _mockDbService.Setup(db => db.DeletePatient(patientId)).Returns(deleteResult);
            }

            var result = _controller.DeletePatient(patientId);

            Assert.NotNull(result);
            
            if (shouldSucceed)
            {
                var okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                Assert.Equal(expectedStatusCode, okResult.StatusCode);
            }
            else
            {
                var notFoundResult = result as NotFoundObjectResult;
                Assert.NotNull(notFoundResult);
                Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
            }
        }
    }
}

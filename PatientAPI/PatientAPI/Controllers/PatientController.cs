using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PatientAPI.Models;
using PatientAPI.Services;

namespace PatientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public PatientController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public class PatientRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public IActionResult GetAllPatients()
        {
            var patients = _dbService.GetAllPatients();
            return Ok(patients);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public IActionResult AddPatient([FromBody] PatientRequest patientData)
        {
            try
            {
                var patient = new Patient
                {
                    FirstName = patientData.FirstName,
                    LastName = patientData.LastName,
                    Age = patientData.Age,
                    Gender = patientData.Gender,
                    Attachments = new List<FileAttachment>()
                };

                var id = _dbService.AddPatient(patient);
                return Ok(new { message = "Patient Created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeletePatient(int id)
        {
            var patient = _dbService.GetPatient(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            var result = _dbService.DeletePatient(id);
            if (!result)
                return BadRequest(new { message = "Failed to delete patient" });

            return Ok(new { message = "Patient deleted successfully" });
        }


        [HttpPost("{id}/attachments")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddFileAttachment(int id)
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "No file dound in the body" });

                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var attachment = new FileAttachment
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        FileData = ms.ToArray(),
                    };

                    var result = _dbService.AddFileAttachment(id, attachment);
                    if (!result)
                        return NotFound(new { message = "Patient not found" });

                    return Ok(new { message = "File attached added successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{patientId}/attachments/{attachmentId}")]
        [Authorize(Roles = "reader")]
        public IActionResult DownloadAttachment(int patientId, int attachmentId)
        {
            var patient = _dbService.GetPatient(patientId);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            var attachment = patient.Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment == null)
                return NotFound(new { message = "Attachment not found" });

            return File(attachment.FileData, attachment.ContentType, attachment.FileName);
        }
    }
}

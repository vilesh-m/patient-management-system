using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Controllers
{
    public class MedicalHistoryRequest
    {
        [Required]
        public string Description { get; set; }
    }
}

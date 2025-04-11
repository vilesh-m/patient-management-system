using System;
using System.Collections.Generic;

namespace PatientAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public List<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
    }
}

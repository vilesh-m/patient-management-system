using System.Linq;
using System.Net.Mail;
using LiteDB;
using PatientSystem.Services.Interfaces;
using PatientSystem.Services.Models;

namespace PatientSystem.Services
{

    public class PatientRepository : IPatientRepository
    {
        private readonly ILiteDatabase _db;
        private readonly string _patientCollection = "patients";
        private readonly ILiteCollection<Patient> _collection;

        public PatientRepository(ILiteDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _collection = _db.GetCollection<Patient>(_patientCollection);

            _collection.EnsureIndex(x => x.Id);
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _collection.FindAll();
        }

        public Patient GetPatient(int id)
        {
            return _collection.FindById(id);
        }

        public int AddPatient(Patient patient)
        {
            return _collection.Insert(patient);
        }

        public bool UpdatePatient(Patient patient)
        {
            return _collection.Update(patient);
        }

        public bool DeletePatient(int id)
        {
            return _collection.Delete(id);
        }

        public bool AddFileAttachment(int patientId, FileAttachment attachment)
        {
            var patient = GetPatient(patientId);
            if (patient == null) return false;

            if (patient.Attachments == null)
                patient.Attachments = new List<FileAttachment>();

            attachment.Id = patient.Attachments.Count > 0 ? patient.Attachments.Max(a => a.Id) + 1 : 1;
            patient.Attachments.Add(attachment);

            return UpdatePatient(patient);
        }

        public IEnumerable<Patient> SearchPatients(string searchText)
        {
            return _collection.Find($"$.Attachments[*].AttachmentContext ANY LIKE '%{searchText}%' " +
                $"or $.FirstName LIKE '%{searchText}%' " +
                $"or $.LastName LIKE '%{searchText}%' " +
                $"or $.MedicalHistory[*].Description ANY LIKE '%{searchText}%'")
                .ToList();
        }

        public bool AddMedicalHistory(int patientId, MedicalHistory history)
        {
            var patient = GetPatient(patientId);
            if (patient == null) return false;

            if (patient.Attachments == null)
                patient.MedicalHistory = new List<MedicalHistory>();

            history.Id = patient.MedicalHistory.Count > 0 ? patient.MedicalHistory.Max(a => a.Id) + 1 : 1;
            patient.MedicalHistory.Add(history);

            return UpdatePatient(patient);
        }
    }
}

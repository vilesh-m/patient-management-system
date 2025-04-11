using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using PatientAPI.Models;

namespace PatientAPI.Services
{
    public class DatabaseService
    {
        private readonly ILiteDatabase _db;
        private readonly string _patientCollection = "patients";

        public DatabaseService(ILiteDatabase db)
        {
            _db = db;
            
            var col = _db.GetCollection<Patient>(_patientCollection);
            
            col.EnsureIndex(x => x.Id);
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _db.GetCollection<Patient>(_patientCollection).FindAll();
        }

        public Patient GetPatient(int id)
        {
            return _db.GetCollection<Patient>(_patientCollection).FindById(id);
        }

        public int AddPatient(Patient patient)
        {
            return _db.GetCollection<Patient>(_patientCollection).Insert(patient);
        }

        public bool UpdatePatient(Patient patient)
        {
            return _db.GetCollection<Patient>(_patientCollection).Update(patient);
        }

        public bool DeletePatient(int id)
        {
            return _db.GetCollection<Patient>(_patientCollection).Delete(id);
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
    }
}

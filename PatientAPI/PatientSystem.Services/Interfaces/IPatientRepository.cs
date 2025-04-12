using PatientSystem.Services.Models;

namespace PatientSystem.Services.Interfaces
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAllPatients();
        IEnumerable<Patient> SearchPatients(string searchText);
        Patient GetPatient(int id);
        int AddPatient(Patient patient);
        bool UpdatePatient(Patient patient);
        bool DeletePatient(int id);
        bool AddFileAttachment(int patientId, FileAttachment attachment);

        bool AddMedicalHistory(int patientId, MedicalHistory history);
    }
}

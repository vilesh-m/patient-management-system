import { useState, useEffect } from 'react';
import { getPatients, addPatient, deletePatient, uploadAttachment, getAttachmentDownloadUrl, searchPatients } from '../service';
import { Patient } from './models/Patient';
import AddPatientModal from './AddPatientModal';
import AddFileUpload from './AddFileUpload';

const PatientCard: React.FC = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isAddPatientModalOpen, setIsAddPatientModalOpen] = useState(false);
  const [selectedPatientId, setSelectedPatientId] = useState<number | null>(null);
  const [isAddAttachmentModalOpen, setIsAddAttachmentModalOpen] = useState(false);
  const [roles, setRoles] = useState<string[]>([]);
  const [searchText, setSetSearchText] = useState<string>('');

  const fetchPatients = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const data = await getPatients();
      setPatients(data);
    } catch (err) {
      setError('Failed to fetch patients. Please check your permissions.');
      console.error('Error fetching patients:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleAddPatient = async (patientData: any) => {
    try {
      await addPatient(patientData);
      setIsAddPatientModalOpen(false);
      fetchPatients();
    } catch (err) {
      console.error('Error adding patient:', err);
      alert('Failed to add patient. Please check your permissions.');
    }
  };

  const handleDeletePatient = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this patient?')) {
      try {
        await deletePatient(id);
        fetchPatients();
      } catch (err) {
        console.error('Error deleting patient:', err);
        alert('Failed to delete patient. Please check your permissions.');
      }
    }
  };

  const handleAddAttachment = async (file: File) => {
    if (!selectedPatientId) return;

    try {
      await uploadAttachment(selectedPatientId, file);
      setIsAddAttachmentModalOpen(false);
      fetchPatients(); // Refresh to get updated attachments
      alert('File attachment added successfully');
    } catch (err) {
      console.error('Error adding attachment:', err);
      alert('Failed to add attachment. Please check your permissions.');
    }
  };

  useEffect(() => {
    const storedRoles = localStorage.getItem('roles');
    if (storedRoles) {
      setRoles(JSON.parse(storedRoles));
    }

    fetchPatients();
  }, []);

  const search = async () => {
    try {
      if (searchText) {
        setIsLoading(true);
        setError(null);
        const data = await searchPatients(searchText);
        setPatients(data);
      }

    } catch (err) {
      setError('Failed to fetch patients. Please check your permissions.');
      console.error('Error fetching patients:', err);
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="card shadow mb-4">
      <div className="card-body">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <input type="text" className="form-control mr-3" value={searchText} onChange={(e) => setSetSearchText(e.target.value)}></input>
          <button type="button" className="btn btn-secondary mr-3" onClick={search}>search</button>
          <button
            onClick={() => setIsAddPatientModalOpen(true)}
            className="btn btn-primary"
          >
            Add Patient
          </button>
        </div>

        {isLoading ? (
          <div className="text-center py-3">Loading patients...</div>
        ) : error ? (
          <div className="text-danger py-3">{error}</div>
        ) : patients.length === 0 ? (
          <div className="text-center py-3 text-secondary">No patients found</div>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover">
              <thead className="table-light">
                <tr>
                  <th>First Name</th>
                  <th>Last Name</th>
                  <th>Gender</th>
                  <th>Attachments</th>
                  <th className="text-center">Actions</th>
                </tr>
              </thead>
              <tbody>
                {patients.map((patient) => (
                  <tr key={patient.id}>
                    <td>{patient.firstName}</td>
                    <td>{patient.lastName}</td>
                    <td>{patient.gender}</td>
                    <td>
                      {patient.attachments?.length > 0 ? (
                        <div className="d-flex flex-column gap-1">
                          {patient.attachments.map(attachment => (
                            <div key={attachment.id} className="d-flex align-items-center">
                              <a
                                onClick={() => getAttachmentDownloadUrl(patient.id, attachment.id)}
                                target="_blank"
                                className="text-primary"
                              >
                                <span className="me-1">{attachment.fileName}</span>
                              </a>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <span className="text-muted">No attachments</span>
                      )}
                    </td>
                    <td className="text-center">
                      <div className="d-flex justify-content-center gap-2">
                        <button
                          onClick={() => {
                            setSelectedPatientId(patient.id);
                            setIsAddAttachmentModalOpen(true);
                          }}
                          className="btn btn-success btn-sm"
                        >
                          {patient.attachments?.length > 0 ? 'Add Attachment' : 'Upload Attachment'}
                        </button>
                        <button
                          onClick={() => handleDeletePatient(patient.id)}
                          className="btn btn-danger btn-sm"
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        <AddPatientModal
          isOpen={isAddPatientModalOpen}
          onClose={() => setIsAddPatientModalOpen(false)}
          onAddPatient={handleAddPatient}
        />



        <AddFileUpload
          isOpen={isAddAttachmentModalOpen}
          onClose={() => setIsAddAttachmentModalOpen(false)}
          onAddAttachment={handleAddAttachment}
        />
      </div>
    </div>
  );
};

export default PatientCard;

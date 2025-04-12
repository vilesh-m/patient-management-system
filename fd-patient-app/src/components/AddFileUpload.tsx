import {
    Button, Modal, ModalFooter,
    ModalHeader, ModalBody
} from "reactstrap"

import { useState } from 'react';

interface AddFileUploadlProps {
    isOpen: boolean;
    onClose: () => void;
    onAddAttachment: (file: File, attachmentContext: string) => void;
}

const AddFileUpload: React.FC<AddFileUploadlProps> = ({ isOpen, onClose, onAddAttachment }) => {
    const [file, setFile] = useState<File | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [attachmentContext, setAttachmentContext] = useState<string>('');

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            const selectedFile = e.target.files[0];
            setFile(selectedFile);
            setError(null);
        }
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (!file) {
            setError('Please select a file to upload');
            return;
        }

        onAddAttachment(file, attachmentContext);
        setFile(null);
    };

    if (!isOpen) return null;


    return (
        <>
            <div className="modal show" style={{ display: 'block', position: 'initial' }} >
                <Modal isOpen={true} toggle={onClose}
                    modalTransition={{ timeout: 200 }}>
                    <ModalHeader toggle={onClose}>
                        Upload a file
                    </ModalHeader>
                    <ModalBody>
                        <form onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label htmlFor="fileUpload" className="form-label">
                                    Attachment Context:
                                </label>
                                <input
                                    id="attachmentContext"
                                    type="text"
                                    value={attachmentContext}
                                    onChange={(e) => setAttachmentContext(e.target.value)}
                                    className={`form-control ${error ? 'is-invalid' : ''}`}
                                />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="fileUpload" className="form-label">
                                    Select a file to upload:
                                </label>
                                <input
                                    id="fileUpload"
                                    type="file"
                                    onChange={handleFileChange}
                                    className="form-control"
                                />

                                {file && (
                                    <div className="mt-2">
                                        <p className="text-success fw-medium">File selected:</p>
                                        <p className="text-dark">{file.name}</p>
                                        <p className="text-secondary small">
                                            {(file.size / 1024).toFixed(2)} KB
                                        </p>
                                    </div>
                                )}
                            </div>

                            {error && (
                                <div className="alert alert-danger">
                                    {error}
                                </div>
                            )}

                            <div className="modal-footer">
                                <button type="button" onClick={onClose} className="btn btn-secondary">
                                    Cancel
                                </button>
                                <button type="submit" disabled={!file} className="btn btn-primary" >
                                    Upload File
                                </button>
                            </div>
                        </form>
                    </ModalBody>
                </Modal>
            </div >
        </>
    );
};

export default AddFileUpload;
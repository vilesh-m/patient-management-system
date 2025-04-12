import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

import { useState } from 'react';

interface AddPatientModalProps {
    isOpen: boolean;
    onClose: () => void;
    onAddPatient: (patientData: any) => void;
}

const AddPatientModal: React.FC<AddPatientModalProps> = ({ isOpen, onClose, onAddPatient }) => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [age, setAge] = useState('');
    const [phone, setPhone] = useState('');
    const [gender, setGender] = useState('');
    const [errors, setErrors] = useState<Record<string, string>>({});

    const validateForm = () => {
        const newErrors: Record<string, string> = {};

        if (!firstName.trim()) newErrors.firstName = 'First name is required';
        if (!age.trim()) {
            newErrors.age = 'Age is required';
        } else if (isNaN(Number(age)) || Number(age) <= 0) {
            newErrors.age = 'Age must be a positive number';
        }
        if (!gender.trim()) newErrors.gender = 'Gender is required';

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = () => {
        if (validateForm()) {
            onAddPatient({
                firstName,
                lastName,
                age: Number(age),
                gender,
                phonenumber: phone
            });

            setFirstName('');
            setLastName('');
            setAge('');
            setGender('');
            setPhone('');
        }

    };
    if (!isOpen)
        return <></>

    return (
        <>
            <div className="modal show" style={{ display: 'block', position: 'initial' }} >
                <Modal.Dialog>
                    <Modal.Header closeButton>
                        <Modal.Title>Modal title</Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="firstName">
                                First Name
                            </label>
                            <input
                                id="firstName"
                                type="text"
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                className={`form-control ${errors.firstName ? 'is-invalid' : ''}`}
                            />
                            {errors.firstName && <div className="invalid-feedback">{errors.firstName}</div>}
                        </div>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="lastName">
                                Last Name
                            </label>
                            <input
                                id="lastName"
                                type="text"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                className={`form-control `}
                            />
                        </div>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="lastName">
                                Age
                            </label>
                            <input
                                id="age"
                                type="number"
                                value={age}
                                onChange={(e) => setAge(e.target.value)}
                                className={`form-control ${errors.age ? 'is-invalid' : ''}`}
                            />
                            {errors.age && <div className="invalid-feedback">{errors.age}</div>}
                        </div>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="gender">
                                Gender
                            </label>
                            <select
                                id="gender"
                                value={gender}
                                onChange={(e) => setGender(e.target.value)}
                                className={`form-select`}>
                                <option value="">Select Gender</option>
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                                <option value="Other">Other</option>
                            </select>
                            {errors.gender && <div className="">{errors.gender}</div>}
                        </div>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="lastName">
                                Phone
                            </label>
                            <input
                                id="age"
                                type="text"
                                value={phone}
                                onChange={(e) => setPhone(e.target.value)}
                                className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                            />
                            {errors.phone && <div className="invalid-feedback">{errors.phone}</div>}
                        </div>
                    </Modal.Body>

                    <Modal.Footer>
                        <Button variant="secondary" onClick={onClose}>Close</Button>
                        <Button variant="primary" onClick={handleSubmit}>Save changes</Button>
                    </Modal.Footer>
                </Modal.Dialog>
            </div >
        </>
    );
};

export default AddPatientModal;
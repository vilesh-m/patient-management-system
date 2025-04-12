// import AddPatientModal from './AddPatientModal';

import { Attachment } from "./Attachment";

// import AddAttachmentModal from './AddAttachmentModal';
export interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  gender: string;
  age: number;
  phoneNumber: string,
  attachments: Attachment[]
}

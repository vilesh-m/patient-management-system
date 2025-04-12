import axios from 'axios'

const API_BASE_URL = "http://localhost:5240/api";

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        "Content-Type": 'application/json'
    },
});

api.interceptors.request.use(
    config => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`
        }
        return config;
    },
    error => {
        Promise.reject(error);
    }
)

export const login = async (username: string) => {
    const response = await api.post('/User/login', { username });
    return response.data;
};


export const addPatient = async (patientData: any) => {
    const response = await api.post('/Patient', patientData);
    return response.data;
};

export const deletePatient = async (id: number) => {
    const response = await api.delete(`/Patient/${id}`);
    return response.data;
};

export const getPatients = async () => {
    const response = await api.get('/Patient');
    return response.data;
};

export const searchPatients = async (searchText: string) => {
    const response = await api.get(`/Patient/search?searchText=${searchText}`);
    return response.data;
};

export const uploadAttachment = async (patientId: number, file: File, attachmentContext: string) => {
    const formdata = new FormData();
    formdata.append('file', file);
    const response = await api.post(`/Patient/${patientId}/attachments?attachmentContext=${attachmentContext}`, formdata, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    return response.data;
}



export const getAttachmentDownloadUrl = async (patientId: number, attachmentId: number) => {
    const response = await api.get(`/Patient/${patientId}/attachments/${attachmentId}`,
        {
            responseType: 'blob'
        }
    );
    const blob = URL.createObjectURL(response.data);
    window.open(blob, '_blank');
    URL.revokeObjectURL(blob);
};

export default api;

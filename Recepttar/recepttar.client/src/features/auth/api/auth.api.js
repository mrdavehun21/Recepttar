import axios from 'axios'

const API_BASE = import.meta.env.VITE_API_URL;

export async function checkEmailApi(email) {
    return axios.get(`${API_BASE}/api/user/checkEmail`, {
        params: { email }
    })
}

export async function loginApi(email, password) {
    const formData = new FormData()
    formData.append('Email', email)
    formData.append('Password', password)

    return axios.post(
        `${API_BASE}/api/user/login`,
        formData, { withCredentials: true }
    )
}

export async function registerApi(name, email, password) {
    const formData = new FormData();
    formData.append('fullName', name);
    formData.append('Email', email)
    formData.append('Password', password)

    return axios.post(`${API_BASE}/api/user/register`, formData);
}

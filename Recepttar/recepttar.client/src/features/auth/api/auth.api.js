import axios from 'axios'

export async function checkEmailApi(email) {
    return axios.get('https://localhost:7035/user/checkEmail', {
        params: { email }
    })
}

export async function loginApi(email, password) {
    const formData = new FormData()
    formData.append('Email', email)
    formData.append('Password', password)

    return axios.post(
        'https://localhost:7035/user/login',
        formData, { withCredentials: true }
    )
}

export async function registerApi(name, email, password) {
    const formData = new FormData();
    formData.append('Name', name);
    formData.append('Email', email)
    formData.append('Password', password)

    return axios.post('https://localhost:7035/user/register', formData);
}

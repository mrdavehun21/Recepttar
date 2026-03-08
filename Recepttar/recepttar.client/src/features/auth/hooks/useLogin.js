import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { checkEmailApi, loginApi } from '../api/auth.api'
import {
    validateEmailInput,
    validatePasswordInput,
    getApiErrorMessage,
    validateEmail,
    validatePassword
} from './authHelper'
import { useAuth } from '../../../GlobalHooks/useAuthContext'

export function useLogin() {
    const [step, setStep] = useState(1)
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const navigate = useNavigate()

    const isEmailValid = validateEmailInput(email);
    const isPasswordValid = validatePasswordInput(password);

    const { refetch } = useAuth();

    useEffect(() => {
        if (!error) return;

        const timer = setTimeout(() => setError(''), 5000)
        return () => clearTimeout(timer)
    }, [error])

    const checkEmail = async () => {
        if (!validateEmail(email, setError)) {
            return
        }

        try {
            await checkEmailApi(email)
            setStep(2)
            setError('')
        } catch (err) {
            setError(getApiErrorMessage(err) || 'No account found.')
        }
    }

    const handleLogin = async () => {
        if (!validatePassword(password, setError)) {
            return
        }

        try {
            await loginApi(email, password)
            await refetch();
            navigate('/', { replace: true })
        } catch (err) {
            setError(getApiErrorMessage(err) || 'Login failed')
        }
    }

    const goBackToEmail = () => {
        setStep(1)
        setPassword('')
        setError('')
    }

    return {
        step,
        email,
        isEmailValid,
        password,
        isPasswordValid,
        error,
        setEmail,
        setPassword,
        checkEmail,
        handleLogin,
        goBackToEmail
    }
}

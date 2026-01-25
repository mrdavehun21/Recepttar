import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { checkEmailApi, loginApi } from '../api/auth.api'

export function useLogin() {
    const [step, setStep] = useState(1)
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const navigate = useNavigate()

    useEffect(() => {
        if (!error) return

        const timer = setTimeout(() => setError(''), 5000)
        return () => clearTimeout(timer)
    }, [error])

    const checkEmail = async () => {
        if (!email) {
            setError('Please enter your email')
            return
        }

        try {
            await checkEmailApi(email)
            setStep(2)
            setError('')
        } catch (err) {
            setError(err.response?.data?.message || 'No account found.')
        }
    }

    const handleLogin = async () => {
        if (!password) {
            setError('Please enter your password')
            return
        }

        try {
            await loginApi(email, password)
            navigate('/dashboard', { replace: true })
        } catch (err) {
            setError(
                err.response?.data?.message ||
                err.response?.data?.error ||
                'Login failed'
            )
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
        password,
        error,
        setEmail,
        setPassword,
        checkEmail,
        handleLogin,
        goBackToEmail
    }
}

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { registerApi, loginApi, checkEmailApi } from '../api/auth.api';
import {
    validateEmailInput,
    validatePasswordInput,
    getApiErrorMessage,
    validateEmail,
    validatePassword
} from './authHelper';

export function useRegister() {
    const [step, setStep] = useState(1);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const isEmailValid = validateEmailInput(email);
    const isPasswordValid = validatePasswordInput(password);

    useEffect(() => {
        if (!error) return;
        const timer = setTimeout(() => setError(''), 5000);
        return () => clearTimeout(timer);
    }, [error]);

    // Email
    const checkEmailExists = async () => {
        if (!name) {
            setError('Please enter your name');
            return;
        }

        if (!validateEmail(email, setError)) {
            return;
        }

        try {
            await checkEmailApi(email);
            setError('Email already registered.');
        } catch (err) {
            if (err.response?.status === 404) {
                setName(name);
                setStep(2);
                setError('');
            } else {
                setError(getApiErrorMessage(err) || 'Failed to check email.');
            }
        }
    };

    // Password
    const handleRegister = async () => {
        if (!validatePassword(password, setError)) {
            return;
        }

        try {
            await registerApi(name, email, password);
            await loginApi(email, password);
            navigate('/', { replace: true });
        } catch (err) {
            setError(getApiErrorMessage(err) || 'Registration failed');
        }
    };

    const goBackToEmail = () => {
        setStep(1);
        setPassword('');
        setError('');
    };

    return {
        step,
        name,
        email,
        isEmailValid,
        password,
        isPasswordValid,
        error,
        setName,
        setEmail,
        setPassword,
        checkEmailExists,
        handleRegister,
        goBackToEmail,
    };
}

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { registerApi, loginApi, checkEmailApi } from '../api/auth.api';
import {
    validateNameInput,
    validateEmailInput,
    validatePasswordInput,
    getApiErrorMessage,
    validateName,
    validateEmail,
    validatePassword
} from './authHelper';
import { useAuth } from '../../../shared/hooks/useAuthContext';

export function useRegister() {
    const [step, setStep] = useState(1);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const isNameValid = validateNameInput(name);
    const isEmailValid = validateEmailInput(email);
    const isPasswordValid = validatePasswordInput(password);

    const { refetch } = useAuth();

    useEffect(() => {
        if (!error) return;
        const timer = setTimeout(() => setError(''), 5000);
        return () => clearTimeout(timer);
    }, [error]);

    // Email
    const checkEmailExists = async () => {
        if (!validateName(name, setError)) {
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
            await refetch();
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
        isNameValid,
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

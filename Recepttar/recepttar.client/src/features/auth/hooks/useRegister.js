import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { registerApi, checkEmailApi } from '../api/auth.api';

export function useRegister() {
    const [step, setStep] = useState(1);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

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

        if (!email) {
            setError('Please enter your email');
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
                setError('Failed to check email.');
            }
        }

    };

    // Password
    const handleRegister = async () => {
        if (!password) {
            setError('Please enter your password');
            return;
        }

        try {
            await registerApi(name, email, password);
            navigate('/', { replace: true });
        } catch (err) {
            setError(
                err.response?.data?.message ||
                err.response?.data?.error ||
                'Registration failed'
            );
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
        password,
        error,
        setName,
        setEmail,
        setPassword,
        checkEmailExists,
        handleRegister,
        goBackToEmail,
    };
}

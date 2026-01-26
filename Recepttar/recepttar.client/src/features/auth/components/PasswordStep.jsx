import { useEffect } from 'react';
import * as bootstrap from 'bootstrap';

export default function PasswordStep({
    email,
    password,
    isPasswordValid,
    onPasswordChange,
    onSubmit,
    onBack,
    onKeyDown,
    theme = 'login'
}) {
    useEffect(() => {
        const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        tooltipTriggerList.forEach(el => new bootstrap.Tooltip(el));
    }, []);

    const primaryClass = theme === 'login' ? 'login-btn-primary' : 'register-btn-primary';
    const accountText = theme === 'login' ? 'Log In' : 'Create an Account';
    
    return (
        <>
            <div className="d-flex align-items-center mb-4 border-bottom pb-3">
                <button
                    type="button"
                    className="btn me-3"
                    onClick={onBack}
                >
                    <i className="bi bi-arrow-left"></i>
                </button>
                <strong>{email}</strong>
            </div>

            <div className="">
                <label className="form-label fw-semibold">Password
                    <i
                        className="bi bi-question-circle ms-2"
                        data-bs-toggle="tooltip"
                        data-bs-placement="right"
                        title="Password requirements: minimum 8 characters and must include a number"
                    />
                </label>
                <input
                    type="password"
                    className="form-control form-control-lg"
                    value={password}
                    onChange={onPasswordChange}
                    onKeyDown={onKeyDown}
                    autoFocus
                />
            </div>

            <button
                type="button"
                className={`btn ${primaryClass} btn-lg w-100`}
                onClick={onSubmit}
                disabled={!isPasswordValid}
            >
                {accountText}
            </button>
        </>
    )
}

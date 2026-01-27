import { useEffect } from 'react';
import * as bootstrap from 'bootstrap';

export default function EmailStep({
    isNameValid,
    email,
    isEmailValid,
    onEmailChange,
    onContinue,
    onKeyDown,
    onSignUp,
    onLogin,
    onDiscover,
    theme = 'login'
}) {
    useEffect(() => {
        const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        tooltipTriggerList.forEach(el => new bootstrap.Tooltip(el));
    }, []);

    const primaryClass = theme === 'login' ? 'login-btn-primary' : 'register-btn-primary';
    const outlineClass = theme === 'login' ? 'login-btn-outline' : 'register-btn-outline';
    const accountText = theme === 'login' ? 'Create an Account' : 'Log In';
    const accountOnclick = theme === 'login' ? onSignUp : onLogin;

    const shouldDisable = theme === 'login'
        ? !isEmailValid
        : !isEmailValid || !isNameValid;

    return (
        <>
            <div className="mb-4 mt-2">
                <label className="form-label fw-semibold">Email Address
                    <i
                        className="bi bi-question-circle ms-2"
                        data-bs-toggle="tooltip"
                        data-bs-placement="right"
                        title="Please use the following pattern: info@recepttar.hu"
                    />
                </label>
                <input
                    type="email"
                    className="form-control form-control-lg"
                    value={email}
                    onChange={onEmailChange}
                    onKeyDown={onKeyDown}
                    autoFocus
                />
            </div>

            <button
                type="button"
                className={`btn ${primaryClass} btn-lg w-100`}
                onClick={onContinue}
                disabled={shouldDisable}
            >
                Continue
            </button>

            <div className="d-flex align-items-center my-4">
                <hr className="flex-grow-1" />
                <span className="px-3 text-muted">or</span>
                <hr className="flex-grow-1" />
            </div>

            <button
                type="button"
                className={`btn ${outlineClass} btn-lg w-100 mb-4`}
                onClick={accountOnclick}
            >
                { accountText }
            </button>

            <button
                type="button"
                className={`btn ${outlineClass} btn-lg w-100`}
                onClick={onDiscover}
            >
                Discover recipes
            </button>
        </>
    )
}

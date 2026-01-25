export default function PasswordStep({
    email,
    password,
    onPasswordChange,
    onSubmit,
    onBack,
    onKeyDown,
    theme = 'login'
}) {
    const primaryClass = theme === 'login' ? 'login-btn-primary' : 'register-btn-primary';

    return (
        <div className="mt-4">
            <div className="d-flex align-items-center mb-4 border-bottom pb-3">
                <button
                    type="button"
                    className="btn me-3"
                    onClick={onBack}
                >
                    <i class="bi bi-arrow-left"></i>
                </button>
                <strong>{email}</strong>
            </div>

            <div className="mb-4">
                <label className="form-label fw-semibold">Password</label>
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
                disabled={!password}
            >
                Sign In
            </button>
        </div>
    )
}

export default function EmailStep({
    email,
    onEmailChange,
    onContinue,
    onKeyDown,
    onSignUp,
    onLogin,
    onDiscover,
    theme = 'login'
}) {
    const primaryClass = theme === 'login' ? 'login-btn-primary' : 'register-btn-primary';
    const outlineClass = theme === 'login' ? 'login-btn-outline' : 'register-btn-outline';
    const accountText = theme === 'login' ? 'Create an Account' : 'Log In';
    const accountOnclick = theme === 'login' ? onSignUp : onLogin;

    return (
        <div className="mt-4">
            <div className="mb-4">
                <label className="form-label fw-semibold">Email Address</label>
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
                disabled={!email}
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
                //style={{ color: '#f27127', borderColor: '#f27127' }}
            >
                { accountText }
            </button>

            <button
                type="button"
                className={`btn ${outlineClass} btn-lg w-100`}
                onClick={onDiscover}
                //style={{ backgroundColor: '#f27127', borderColor: '#f27127' }}
            >
                Discover recipes
            </button>
        </div>
    )
}

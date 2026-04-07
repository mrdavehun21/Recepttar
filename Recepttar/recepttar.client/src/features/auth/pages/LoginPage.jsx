import './LoginPage.css'
import loginImage from '../../../assets/auth-background.jpg'
import { useLogin } from '../hooks/useLogin'
import EmailStep from '../components/EmailStep'
import PasswordStep from '../components/PasswordStep'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next';

export default function LoginPage() {
    const navigate = useNavigate()
    const {
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
    } = useLogin()

    const { t } = useTranslation();

    const handleKeyDown = (e) => {
        if (e.key === 'Enter') {
            step === 1 ? checkEmail() : handleLogin()
        }
    }

    return (
        <div className="container-fluid vh-100 p-0">
            <div className="row g-0 h-100">
                <div className="col-md-6 d-none d-md-block">
                    <img src={loginImage} alt="" className="login-image" />
                </div>

                <div className="col-md-6 d-flex align-items-center justify-content-center bg-light">
                    <div className="login-container p-4 p-lg-2">
                        <h1 className="h2 fw-bold text-dark">{t("loginPage.signIn")}</h1>

                        <div style={{ minHeight: '60px' }}>
                            {error && (
                                <div className="alert alert-danger fade show">
                                    {error}
                                </div>
                            )}
                        </div>

                        {step === 1 && (
                            <EmailStep
                                email={email}
                                isEmailValid={isEmailValid}
                                onEmailChange={(e) => setEmail(e.target.value)}
                                onContinue={checkEmail}
                                onKeyDown={handleKeyDown}
                                onLogin={() => navigate('/login')}
                                onSignUp={() => navigate('/register')}
                                onDiscover={() => navigate('/')}
                                theme="login"
                                t={t}
                            />
                        )}

                        {step === 2 && (
                            <PasswordStep
                                email={email}
                                password={password}
                                isPasswordValid={isPasswordValid}
                                onPasswordChange={(e) => setPassword(e.target.value)}
                                onSubmit={handleLogin}
                                onBack={goBackToEmail}
                                onKeyDown={handleKeyDown}
                                theme="login"
                                t={t}
                            />
                        )}

                        <div className="mt-3 text-center text-muted">
                            <small>&copy; 2026 Recepttár</small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

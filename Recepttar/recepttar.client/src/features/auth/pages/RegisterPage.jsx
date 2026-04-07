import './RegisterPage.css';
import registerImage from '../../../assets/auth-background.jpg';
import { useRegister } from '../hooks/useRegister';
import NameSet from '../components/NameSet'
import EmailStep from '../components/EmailStep';
import PasswordStep from '../components/PasswordStep';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function RegisterPage() {
    const navigate = useNavigate();
    const {
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
        goBackToEmail
    } = useRegister();

    const { t } = useTranslation();

    const handleKeyDown = (e) => {
        if (e.key === 'Enter') {
            step === 1 ? checkEmailExists() : handleRegister();
        }
    };

    return (
        <div className="container-fluid vh-100 p-0">
            <div className="row g-0 h-100">
                <div className="col-md-6 d-none d-md-block">
                    <img src={registerImage} alt="" className="register-image" />
                </div>

                <div className="col-md-6 d-flex align-items-center justify-content-center bg-light">
                    <div className="register-container p-4 p-lg-5">
                        <h1 className="h2 fw-bold mb-2 text-dark">{t("registerPage.register")}</h1>

                        {error && (
                            <div className="alert alert-danger fade show">
                                {error}
                            </div>
                        )}

                        {step === 1 && (
                            <>
                                <NameSet name={name} setName={setName} t={t} />
                                <EmailStep
                                    isNameValid={isNameValid}
                                    email={email}
                                    isEmailValid={isEmailValid}
                                    onEmailChange={(e) => setEmail(e.target.value)}
                                    onContinue={checkEmailExists}
                                    onKeyDown={handleKeyDown}
                                    onSignUp={() => navigate('/register')}
                                    onLogin={() => navigate('/login') }
                                    onDiscover={() => navigate('/')}
                                    theme="register"
                                    t={t}
                                />
                            </>
                        )}

                        {step === 2 && (
                            <PasswordStep
                                email={email}
                                password={password}
                                isPasswordValid={isPasswordValid}
                                onPasswordChange={(e) => setPassword(e.target.value)}
                                onSubmit={handleRegister}
                                onBack={goBackToEmail}
                                onKeyDown={handleKeyDown}
                                theme="register"
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
    );
}

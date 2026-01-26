export const EMAIL_REGEX = /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/;
export const PASSWORD_REGEX = /^(?=.*[A-Za-z])(?=.*\d).{8,}$/;

export const validateEmailInput = (email) => EMAIL_REGEX.test(email.trim());
export const validatePasswordInput = (password) => PASSWORD_REGEX.test(password.trim());

export const getApiErrorMessage = (err) => {
    return err.response?.data?.message ||
        err.response?.data?.error ||
        'An error occurred';
};

export const validateEmail = (email, setError) => {
    if (!email) {
        setError('Please enter your email');
        return false;
    }

    if (!validateEmailInput(email)) {
        setError('Please enter a valid email');
        return false;
    }

    return true;
};

export const validatePassword = (password, setError) => {
    if (!password) {
        setError('Please enter your password');
        return false;
    }

    if (!validatePasswordInput(password)) {
        setError('Please enter a valid password');
        return false;
    }

    return true;
};
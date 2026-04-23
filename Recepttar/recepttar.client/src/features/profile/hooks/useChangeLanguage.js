import { useTranslation } from 'react-i18next';

function useChangeLanguage() {
    const { i18n } = useTranslation();

    const changeLanguage = (lng) => {
        i18n.changeLanguage(lng);
    };

    return { changeLanguage };
}

export default useChangeLanguage;
import { useEffect, useState } from "react";
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuthContext';
import { ImageAvailable } from '../../hooks/usePictureChecker';
import { submitLogoutRequest } from '../../api/recipe.api';
import { useTranslation } from 'react-i18next';
import Dropdown from 'react-bootstrap/Dropdown';
import Logo from '../../../assets/Logo.png';
import './Navbar.css';

export default function NavbarComponent() {
    const { isLoggedIn, profileData } = useAuth();
    const [imageExists, setImageExists] = useState(false);

    const API_BASE = import.meta.env.VITE_API_URL;

    const { t } = useTranslation();

    async function handleLogout() {
        try {
            await submitLogoutRequest();
            window.location.href = '/login';
        } catch (err) {
            if (err?.response?.status === 401) {
                console.warn('Already logged out');
            } else {
                console.error('Logout failed', err);
            }
        }
    }

    useEffect(() => {
        async function checkImage() {
            const exists = await ImageAvailable(
                `${API_BASE}/${profileData?.profilePicture}`
            );
            setImageExists(exists);
        }
        if (profileData?.profilePicture != undefined) {
            checkImage();
        }
    }, [profileData?.profilePicture]);

    return (
        <nav className="w-100 main-green d-flex justify-content-between align-items-center pt-2 pb-2 navbar">
            <div className="d-none d-sm-block">
                <Link to="/home"><img className="h-75px" src={Logo} alt="" /></Link>
                <Link to="/leaderboard" className="fs-4 ms-3 font-neutral-100 fw-bold">{t("navbar.leaderboard")}</Link>
                <Link to="/polls" className="fs-4 ms-3 font-neutral-100 fw-bold">{t("navbar.polls")}</Link>
            </div>
            <div className="d-block d-sm-none">
            <Dropdown align="end" className="ms-3 rounded-2 dropdown">
                <Dropdown.Toggle id="profile-dropdown" className="d-flex align-items-center p-0 bg-transparent border-0 text-black fs-1 shadow-none" bsPrefix="no-caret" style={{height: "40px"}}>
                    <span className="d-block">&#9776;</span>
                </Dropdown.Toggle>

                <Dropdown.Menu className="mt-1">
                    <Dropdown.Item href="/home">{t("navbar.home")}</Dropdown.Item>
                    <Dropdown.Item href="/leaderboard">{t("navbar.leaderboard")}</Dropdown.Item>
                    <Dropdown.Item href="/polls">{t("navbar.polls")}</Dropdown.Item>
                </Dropdown.Menu>
            </Dropdown>
            </div>
            <Dropdown align="end" className="bg-light shadow me-3 rounded-2 dropdown">
                <Dropdown.Toggle id="profile-dropdown" className="d-flex align-items-center gap-1 bg-light border-0 text-black">
                    <>
                        {isLoggedIn && imageExists ? (
                            <img
                                src={`${API_BASE}/${profileData.profilePicture}`}
                                className="profile-dimensions rounded-circle"
                            />
                        ) : (
                            <i className="bi bi-person-circle fs-2"></i>
                        )}

                        <span className="fw-semibold d-none d-sm-block">
                            {isLoggedIn ? profileData.fullName : t("navbar.profile")}
                        </span>
                    </>
                </Dropdown.Toggle>

                <Dropdown.Menu className="mt-1">
                    {isLoggedIn ? (
                        <>
                            <Dropdown.Item href="/mycollection#favorites">{t("navbar.myFavorites")}</Dropdown.Item>
                            <Dropdown.Item href="/mycollection#polls">{t("navbar.myPolls")}</Dropdown.Item>
                            <Dropdown.Item href="/mycollection#recipes">{t("navbar.myRecipes")}</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item href="/recipe/createrecipe">{t("navbar.createRecipe")}</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item href="/profile">{t("navbar.myProfile")}</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item onClick={handleLogout} className="text-danger">{t("navbar.logOut")}</Dropdown.Item>
                        </>
                    ) : (
                        <Dropdown.Item href="/login">{t("navbar.logIn")}</Dropdown.Item>
                    )}
                </Dropdown.Menu>
            </Dropdown>
        </nav>
    )
}
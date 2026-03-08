import { useEffect, useState } from "react";
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import Dropdown from 'react-bootstrap/Dropdown';
import Image from 'react-bootstrap/Image';
import Logo from '../../assets/Logo.png';
import './Navbar.css';
import { useAuth } from '../../GlobalHooks/useAuthContext';
import { submitLogoutRequest } from '../../GlobalApi/recipe.api';
import { ImageAvailable } from '../../GlobalHooks/usePictureChecker';

function NavbarComponent() {
    const { isLoggedIn, profileData } = useAuth();
    const [imageExists, setImageExists] = useState(false);

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
                `https://localhost:7035/${profileData?.profilePicture}`
            );
            setImageExists(exists);
        }
        if (profileData?.profilePicture != undefined) {
            checkImage();
        }
    }, [profileData?.profilePicture]);

    return (
        <Navbar variant="dark" expand="sm" className="px-3 justify-content-between main-green Titlebar">
            <div className="d-flex align-items-center">
                <Navbar.Brand href="/home" className="d-none d-sm-block">
                    <img src={Logo} alt="Logo" className="Logo" />
                </Navbar.Brand>

                <Nav className="gap-3 d-flex flex-row">
                    <Nav.Link href="/myRecipes" className="fs-4 navbar-link font-neutral-100">My recipes</Nav.Link>
                    <Nav.Link href="/polls" className="fs-4 navbar-link font-neutral-100">Polls</Nav.Link>
                </Nav>
            </div>

            <Dropdown align="end">
                <Dropdown.Toggle
                    as={Nav.Link}
                    id="profile-dropdown"
                    className="d-flex align-items-center gap-2"
                >
                    {isLoggedIn && imageExists ? (
                        <>
                            <Image
                                src={`https://localhost:7035/${profileData.profilePicture}`}
                                roundedCircle
                                width={36}
                                height={36}
                                style={{ objectFit: 'cover' }}
                            />
                            <span className="fw-semibold d-none d-sm-block">
                                {profileData.name}
                            </span>
                        </>
                    ) : (
                        <>
                            <i className="bi bi-person-circle fs-2"></i>
                            <span className="fw-semibold d-none d-sm-block">Profile</span>
                        </>
                    )}
                </Dropdown.Toggle>

                <Dropdown.Menu className="mt-1">
                    {isLoggedIn ? (
                        <>
                            <Dropdown.Item href="/profile">Profile</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item onClick={handleLogout} className="text-danger">
                                Logout
                            </Dropdown.Item>
                        </>
                    ) : (
                        <Dropdown.Item href="/login">Login</Dropdown.Item>
                    )}
                </Dropdown.Menu>
            </Dropdown>
        </Navbar>

    );
}

export default NavbarComponent;
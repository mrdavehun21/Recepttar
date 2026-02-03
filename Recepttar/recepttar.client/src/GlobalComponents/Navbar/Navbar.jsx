import { useEffect } from "react";
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import Dropdown from 'react-bootstrap/Dropdown';
import Image from 'react-bootstrap/Image';
import Logo from '../../assets/Logo.png';
import './Navbar.css';
import { useIsLoggedIn } from '../../GlobalHooks/useLoginState'
import { submitLogoutRequest } from '../../GlobalApi/recipe.api';

function NavbarComponent() {
    const { isLoggedIn, profileData } = useIsLoggedIn();

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
        const el = document.getElementById('profileDropdown');
        if (!el) return;

        const dropdown = new bootstrap.Dropdown(el);

        return () => dropdown.dispose();
    }, [isLoggedIn]);


    return (
        <Navbar bg="success" expand="sm" className="px-2">
            <Navbar.Brand href="/">
                <img src={Logo} alt="Logo" className="Logo" />
            </Navbar.Brand>

            <Nav className="ms-auto">
                <Dropdown align="end">
                    <Dropdown.Toggle
                        variant="success"
                        id="profile-dropdown"
                        className="d-flex align-items-center gap-2"
                    >
                        {isLoggedIn ? (
                            <>
                                <Image
                                    src={`https://localhost:7035${profileData.profilePicture}`}
                                    roundedCircle
                                    width={36}
                                    height={36}
                                    style={{ objectFit: 'cover' }}
                                />
                                <span className="fw-semibold text-light">{profileData.name}</span>
                            </>
                        ) : (
                            <>
                                <div
                                    className="bg-secondary rounded-circle"
                                    style={{ width: 36, height: 36 }}
                                />
                                <span className="fw-semibold text-light">Profile</span>
                            </>
                        )}
                    </Dropdown.Toggle>

                    <Dropdown.Menu>
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
            </Nav>
        </Navbar>
    );
}

export default NavbarComponent;
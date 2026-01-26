import { useState, useEffect } from "react";
import Logo from '../../../../assets/Logo.png';
import './Titlebar.css';
import { useLoginStatus } from '../../hooks/useLoginState'

function Titlebar({ onSearch }) {
    const [search, setSearch] = useState("");
    const { user } = useLoginStatus();

    useEffect(() => {
        if (!search.trim()) return;

        const timeout = setTimeout(() => {
            onSearch(search);
        }, 300);

        return () => clearTimeout(timeout);
    }, [search, onSearch]);

    return (
        <nav className="navbar navbar-expand-sm navbar-light bg-success px-2">
            <img src={Logo} alt="Logo" className="Logo" />

            <button

                className="navbar-toggler ms-auto"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#navbarContent"
                aria-controls="navbarContent"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
                <span className="navbar-toggler-icon"></span>
            </button>

            <div className="collapse navbar-collapse" id="navbarContent">
                <div className="d-flex my-2 my-lg-0 me-lg-3" style={{ height: "45px" }}>
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Search"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                    <button
                        className="btn bg-light bg-primary ms-1"
                        onClick={() => onSearch(search)}
                    >
                        Search
                    </button>
                </div>
                {
                    user.isLoggedIn == false ? (
                        <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <a href="/login" className="nav-link text-light">Log in</a>
                            </li>
                        </ul>
                    ) : (
                        <div className="ProfileBox navbar-nav ms-auto mb-2 mb-lg-0 bg-light rounded-1 d-flex align-items-center gap-2 p-2">
                            <img src={"https://localhost:7035" + user.profilePicture} className="rounded-1 ProfileBoxImage" />
                            <span className="fw-bold d-block">{user.name}</span>
                        </div>
                    )
                }
            </div>
        </nav>
    );
}

export default Titlebar;
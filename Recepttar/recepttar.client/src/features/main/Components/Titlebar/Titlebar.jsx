import { useState, useEffect } from "react";
import Logo from '../../../../assets/Logo.png';
import './Titlebar.css';

function Titlebar({ onSearch }) {
    const [search, setSearch] = useState("");

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
                <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
                    <li className="nav-item">
                        <a href="#" className="nav-link text-light">Log in</a>
                    </li>
                </ul>
            </div>
        </nav>
    );
}

export default Titlebar;
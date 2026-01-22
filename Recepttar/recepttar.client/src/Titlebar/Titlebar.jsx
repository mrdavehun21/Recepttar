import Logo from '../assets/Logo.png';
import './Titlebar.css';

function Titlebar() {
    return (
        <div className="w-100 p-2 Titlebar d-flex justify-content-between align-items-center">
            <img src={Logo} alt="Logo" className="Logo" />
            <ul className="d-flex gap-3 list-unstyled">
                <li>Recepies</li>
                <li>Polls</li>
                <li>About</li>
            </ul>
            <div className="h-75 d-flex align-items-center bg-light rounded-3 text-dark">
                <img src="https://localhost:7035/user/profile/profilepicture/1" className="h-50 m-2 rounded-5" />
                <span className="d-block me-2">Cat007</span>
            </div>
        </div>
    );
}

export default Titlebar;
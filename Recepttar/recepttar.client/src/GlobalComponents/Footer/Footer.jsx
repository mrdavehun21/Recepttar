import Logo from '../../assets/Logo.png';
import './Footer.css'

function Footer() {
    return (
        <div className="w-100 d-flex align-items-center gap-2 text-light main-green">
            <img src={Logo} className="Logo" />
            <h2>Recepttár</h2>
        </div>
  );
}

export default Footer;
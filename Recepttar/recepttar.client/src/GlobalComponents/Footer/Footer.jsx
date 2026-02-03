import Logo from '../../assets/Logo.png';
import './Footer.css'

function Footer() {
    return (
        <div className="w-100 bg-success d-flex align-items-center gap-2 text-light">
            <img src={Logo} className="Logo" />
            <h2>Recepttar</h2>
        </div>
  );
}

export default Footer;
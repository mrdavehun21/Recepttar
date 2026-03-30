import { useLocation } from 'react-router-dom';
import Navbar from '../shared/components/Navbar/Navbar';
import Footer from '../shared/components/Footer/Footer';

function Layout({ children }) {
    const location = useLocation();
    const hideNavAndFooter = ['/login', '/register'].includes(location.pathname);

    return (
        <div className="app-container">
            {!hideNavAndFooter && <Navbar />}
            <main className="content">
                {children}
            </main>
            {!hideNavAndFooter && <Footer />}
        </div>
    );
}

export default Layout;
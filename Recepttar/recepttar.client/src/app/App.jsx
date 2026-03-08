import { AuthProvider } from '../GlobalHooks/useAuthContext';
import AppRouter from './router';
import Navbar from '../GlobalComponents/Navbar/Navbar';
import Footer from '../GlobalComponents/Footer/Footer';

export default function App() {
    return (
        <AuthProvider>
            <div className="app-container">
                <Navbar />
                <main className="content">
                    <AppRouter />
                </main>
                <Footer />
            </div>
        </AuthProvider>
    )
}
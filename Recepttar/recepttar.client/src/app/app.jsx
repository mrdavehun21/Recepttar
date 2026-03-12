import { AuthProvider } from '../shared/hooks/useAuthContext';
import AppRouter from './router';
import Navbar from '../shared/components/Navbar/Navbar';
import Footer from '../shared/components/Footer/Footer';

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
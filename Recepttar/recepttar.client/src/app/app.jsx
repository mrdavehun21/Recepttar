import { AuthProvider } from '../shared/hooks/useAuthContext';
import AppRouter from './router';
import Layout from './layout';

export default function App() {
    return (
        <AuthProvider>
            <Layout>
                <AppRouter />
            </Layout>
        </AuthProvider>
    )
}
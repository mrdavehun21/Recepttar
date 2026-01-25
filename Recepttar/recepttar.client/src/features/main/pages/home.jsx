import Titlebar from '../Components/Titlebar/Titlebar';
import Recipes from './App.jsx';
import PollApp from '../Components/PollSection/PollApp';
import SearchBottom from '../Components/SearchBottom/SearchApp';
import Footer from '../Components/Footer/Footer';
import { useRecipes } from '../hooks/useRecipes';

import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';

function Home() {
    const {
        recipes,
        loading,
        error,
        fetchRecipes
    } = useRecipes();

    return (
        <>
            <Titlebar onSearch={fetchRecipes} />

            {loading && <p className="text-center mt-4">Loading...</p>}
            {error && <p className="text-danger text-center">{error}</p>}

            {!loading && !error && <Recipes recipes={recipes} />}

            <PollApp />
            <SearchBottom />
            <Footer />
        </>
    );
}

export default Home;
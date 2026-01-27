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
        error,
        fetchRecipes
    } = useRecipes();

    return (
        <>
            <Titlebar onSearch={fetchRecipes} />

            {error && <p className="text-danger text-center">{error}</p>}

            {!error && <Recipes recipes={recipes} />}

            <PollApp />
            <SearchBottom />
            <Footer />
        </>
    );
}

export default Home;
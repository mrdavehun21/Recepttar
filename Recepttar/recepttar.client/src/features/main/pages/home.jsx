import Navbar from '../../../GlobalComponents/Navbar/Navbar';
import Recipes from './App.jsx';
import PollApp from '../Components/PollSection/PollApp';
import SearchBottom from '../Components/SearchBottom/SearchApp';
import Footer from '../../../GlobalComponents/Footer/Footer';
import { useRecipes } from '../hooks/useRecipes';
import ErrorBox from '../../../GlobalComponents/ErrorBox/ErrorBox';

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
            <Navbar onSearch={fetchRecipes} />

            <SearchBottom />

            {error.errorCode &&
                <ErrorBox
                    errorCode={error.errorCode}
                    errorMessage={error.errorMessage}
                />
            }

            {!error.errorCode && <Recipes recipes={recipes} />}

            <PollApp />
            <Footer />
        </>
    );
}

export default Home;
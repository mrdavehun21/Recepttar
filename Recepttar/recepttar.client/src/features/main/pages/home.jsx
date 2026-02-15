import Navbar from '../../../GlobalComponents/Navbar/Navbar';
import Recipes from '../Components/RecipeList/RecipeListComponent.jsx';
import PollApp from '../Components/PollSection/PollApp';
import SearchBottom from '../Components/SearchParentComponent/SearchParentComponent';
import Footer from '../../../GlobalComponents/Footer/Footer';
import { useRecipes } from '../hooks/useRecipes';
import ErrorBox from '../../../GlobalComponents/ErrorBox/ErrorBox';
import { useState, useEffect } from 'react';

import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
import { useIsLoggedIn } from '../../../GlobalHooks/useLoginState';

function Home() {
    const { recipes, error, fetchRecipes } = useRecipes();

    const [selectedTags, setSelectedTags] = useState([]);
    const [search, setSearch] = useState("");
    const [selectedIngredients, setSelectedIngredients] = useState([]);

    useEffect(() => {
        fetchRecipes([
            selectedTags,
            selectedIngredients,
            search
        ])
    }, [selectedTags, selectedIngredients, search]);

    const filteredRecipes = recipes.filter((recipe) => {
        if (
            selectedTags.length &&
            !selectedTags.every((tag) => recipe.tags?.includes(tag))
        ) {
            return false;
        }

        if (
            selectedIngredients.length &&
            !selectedIngredients.every((ing) =>
                recipe.ingredients?.includes(ing.name)
            )
        ) {
            return false;
        }

        return true;
    });

    const { isLoggedIn, profileData } = useIsLoggedIn();

    return (
        <>
            <Navbar />

            <div className="d-flex flex-column flex-xxl-row align-items-start justify-content-between w-100 gap-2">
                <div className="d-flex flex-column w-100 h-100 justify-content-center align-items-center">
                    <SearchBottom
                        selectedTags={selectedTags}
                        setSelectedTags={setSelectedTags}
                        selectedIngredients={selectedIngredients}
                        setSelectedIngredients={setSelectedIngredients}
                        setSearch={setSearch}
                    />

                    {!error.errorCode && (
                        <Recipes recipes={recipes} />
                    )}
                </div>

                <PollApp loginStatus={isLoggedIn} />
            </div>

            <Footer />
        </>
    );
}

export default Home;
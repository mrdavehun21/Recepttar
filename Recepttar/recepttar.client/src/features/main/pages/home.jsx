import { useRecipes } from '../hooks/useRecipes';
import { useState, useEffect } from 'react';
import Recipes from '../Components/RecipeList/RecipeListComponent.jsx';
import PollApp from '../Components/PollSection/PollApp';
import SearchBottom from '../Components/SearchParentComponent/SearchParentComponent';

import './index.css';

function Home({ isLoggedIn, profileID }) {
    const { recipes, error, fetchRecipes } = useRecipes();

    const [selectedTags, setSelectedTags] = useState([]);
    const [search, setSearch] = useState("");
    const [selectedIngredients, setSelectedIngredients] = useState([]);

    useEffect(() => {
        const hasFilters = selectedTags.length > 0 || selectedIngredients.length > 0 || search.trim() !== "";

        if (hasFilters) {
            fetchRecipes([selectedTags, selectedIngredients, search]);
        } else {
            fetchRecipes();
        }
    }, [selectedTags, selectedIngredients, search]);

    return (
        <>
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

                <PollApp loginStatus={isLoggedIn} profileID={profileID}/>
            </div>
        </>
    );
}

export default Home;
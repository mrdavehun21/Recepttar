import { useRecipes } from '../hooks/useRecipes';
import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import Recipes from '../components/recipe-list/RecipeListComponent.jsx';
import PollApp from '../components/poll-section/PollApp.jsx';
import SearchBottom from '../components/search-parent-component/SearchParentComponent.jsx';

import './index.css';

function Home({ isLoggedIn, profileID }) {
    const { recipes, error, fetchRecipes } = useRecipes();

    const [selectedTags, setSelectedTags] = useState([]);
    const [search, setSearch] = useState("");
    const [selectedIngredients, setSelectedIngredients] = useState([]);

    const { t } = useTranslation();

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
                        t={t}
                    />

                    {!error.errorCode && (
                        <Recipes recipes={recipes} loginStatus={isLoggedIn} t={t} />
                    )}
                </div>

                <PollApp loginStatus={isLoggedIn} profileID={profileID} t={t}/>
            </div>
        </>
    );
}

export default Home;
import { useState, useEffect, useCallback } from 'react';
import { searchRecipes, getRecipeReviews, getAllRecipes } from '../api/main.api';

export function useRecipes() {
    const [recipes, setRecipes] = useState([]);
    const [error, setError] = useState({errorCode: null, errorMessage: null});

    const fetchRecipes = useCallback(async (query = '') => {
        setError({ errorCode: null, errorMessage: null });

        try {
            let data = null;
            if (query == '') {
                data = await getAllRecipes();
            }
            else {
                data = await searchRecipes(query);
            }

            setRecipes(data);
        } catch (err) {
            setError({ errorCode: err.status, errorMessage: 'Something went wrong' });
        }
    }, []);

    return {
        recipes,
        error,
        fetchRecipes
    };
}

import { useState, useEffect, useCallback } from 'react';
import { searchRecipes, getRecipeReviews } from '../api/recipe.api';

export function useRecipes() {
    const [recipes, setRecipes] = useState([]);
    const [error, setError] = useState({errorCode: null, errorMessage: null});

    const fetchRecipes = useCallback(async (query = '', type = '') => {
        setError({ errorCode: null, errorMessage: null });

        try {
            const data = await searchRecipes(query, type);

            const dataWithRatings = await Promise.all(
                data.map(async (recipe) => {
                    const reviews = await getRecipeReviews(recipe.id);
                    const avg =
                        reviews.length > 0
                            ? reviews.reduce((a, b) => a + b.stars, 0) / reviews.length
                            : 0;

                    return {
                        ...recipe,
                        averageRating: avg,
                        reviewCount: reviews.length
                    };
                })
            );

            setRecipes(dataWithRatings);
        } catch (err) {
            setError({ errorCode: err.status, errorMessage: 'Something went wrong' });
        }
    }, []);

    useEffect(() => {
        fetchRecipes();
    }, [fetchRecipes]);

    return {
        recipes,
        error,
        fetchRecipes
    };
}

import { useState, useEffect, useCallback } from 'react';
import { searchRecipes, getRecipeReviews } from '../api/recipe.api';

export function useRecipes() {
    const [recipes, setRecipes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchRecipes = useCallback(async (query = '', type = '') => {
        setLoading(true);
        setError(null);

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
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchRecipes();
    }, [fetchRecipes]);

    return {
        recipes,
        loading,
        error,
        fetchRecipes
    };
}

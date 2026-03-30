import { useState, useEffect, useCallback } from 'react';
import { getRecipeById, getUserById, getReviewsByRecipeId } from '../api/recipe.api';

const useRecipe = (recipeId) => {
    const [recipe, setRecipe] = useState(null);
    const [ingredients, setIngredients] = useState([]);
    const [reviews, setReviews] = useState([]);
    const [author, setAuthor] = useState(null);
    const [error, setError] = useState(null);

    const refetchReviews = useCallback(async () => {
        if (!recipeId) return;
        try {
            const recipeReviewResponse = await getReviewsByRecipeId(recipeId);
            setReviews(recipeReviewResponse.data);
        } catch {
            setError('Failed to fetch reviews. Please try again.');
        }
    }, [recipeId]);

    useEffect(() => {
        const fetchRecipeAndAuthor = async () => {
            if (!recipeId) {
                setError('No recipe ID provided');
                return;
            }
            try {
                const recipeResponse = await getRecipeById(recipeId);
                setRecipe(recipeResponse.data);
                setIngredients(recipeResponse.data.ingredients ?? []);
                const recipeReviewResponse = await getReviewsByRecipeId(recipeId);
                setReviews(recipeReviewResponse.data);
                const authorId = recipeResponse.data.authorId;
                if (authorId) {
                    const authorResponse = await getUserById(authorId);
                    setAuthor(authorResponse.data);
                }
                setError(null);
            } catch (err) {
                if (err.response?.status === 404) {
                    setError(err.response?.data?.error || 'No recipe found.');
                } else {
                    setError('Failed to fetch recipe. Please try again.');
                }
                setRecipe(null);
                setAuthor(null);
            }
        };
        fetchRecipeAndAuthor();
    }, [recipeId]);

    return { recipe, ingredients, reviews, author, error, refetchReviews };
};

export default useRecipe;
import { useState } from 'react';
import { updateFavoriteState } from '../api/recipe.api';

export function useFavorites(initialState = false) {
    const [isFavorite, setIsFavorite] = useState(initialState);

    const toggleFavorite = async (recipeId) => {
        try {
            await updateFavoriteState(recipeId);
            setIsFavorite(prev => !prev);
        } catch (error) {
            console.error("Failed to update favorite:", error);
        }
    };

    return { isFavorite, toggleFavorite };
}

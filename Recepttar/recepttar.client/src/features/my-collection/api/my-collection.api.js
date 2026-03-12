export async function getUserFavoriteRecipe() {
    const API_BASE = import.meta.env.VITE_API_URL;
    try {
        const response = await fetch(`${API_BASE}/api/user/favorites`, {
            method: 'GET',
            credentials: 'include',
        });
        // Add isFavorite property to each recipe
        const recipes = await response.json();
        return recipes.map(recipe => ({ ...recipe, isFavorite: true }));
    } catch (error) {
        console.error('Error fetching user favorites:', error);
        throw error;
    }
}

export async function getUserRecipes() {
    const API_BASE = import.meta.env.VITE_API_URL;
    try {
        const response = await fetch(`${API_BASE}/api/recipe/recipes`, {
            method: 'GET',
            credentials: 'include',
        });
        return await response.json();
    } catch (error) {
        console.error('Error toggling favorite:', error);
        throw error;
    }
}

export async function getUserPolls(userId) {
    const API_BASE = import.meta.env.VITE_API_URL;
    try {
        const response = await fetch(`${API_BASE}/api/poll/${userId}/polls`, {
            method: 'GET',
            credentials: 'include',
        });
        return await response.json();
    } catch (error) {
        console.error('Error fetching user polls:', error);
        throw error;
    }
}
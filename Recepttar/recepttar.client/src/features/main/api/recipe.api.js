export async function searchRecipes(query = '', type = '') {
    const params = new URLSearchParams();
    if (query) params.append('search', query);
    if (type) params.append('type', type);

    const res = await fetch(
        `https://localhost:7035/recipes/search?${params.toString()}`
    );

    if (!res.ok) throw new Error('Failed to fetch recipes');
    const recipes = await res.json();

    const favRes = await fetch(`https://localhost:7035/user/favorites`, {
        credentials: 'include'
    });
    if (!favRes.ok) throw new Error('Failed to fetch favorites');

    const favoritesData = await favRes.json();
    const favoriteIds = favoritesData.map(fav => fav.id); // Extract IDs

    const recipesWithFavorites = recipes.map(recipe => ({
        ...recipe,
        isFavorite: favoriteIds.includes(recipe.id)
    }));

    return recipesWithFavorites;
}


export async function getRecipeReviews(recipeId) {
    const res = await fetch(
        `https://localhost:7035/recipes/${recipeId}/reviews`
    );

    if (!res.ok) {
        throw new Error('Failed to fetch reviews');
    }

    return res.json();
}

export async function fetchActivePolls() {
    const res = await fetch('https://localhost:7035/polls/active', {
        credentials: 'include'
    });
    if (!res.ok) {
        throw new Error('Failed to fetch polls');
    }
    return res.json();
}

export async function submitVote(pollId, optionId) {
    const body = new URLSearchParams();
    body.append('OptionId', optionId);

    const res = await fetch(
        `https://localhost:7035/polls/${pollId}/vote`,
        {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: body.toString(),
        }
    );

    if (!res.ok) {
        throw new Error('Failed to submit vote');
    }

    return res.json();
}

export async function updateFavoriteState(recipeId) {
    const res = await fetch(
        `https://localhost:7035/user/favorites/` + recipeId,
        {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
        }
    );

    if (!res.ok) {
        throw new Error('Failed to modify favorite state');
    }

    return res.json();
}
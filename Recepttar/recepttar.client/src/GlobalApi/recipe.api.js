import axios from 'axios'

axios.defaults.withCredentials = true;

export async function searchRecipes(query = '', type = '') {
    const params = new URLSearchParams();
    if (query) params.append('search', query);
    if (type) params.append('type', type);

    const res = await axios.get('https://localhost:7035/recipes/search', { params });

    const recipes = res.data;

    try {
        const favRes = await axios.get('https://localhost:7035/user/favorites');

        const favoriteIds = favRes.data.map(fav => fav.id);

        return recipes.map(recipe => ({
            ...recipe,
            isFavorite: favoriteIds.includes(recipe.id)
        }));
    } catch (err) {
        if (err.response?.status === 401) {
            return recipes;
        }

        throw err;
    }
}

export async function getRecipeReviews(recipeId) {
    const res = await axios.get(`https://localhost:7035/recipes/${recipeId}/reviews`);
    return res.data;
}

export async function fetchActivePolls() {
    const res = await axios.get('https://localhost:7035/polls/active')

    if (res.status != 200) {
        throw new Error('Failed to fetch polls');
    }
    return res.data;
}

export async function submitVote(pollId, optionId) {
    const body = new URLSearchParams();
    body.append('OptionId', optionId);

    const res = await axios.post(`https://localhost:7035/polls/${pollId}/vote`, body.toString());

    return res.data;
}

export async function updateFavoriteState(recipeId) {

    const res = await axios.post(`https://localhost:7035/user/favorites/` + recipeId);
    return res.data;
}

export async function getLoginStatus() {
    return await axios.get('https://localhost:7035/user/profile');
}

export async function submitLogoutRequest() {
    return await axios.post('https://localhost:7035/user/logout');
}

export async function searchIngredients(search) {
    const params = new URLSearchParams();
    if (search) params.append('search', search);

    return await axios.get('https://localhost:7035/ingredients/search', { params });
}
import axios from 'axios'

axios.defaults.withCredentials = true;

const fallbackPolls = [
    {
        id: 1,
        question: "What is your favorite type of cuisine?",
        options: [
            { optionId: 1, optionText: "Italian", voteCount: 4 },
            { optionId: 2, optionText: "Asian", voteCount: 1 },
            { optionId: 3, optionText: "Mexican", voteCount: 1 },
            { optionId: 4, optionText: "French", voteCount: 1 }
        ],
        votedOn: 1
    }
];

const API_BASE = import.meta.env.VITE_API_URL;

export async function searchRecipes(query = '', type = '') {
    const params = new URLSearchParams();
    if (query) params.append('search', query);
    if (type) params.append('type', type);

    const res = await axios.get(`${API_BASE}/api/recipe/search`, { params });

    const recipes = res.data;

    try {
        const favRes = await axios.get(`${API_BASE}/api/user/favorites`);

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
    const res = await axios.get(`${API_BASE}/api/recipe/${recipeId}/reviews`);
    return res.data;
}

export async function fetchActivePolls() {
    try {
        const res = await axios.get(`${API_BASE}/api/poll/active`);

        return res.data
    }
    catch (error) {
        return fallbackPolls;
    }
}

export async function submitVote(pollId, optionId) {
    const body = new URLSearchParams();
    body.append('OptionId', optionId);

    const res = await axios.post(`${API_BASE}/api/poll/${pollId}/vote`, body.toString());

    return res.data;
}

export async function updateFavoriteState(recipeId) {

    const res = await axios.post(`${API_BASE}/api/user/favorites/` + recipeId);
    return res.data;
}

export async function getLoginStatus() {
    return await axios.get(`${API_BASE}/api/user/profile`);
}

export async function submitLogoutRequest() {
    return await axios.post(`${API_BASE}/api/user/logout`);
}

export async function searchIngredients(search) {
    const params = new URLSearchParams();
    if (search) params.append('search', search);

    return await axios.get(`${API_BASE}/api/ingredient/search`, { params });
}
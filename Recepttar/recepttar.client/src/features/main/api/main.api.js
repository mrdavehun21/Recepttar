import axios from 'axios'

axios.defaults.withCredentials = true;

function buildSearchParams([tags, ingredients, search]) {
    const params = {};

    if (search) {
        params.search = search;
    }

    const TAG_MAP = {
        Dessert: { type: "dessert" },
        "Main dish": { type: "mainDish" },
        Appetizer: { type: "appetizer" },

        Easy: { difficulty: "easy" },
        Medium: { difficulty: "medium" },
        Hard: { difficulty: "hard" },

        Vegan: { isVegan: true },

        Expensive: { isExpensive: true },
        Cheap: { isExpensive: false },
    };

    tags.forEach(tag => {
        const mapping = TAG_MAP[tag];
        if (!mapping) return;

        Object.assign(params, mapping);
    });

    if (ingredients.length) {
        params.ingredients = ingredients.map(i => i.id).join(",");
    }

    return params;
}

export async function searchRecipes(query = []) {
    const params = buildSearchParams(query);

    const res = await axios.get(
        "https://localhost:7035/recipes/search",
        { params }
    );

    const recipes = res.data;

    try {
        const favRes = await axios.get("https://localhost:7035/user/favorites");
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

export async function updateFavoriteState(recipeId, favoriteState) {
    let res = null;
    if (!favoriteState) {
        res = await axios.post(`https://localhost:7035/user/favorites/` + recipeId);
    }
    else {
        res = await axios.delete(`https://localhost:7035/user/favorites/` + recipeId);
    }
    return res.data;
}

export async function getUserProfile() {
    try {
        const res = await axios.get('https://localhost:7035/user/profile');

        return res.data;
    } catch (err) {
        if (err.response?.status === 401) {
            return { isLoggedIn: false };
        }

        throw err;
    }
}

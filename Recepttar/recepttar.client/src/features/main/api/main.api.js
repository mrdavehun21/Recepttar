import axios from 'axios'

axios.defaults.withCredentials = true;

const fallbackPolls = [
    {
        id: 0,
        question: "What is your favorite type of cuisine?",
        options: [
            { optionId: 1, optionText: "Italian", voteCount: 4 },
            { optionId: 2, optionText: "Asian", voteCount: 1 },
            { optionId: 3, optionText: "Mexican", voteCount: 1 },
            { optionId: 4, optionText: "French", voteCount: 1 }
        ],
    }
];

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

export async function getAllRecipes() {
    const res = await axios.get("https://localhost:7035/api/recipe/all");

    const recipes = res.data;

    try {
        const favRes = await axios.get("https://localhost:7035/api/user/favorites");
        const favoriteIds = favRes.data.map(fav => fav.recipeId);

        return recipes.map(recipe => ({
            ...recipe,
            isFavorite: favoriteIds.includes(recipe.recipeId)
        }));
    } catch (err) {
        if (err.response?.status === 401) {
            return recipes;
        }
        throw err;
    }
}

export async function searchRecipes(query = []) {
    const params = buildSearchParams(query);

    const res = await axios.get(
        "https://localhost:7035/api/recipe/search",
        { params }
    );

    const recipes = res.data;

    try {
        const favRes = await axios.get("https://localhost:7035/api/user/favorites");
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
    const res = await axios.get(`https://localhost:7035/api/recipe/${recipeId}/reviews`);
    return res.data;
}

export async function fetchActivePolls() {
    try {
        const res = await axios.get('https://localhost:7035/api/poll/active')
        return res.data
    }
    catch (error) {
        return fallbackPolls;
    }
}

export async function submitVote(pollId, optionId) {
    const body = new URLSearchParams();
    body.append('OptionId', optionId);

    const res = await axios.post(`https://localhost:7035/api/poll/${pollId}/vote`, body.toString());

    return res.data;
}

export async function updateFavoriteState(recipeId, favoriteState) {
    let res = null;
    if (!favoriteState) {
        res = await axios.post(`https://localhost:7035/api/user/favorites/` + recipeId);
    }
    else {
        res = await axios.delete(`https://localhost:7035/api/user/favorites/` + recipeId);
    }
    return res.data;
}

export async function getUserProfile() {
    try {
        const res = await axios.get('https://localhost:7035/api/user/profile');

        return res.data;
    } catch (err) {
        if (err.response?.status === 401) {
            return { isLoggedIn: false };
        }

        throw err;
    }
}

export async function getAuthorProfile(userId){
    try{
        const res = await axios.get(`https://localhost:7035/api/user/profile/${userId}`);

        return res.data;
    }
    catch(err){
        throw err;
    }
}

export async function deletePoll(pollId) {
    try {
        const res = await axios.delete(`https://localhost:7035/api/poll/${pollId}`);

        return res;
    } catch (error) {

    }
}
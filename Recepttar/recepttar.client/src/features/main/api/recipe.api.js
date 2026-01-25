export async function searchRecipes(query = '', type = '') {
    const params = new URLSearchParams();
    if (query) params.append('search', query);
    if (type) params.append('type', type);

    const res = await fetch(
        `https://localhost:7035/recipes/search?${params.toString()}`
    );

    if (!res.ok) {
        throw new Error('Failed to fetch recipes');
    }

    return res.json();
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
    const res = await fetch('https://localhost:7035/polls/active');
    if (!res.ok) {
        throw new Error('Failed to fetch polls');
    }
    return res.json();
}

export async function submitVote(pollId, optionId) {
    const res = await fetch(`https://localhost:7035/polls/${pollId}/vote`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ optionId }),
    });

    if (!res.ok) {
        throw new Error('Failed to submit vote');
    }

    return res.json();
}

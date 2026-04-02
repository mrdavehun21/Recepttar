import axios from 'axios';

const API_BASE = import.meta.env.VITE_API_URL;

export async function fetchSortOptions(){
    let response = null;
    try {
        response = await axios.get(`${API_BASE}/api/leaderboard/sortoptions`);
        return response.data.categories;
    } catch (err) {
        console.error('Error fetching categories:', err);
        throw err;
    }
}

export async function fetchLeaderboardData(sortOption){
    let response = null;
    let url = `${API_BASE}/api/leaderboard` + (sortOption ? `?sortBy=${sortOption}` : '');
    try {
        response = await axios.get(url);
        return response.data;
    } catch (err) {
        console.error('Error fetching leaderboard data:', err);
        throw err;
    }
}
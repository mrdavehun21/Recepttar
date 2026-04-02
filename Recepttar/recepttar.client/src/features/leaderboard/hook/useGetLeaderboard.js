import { fetchLeaderboardData } from '../api/api.leaderboard';

export async function getSortOptions() {
    
}

export async function getLeaderboardData(sortOption = null) {
    return await fetchLeaderboardData(sortOption);
}
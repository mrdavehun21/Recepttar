import { fetchLeaderboardData, fetchSortOptions } from '../api/api.leaderboard';

export async function getSortOptions() {
    return await fetchSortOptions();
}

export async function getLeaderboardData(sortOption = null) {
    return await fetchLeaderboardData(sortOption);
}
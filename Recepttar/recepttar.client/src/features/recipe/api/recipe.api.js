import axios from 'axios';

const API_BASE = import.meta.env.VITE_API_URL;

export function getRecipeById(recipeId) {
    return axios.get(`${API_BASE}/api/recipe/${recipeId}`);
}
export function getUserById(userId) {
    return axios.get(`${API_BASE}/api/user/profile/${userId}`);
}
export function getReviewsByRecipeId(recipeId) {
    return axios.get(`${API_BASE}/api/recipe/${recipeId}/reviews`)
}
export function addReviewByRecipeId(recipeId, reviewData) {
    return axios.post(`${API_BASE}/api/recipe/${recipeId}/reviews`, reviewData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
}
export function updateReviewById(reviewId, reviewData) {
    return axios.patch(`${API_BASE}/api/review/${reviewId}`, reviewData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
}
export function deleteReviewById(recipeId) {
    return axios.delete(`${API_BASE}/api/review/${recipeId}`)
}
export function deleteRecipeById(recipeId){
    return axios.delete(`${API_BASE}/api/recipe/${recipeId}`);
};
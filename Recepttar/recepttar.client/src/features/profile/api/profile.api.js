import axios from "axios";

const API_BASE = import.meta.env.VITE_API_URL;

export async function UpdateUserAPI(form, setError) {
    const token = localStorage.getItem('token');

    try{
        await axios.patch(`${API_BASE}/api/user/profile`, form, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            }
        });
        return true;
    } catch (error) {
        setError(error.message || 'An error occurred while updating the profile.');
        return false;
    }
}


export async function fetchProfileDataAPI(userId, setError) {
    const token = localStorage.getItem('token');

    try {
        const response = await axios.get(`${API_BASE}/api/user/profile/` + (userId || ''));
        const userRecipes = await axios.get(`${API_BASE}/api/recipe/` + response.data.id + "/recipes");
        response.data.recipes = userRecipes.data;
        return response.data;
    } catch (error) {
        if(error.status === 401){
            setError("Unauthorized. Please log in again.");
            return null;
        }
        else{
            setError(error.message || 'An error occurred while fetching the profile data.');
        }
        return null;
    }
}
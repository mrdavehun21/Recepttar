import { useState } from "react";
import { fetchProfileDataAPI } from "../api/profile.api";

export function useProfileFetcher() {
    const [data, setData] = useState(null);
    const fetchProfileData = async (userId, setError) => {
        const data = await fetchProfileDataAPI(userId, setError);
        return data;
    }
    return {
        fetchProfileData
    };
}
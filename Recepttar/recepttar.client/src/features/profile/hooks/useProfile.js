import { useEffect, useState } from "react";
import { useProfileFetcher } from "./useProfileFetcher";
import { ImageAvailable } from "../../../shared/hooks/usePictureChecker";

export function useProfile(profileId, profileData, isLoggedIn, setError) {
    const API_BASE = import.meta.env.VITE_API_URL;

    const { fetchProfileData } = useProfileFetcher();

    const [data, setData] = useState();
    const [imageExists, setImageExists] = useState(false);

    useEffect(() => {
        const loadProfile = async () => {
            const id =
                profileId === undefined && !isLoggedIn
                    ? profileData?.id
                    : profileId;

            const result = await fetchProfileData(id, setError);
            setData(result);
        };

        loadProfile();
    }, [profileId]);

    useEffect(() => {
        async function checkImage() {
            const exists = await ImageAvailable(`${API_BASE}/${data?.profilePicture}`);
            setImageExists(exists);
        }

        if (data?.id) checkImage();
    }, [data?.id]);

    return { data, imageExists };
}
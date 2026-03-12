import { useEffect, useState } from 'react';
import { getUserFavoriteRecipe, getUserRecipes, getUserPolls } from '../api/my-collection.api';

export default function useGetUserCollections(userId) {
    const [favoriteRecipes, setFavoriteRecipes] = useState([]);
    const [userRecipes, setUserRecipes] = useState([]);
    const [userPolls, setUserPolls] = useState([]);

    useEffect(() => {
        const fetchFavoriteRecipes = async () => {
            try {
                const data = await getUserFavoriteRecipe();
                setFavoriteRecipes(data);
            } catch (error) {
                console.error('Error fetching favorite recipes:', error);
            }
        };

        fetchFavoriteRecipes();
    }, []);

    useEffect(() => {
        const fetchUserRecipes = async () => {
            try {
                const data = await getUserRecipes();
                setUserRecipes(data);
            } catch (error) {
                console.error('Error fetching user recipes:', error);
            }
        };

        fetchUserRecipes();
    }, []);

    useEffect(() => {
        const fetchUserPolls = async () => {
            try {
                const data = await getUserPolls(userId);
                setUserPolls(data);
            } catch (error) {
                console.error('Error fetching user polls:', error);
            }
        };

        fetchUserPolls();
    }, []);

    return {
        favoriteRecipes,
        userRecipes,
        userPolls
    };
}
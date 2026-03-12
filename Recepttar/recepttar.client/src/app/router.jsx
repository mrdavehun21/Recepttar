import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from '../shared/hooks/useAuthContext';
import LoginPage from '../features/auth/pages/LoginPage.jsx';
import RegisterPage from '../features/auth/pages/RegisterPage.jsx';
import Home from '../features/main/pages/home.jsx';
import Recipe from '../features/recipe/pages/RecipeDetail.jsx';
import Polls from '../features/poll/pages/poll.jsx';
import ProfilePage from '../features/profile/pages/profile.jsx';
import MyCollection from '../features/my-collection/pages/MyCollection.jsx';

export default function AppRouter() {
    const { isLoggedIn, profileData } = useAuth();

    return (
        <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage/>} />
            <Route path="/home" element={<Home isLoggedIn={isLoggedIn} profileID={profileData} />} />
            <Route path="/" element={<Navigate to="/home" />} />
            <Route path="/recipe/:recipeId" element={<Recipe />} />
            <Route path="/polls"
                element={
                    isLoggedIn === true ? (
                        <Polls loginStatus={true} profileID={profileData} />
                    ) : isLoggedIn === false ? (
                        <Navigate to="/login" replace />
                    ) : null
                }
            />
            <Route path="/profile" 
                element={
                    isLoggedIn === true ? (
                        <ProfilePage />
                    ) : isLoggedIn === false ? (
                        <Navigate to="/login" replace />
                    ) : null
                }
            />
            <Route path="/profile/:profileId" element={<ProfilePage />} />
            <Route path="/mycollection" 
                element={
                    isLoggedIn === true ? (
                        <MyCollection />
                    ) : isLoggedIn === false ? (
                        <Navigate to="/login" replace />
                    ) : null
                }
            />
        </Routes>
    )
}

import { Routes, Route, Navigate } from 'react-router-dom'
import LoginPage from '../features/auth/pages/LoginPage.jsx'
import RegisterPage from '../features/auth/pages/RegisterPage.jsx'
import Home from '../features/main/pages/Home.jsx'
import Recipe from '../features/recipe/pages/RecipeDetail.jsx'
import Polls from '../features/poll/pages/poll.jsx'
import { useIsLoggedIn } from '../GlobalHooks/useLoginState'

export default function AppRouter() {
    const { isLoggedIn, profileData } = useIsLoggedIn();

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
        </Routes>
    )
}

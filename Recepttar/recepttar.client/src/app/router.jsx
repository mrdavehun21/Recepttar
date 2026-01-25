import { Routes, Route, Navigate } from 'react-router-dom'
import Home from '../features/main/pages/Home.jsx'

export default function AppRouter() {
    return (
        <Routes>
            <Route path="/home" element={<Home />} />
            <Route path="/" element={<Navigate to="/home" />} />
        </Routes>
    )
}

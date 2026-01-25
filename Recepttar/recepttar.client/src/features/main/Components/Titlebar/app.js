import { useEffect, useState } from 'react';
import Card from '../Components/RecipeCard/Card';
import Titlebar from './Titlebar';

function App() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [typeFilter, setTypeFilter] = useState(''); // appetizer, main, dessert

    const fetchRecipes = async (query = '', type = '') => {
        setLoading(true);
        setError(null);

        const params = new URLSearchParams();
        if (query) params.append('search', query);
        if (type) params.append('type', type);

        try {
            const res = await fetch(`https://localhost:7035/recipes/search?${params.toString()}`);
            const recipes = await res.json();

            const recipesWithRatings = await Promise.all(
                recipes.map(async recipe => {
                    const resReviews = await fetch(`https://localhost:7035/recipes/${recipe.id}/reviews`);
                    const reviews = await resReviews.json();
                    const avg = reviews.length > 0 ? reviews.reduce((a, b) => a + b.stars, 0) / reviews.length : 0;
                    return { ...recipe, averageRating: avg, reviewCount: reviews.length };
                })
            );

            setData(recipesWithRatings);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchRecipes();
    }, []);

    const handleSearch = (query, type = '') => {
        setSearchQuery(query);
        setTypeFilter(type);
        fetchRecipes(query, type);
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <>
            <Titlebar onSearch={handleSearch} />
            <div className="d-flex flex-wrap gap-3 mt-5 justify-content-center pb-3">
                {data.map(item => (
                    <Card key={item.id} data={item} />
                ))}
            </div>
        </>
    );
}

export default App;

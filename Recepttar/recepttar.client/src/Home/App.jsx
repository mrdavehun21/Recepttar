import { useEffect, useState } from 'react';
import Card from '../RecipeCard/Card'

function App() {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch('https://localhost:7035/recipes')
            .then((res) => {
                if (!res.ok) {
                    throw new Error('Request failed');
                }
                return res.json();
            })
            .then((data) => {
                setData(data);
                setLoading(false);
            })
            .catch((err) => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div className="d-flex flex-wrap gap-3 mt-5 justify-content-center pb-3">
            {data.map(item => (
                <>
                    <Card data={item} />
                </>
            ))}
        </div>
    );
}

export default App;
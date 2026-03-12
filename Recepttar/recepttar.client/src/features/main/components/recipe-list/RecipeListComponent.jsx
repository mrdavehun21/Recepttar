import { useState } from "react";

import Card from '../recipe-card/Card';

function Reicpes({ recipes }) {
    const [visibleCount, setVisibleCount] = useState(9);

    return (
        <div className="recipe-container-background w-95 rounded-2 m-4">
            <h1 className="m-2 mt-2 mb-4 ms-2 text-decoration-underline color-neutral-100 fs-3">Recipes</h1>
            <div className="d-flex flex-wrap align-items-center w-100 justify-content-center gap-3 pb-3 p-2">
                {recipes.slice(0, visibleCount).map(item => (
                    <Card key={item.recipeId} data={item} />
                ))}
            </div>
            {
                visibleCount < recipes.length && (
                    <div className="text-center pb-4">
                        <button
                            className="polls-bg-additional-8 p-2 rounded-2 text-light ms-auto me-auto mt-3 nav-link"
                            onClick={() => setVisibleCount(prev => prev + 6)}
                        >
                            Show More
                        </button>
                    </div>
                )
            }
        </div>
    );
}

export default Reicpes;

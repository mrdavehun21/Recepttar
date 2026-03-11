import { useState } from "react";
import { getIngredientsOnSearch } from "../../hooks/useIngredientSearch";
import "../ingredients-filter/IngredientFilter.css";

function IngredientFilter({ selectedIngredients, setSelectedIngredients }) {
    const [ingredientSearch, setIngredientSearch] = useState("");
    const [results, setResults] = useState([]);

    const handleSearch = async (value) => {
        const data = await getIngredientsOnSearch(value);
        setResults(data);
    };

    const toggleIngredient = (ingredient) => {
        setSelectedIngredients((prev) =>
            prev.some((item) => item.id === ingredient.id)
                ? prev.filter((item) => item.id !== ingredient.id)
                : [...prev, ingredient]
        );
    };

    const selectedIds = selectedIngredients.map(i => i.id);
    const unselectedResults = results.filter(
        ingredient => !selectedIds.includes(ingredient.id)
    ).slice(0, 4 - selectedIngredients.length);

    return (
        <div className="w-95 d-flex flex-column align-items-center">
            <h4 className="mt-0 mb-3 text-decoration-underline color-neutral-100 fs-3">
                Ingredients
            </h4>

            <div className="rounded-5 d-flex border border-dark overflow-hidden">
                <input
                    type="text"
                    value={ingredientSearch}
                    onChange={(e) => {
                        const value = e.target.value;
                        setIngredientSearch(value);
                        handleSearch(value);
                    }}
                    className="border-0 p-2 d-block"
                />
                <button className="border-0 p-2 d-block bg-white">
                    <i className="bi bi-search"></i>
                </button>
            </div>

            <div
                className="ms-auto me-auto mt-3 w-100"
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(2, 1fr)",
                    gap: "0.5rem",
                }}
            >
                {selectedIngredients.map((ingredient) => (
                    <div
                        key={ingredient.id}
                        onClick={() => toggleIngredient(ingredient)}
                        className="text-center p-2 OptionButtonIngredient tag-bg-additional-1 tag-bg-additional-2-hover tag-bg-additional-3"
                    >
                        {ingredient.name}
                    </div>
                ))}

                {unselectedResults.map((ingredient) => (
                    <div
                        key={ingredient.id}
                        onClick={() => toggleIngredient(ingredient)}
                        className="text-center p-2 OptionButtonIngredient tag-bg-additional-1 tag-bg-additional-2-hover"
                    >
                        {ingredient.name}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default IngredientFilter;

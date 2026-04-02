import { useTagSelection } from "../../hooks/useTagSelection";
import SearchComponent from "../search/SearchComponent";
import Ingredient from "../../../../shared/components/ingredients-filter/IngredientFilter";
import "./SearchParentComponent.css";

function SearchApp({
        selectedTags,
        setSelectedTags,
        selectedIngredients,
        setSelectedIngredients,
        setSearch
    }) {

    const { TAGS, toggleTag } = useTagSelection(
        selectedTags,
        setSelectedTags
    );

    const colors = [
        "is-expensive",
        "is-expensive",
        "is-vegan",

        "Difficulty",
        "Difficulty",
        "Difficulty",

        "DishType",
        "DishType",
        "DishType"
    ]

    return (
        <div className="search-container w-95 rounded-2 m-4">
            <h2 className="m-2 mt-2 mb-4 ms-2 text-decoration-underline text-light fs-3">
                Search
            </h2>

            <div className="d-flex flex-column flex-lg-row justify-content-between bg-light m-3 p-2 rounded-2 shadow gap-4 gap-lg-0">
                <div className="d-flex flex-column align-items-center order-1 order-lg-1">
                    <h3 className="mt-0 mb-3 text-decoration-underline color-neutral-100 fs-3">
                        Tags
                    </h3>

                    <div className="OptionButtonContainer">
                        {TAGS.map((tag, index) => (
                            <button
                                key={tag}
                                onClick={() => toggleTag(tag)}
                                className={`OptionButton tag-bg-additional-1 tag-bg-additional-2-hover ${selectedTags.includes(tag)
                                    ? "bg-white border border-black border-2"
                                        : colors[index]
                                    }`}
                            >
                                {tag}
                            </button>
                        ))}
                    </div>
                </div>

                <SearchComponent setSearch={setSearch} />

                <div
                    className="d-flex flex-column align-items-center order-2 order-lg-3 ms-auto ms-lg-0 me-auto me-lg-0"
                    style={{ width: "max(25%, 240px)" }}
                >
                    <Ingredient
                        selectedIngredients={selectedIngredients}
                        setSelectedIngredients={setSelectedIngredients}
                    />
                </div>
            </div>
        </div>
    );
}

export default SearchApp;

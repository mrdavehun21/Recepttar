import { useCallback } from "react";

export function useTagSelection(selectedTags, setSelectedTags) {
    const TAGS = [
        "Expensive",
        "Cheap",
        "Vegan",
        "Easy",
        "Medium",
        "Hard",
        "Appetizer",
        "Main dish",
        "Dessert",
    ];

    const toggleTag = useCallback((tag) => {
        setSelectedTags((prev) => {
            let updated;

            if (prev.includes(tag)) {
                updated = prev.filter((t) => t !== tag);
            } else {
                updated = [...prev, tag];
            }

            switch (tag) {
                case "Expensive":
                    updated = updated.filter((t) => t !== "Cheap");
                    break;

                case "Cheap":
                    updated = updated.filter((t) => t !== "Expensive");
                    break;

                case "Easy":
                    updated = updated.filter(
                        (t) => t !== "Medium" && t !== "Hard"
                    );
                    break;

                case "Medium":
                    updated = updated.filter(
                        (t) => t !== "Easy" && t !== "Hard"
                    );
                    break;

                case "Hard":
                    updated = updated.filter(
                        (t) => t !== "Easy" && t !== "Medium"
                    );
                    break;

                case "Appetizer":
                    updated = updated.filter(
                        (t) => t !== "Main dish" && t !== "Dessert"
                    );
                    break;

                case "Main dish":
                    updated = updated.filter(
                        (t) => t !== "Appetizer" && t !== "Dessert"
                    );
                    break;

                case "Dessert":
                    updated = updated.filter(
                        (t) => t !== "Appetizer" && t !== "Main dish"
                    );
                    break;
            }

            return updated;
        });
    }, [setSelectedTags]);

    return { TAGS, toggleTag };
}

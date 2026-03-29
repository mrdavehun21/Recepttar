import { uploadRecipe } from "../api/api.create-recipe";

export function submitRecipe(e, setErrorMessage) {
    e.preventDefault();

    const formData = new FormData(e.target);

    //replace formData IsExpensive value with boolean
    const isExpensiveValue = formData.get("IsExpensive");
    formData.set("IsExpensive", isExpensiveValue === "Expensive");

    // if isVegan is not checked, set it to true
    if (!formData.has("IsVegan")) {
      formData.set("IsVegan", false);
    }
    else{
      formData.set("IsVegan", true);
    }

    formData.set("Difficulty", formData.get("Difficulty").toLocaleLowerCase());

    uploadRecipe(formData, setErrorMessage);
}

export function handleRemoveIngredient(id, setIngredients) {
    setIngredients(prev => prev.filter(ing => ing.id !== id));
};
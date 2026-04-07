import { getRecipe, updateRecipe } from "../api/api.patch-recipe";

export async function patchRecipe(recipeId, formData, setErrors, navigate) {
    // formData.preventDefault();

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

    const success = await updateRecipe(recipeId, formData, setErrors);
    if (success) {
        navigate(`/recipe/${recipeId}`);
    }
}

export async function fetchRecipe(recipeId){
    return await getRecipe(recipeId);
}
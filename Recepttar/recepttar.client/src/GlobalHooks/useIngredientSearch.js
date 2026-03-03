import { searchIngredients } from '../GlobalApi/recipe.api';

export async function getIngredientsOnSearch(search) {
    const res = await searchIngredients(search);
    return res.data;
}
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from '../../../shared/hooks/useAuthContext';
import { submitRecipe } from '../hooks/useCreateRecipe';
import { patchRecipe, fetchRecipe } from '../hooks/usePatchRecipe';
import RecipeProperties from '../components/recipe-properties/RecipeProperties';
import RecipeBasic from '../components/recipe-basic-info/RecipeBasic';
import Ingredients from '../../../shared/components/ingredients-filter/IngredientFilter';
import RecipeSteps from '../components/recipe-steps/RecipeSteps';
import IngredientList from '../components/recipe-ingredients-list/IngredientsList';
import ErrorPage from '../../../shared/pages/NotFound';

function CreateRecipe() {
    const { recipeId } = useParams();
    const { isLoggedIn, profileData } = useAuth();
    const navigate = useNavigate();

    const [selectedIngredients, setSelectedIngredients] = useState([]);
    const [ingredients, setIngredients] = useState([]);
    const [recipeData, setRecipeData] = useState(null);
    const [errors, setErrors] = useState([]);
    const [steps, setSteps] = useState([
        { id: 1, text: "" },
        { id: 2, text: "" }
    ]);

    useEffect(() => {
        if (selectedIngredients.length > 0) {
            if(ingredients.some(ing => selectedIngredients.some(sel => sel.id === ing.id))){
                setSelectedIngredients([]);
                return;
            }
            setIngredients(prev => [...prev, ...selectedIngredients]);
            setSelectedIngredients([]);
        }
    }, [selectedIngredients]);

    useEffect(() => {
        if (recipeId == null) return;
        async function fetchData() {
            const recipe = await fetchRecipe(recipeId);

            if(recipe?.authorId !== profileData?.id){
                navigate(`/recipe/${recipeId}`);
            }

            setIngredients(recipe.ingredients);
            setRecipeData(recipe);
        }
        fetchData();
    }, [recipeId])

    return (
        <form className="ms-auto me-auto w-95 mt-3" onSubmit={(e) => {
            e.preventDefault();
            setErrors({});

            const formData = new FormData(e.target);
            const newErrors = {};

            const title = formData.get("Title")?.trim();
            if (!title) {
                newErrors.Title = ["Title is required."];
            }

            const description = formData.get("Description")?.trim();
            if (!description) {
                newErrors.Description = ["Description is required."];
            }

            const timeMinutes = Number(formData.get("TimeMinutes"));
            if (!timeMinutes || timeMinutes <= 0) {
                newErrors.TimeMinutes = ["Time must be greater than 0."];
            }

            const servings = Number(formData.get("Servings"));
            if (!servings || servings <= 0) {
                newErrors.Servings = ["Servings must be greater than 0."];
            }

            if (ingredients.length === 0) {
                newErrors.Ingredients = "At least one ingredient is required.";
            }

            if (steps.length === 0) {
                newErrors.Steps = ["At least one step is required."];
            }

            if (Object.keys(newErrors).length > 0) {
                setErrors(newErrors);
                return;
            }

            if (recipeId != null) {
                patchRecipe(recipeId, formData, setErrors, navigate);
            } else {
                submitRecipe(e, setErrors, profileData.id, navigate);
            }
        }}>
            <h3>
                {recipeData ? "Edit recipe" : "Create a new recipe"}
            </h3>
            <div className="row g-3">
                <div className="col-12 col-md-8">
                    <RecipeBasic errors={errors} recipeData={recipeData} />
                    <div className="card shadow mt-3 p-3 me-auto ms-auto container d-md-none">
                        <div className={"p-2 bg-danger text-white text-start " + (errors?.Ingredients == null ? "d-none" : "")}>{errors?.Ingredients}&nbsp;</div>
                        <Ingredients selectedIngredients={selectedIngredients} setSelectedIngredients={setSelectedIngredients} />
                    </div>
                    <IngredientList ingredients={ingredients} setIngredients={setIngredients} errors={errors} />
                </div>
                <div className="col-12 col-md-4">
                    <RecipeProperties errors={errors} recipeData={recipeData} />
                    <div className="card shadow mt-3 p-3 me-auto ms-auto container d-none d-md-block">
                        <div className={"p-2 bg-danger text-white text-start " + (errors?.Ingredients == null ? "d-none" : "")}>{errors?.Ingredients}&nbsp;</div>
                        <Ingredients selectedIngredients={selectedIngredients} setSelectedIngredients={setSelectedIngredients} />
                    </div>
                </div>
            </div>
            <div className="row p-0 g-3">
                <div className="col-12">
                    <RecipeSteps errors={errors} recipeData={recipeData} steps={steps} setSteps={setSteps} />
                </div>
            </div>
        </form>
    );
}

export default CreateRecipe;
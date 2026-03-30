import RecipeProperties from '../components/recipe-properties/RecipeProperties';
import RecipeBasic from '../components/recipe-basic-info/RecipeBasic';
import Ingredients from '../../../shared/components/ingredients-filter/IngredientFilter';
import RecipeSteps from '../components/recipe-steps/RecipeSteps';
import { submitRecipe } from '../hooks/useCreateRecipe';
import IngredientList from '../components/recipe-ingredients-list/IngredientsList';
import { useEffect, useState } from 'react';
import { useAuth } from '../../../shared/hooks/useAuthContext';
import { useNavigate } from "react-router-dom";

function CreateRecipe() {
  const { isLoggedIn, profileData } = useAuth();
  const navigate = useNavigate();

  const [selectedIngredients, setSelectedIngredients] = useState([]);
  const [ingredients, setIngredients] = useState([]);
  const [errors, setErrors] = useState([]);

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

  return (
    <form className="ms-auto me-auto w-95 mt-3" onSubmit={e => submitRecipe(e, setErrors, profileData.id, navigate)}>
      <div className="row g-3">
        <div className="col-12 col-md-8">
          <RecipeBasic errors={errors} />
          <div className="card shadow mt-3 p-3 me-auto ms-auto container d-md-none">
            <div className={"p-2 bg-danger text-white text-start " + (errors?.Ingredients == null ? "d-none" : "")}>{errors?.Ingredients}&nbsp;</div>
            <Ingredients selectedIngredients={selectedIngredients} setSelectedIngredients={setSelectedIngredients} />
          </div>
          <IngredientList ingredients={ingredients} setIngredients={setIngredients} errors={errors} />
        </div>
        <div className="col-12 col-md-4">
          <RecipeProperties errors={errors} />
          <div className="card shadow mt-3 p-3 me-auto ms-auto container d-none d-md-block">
            <div className={"p-2 bg-danger text-white text-start " + (errors?.Ingredients == null ? "d-none" : "")}>{errors?.Ingredients}&nbsp;</div>
            <Ingredients selectedIngredients={selectedIngredients} setSelectedIngredients={setSelectedIngredients} />
          </div>
        </div>
      </div>
      <div className="row p-0 g-3">
          <div className="col-12">
            <RecipeSteps errors={errors} />
          </div>
      </div>
    </form>
  );
}

export default CreateRecipe;
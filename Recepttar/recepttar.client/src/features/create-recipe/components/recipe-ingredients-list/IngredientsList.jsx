import { handleRemoveIngredient } from '../../hooks/useCreateRecipe';
import { useEffect, useState } from 'react';
import { getMeasurementUnit } from '../../api/api.create-recipe';

function IngredientList({ ingredients, setIngredients, errors, t }) {
    const [measurementUnits, setMeasurementUnits] = useState([]);
        useEffect(() => {
            getMeasurementUnit(setMeasurementUnits);
    }, []);

  return (
    ingredients.length > 0 && (
        <div className="card shadow mt-3 p-3 me-auto ms-auto container">
            <h5 className="card-title">{t("createEditRecipePage.selectedIngredients")}</h5>
            <ul className="list-group list-group-flush">
                {ingredients.map((ingredient, index) => (
                    <div key={`ingredient${index}`}>
                        <div className={"p-2 bg-danger text-white text-start mt-1 " + (errors?.[`Ingredients[${index}].Quantity`] == null ? "d-none" : "")}>{errors?.[`Ingredients[${index}].Quantity`]?.[0]}&nbsp;</div>
                        <div className={"p-2 bg-danger text-white text-start mt-1 " + (errors?.[`Ingredients[${index}].MeasurementUnit`] == null ? "d-none" : "")}>{errors?.[`Ingredients[${index}].MeasurementUnit`]?.[0]}&nbsp;</div>
                        <div key={ingredient.id} className="ingredient-highlight list-group-item d-flex justify-content-between flex-wrap align-items-center">
                            {ingredient.name || ingredient.ingredientName}
                            <div className="d-flex gap-1">
                                <input type="hidden" name={`Ingredients[${index}].Id`} value={ingredient.id} />
                                <input type="number" name={`Ingredients[${index}].Quantity`} id="" className="d-block form-control" style={{width: "80px"}} min={1} max={999} defaultValue={1} />
                                <select name={`Ingredients[${index}].MeasurementUnit`} id="" className="form-select" defaultValue={ingredient.measurementUnit || "Unit"}>
                                    <option value="Unit">{t("measurementUnits.unit")}</option>
                                    {measurementUnits.map((unit) => (
                                        <option key={unit} value={unit}>{t(`measurementUnits.${unit.toLowerCase()}`)}</option>
                                    ))}
                                </select>
                                <a onClick={() => handleRemoveIngredient(ingredient.id, setIngredients)}><i className="bi bi-trash3 text-danger fs-3 ms-2"></i></a>
                            </div>
                        </div>
                    </div>
                ))}
            </ul>
        </div>
    )
  );
}

export default IngredientList;
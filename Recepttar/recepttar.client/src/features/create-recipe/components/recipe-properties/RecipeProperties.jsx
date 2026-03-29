import { useState } from 'react';

function RecipeProperties({ errors }) {
  const [isExpensive, setIsExpensive] = useState("Cheap");

    return (
      <div className="container p-3 bg-disabled d-flex flex-column gap-3 card shadow">
        <div className="w-100">
          <h5 className="form-check-label d-block card-title" htmlFor="RecipeName">Recipe difficulty</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Difficulty == null ? "d-none" : "")}>{errors?.Difficulty}&nbsp;</div>
          <select className="w-100 form-select" name="Difficulty">
            <option value="Difficulty" selected>Difficulty</option>
            <option value="Easy">Easy</option>
            <option value="Medium">Medium</option>
            <option value="Hard">Hard</option>
          </select>
        </div>

        {/* HOOK */}
        
        <div>
          <h5 className="form-check-label d-block card-title" htmlFor="PrepMin">Preparation time</h5>
          <input type="number" min={1} max={1440} id="PrepMin" name="TimeMinutes" defaultValue={1} className="d-block w-100 form-control"></input>
        </div>

        <div>
          <h5 className="form-check-label d-block card-title" htmlFor="Servings">Servings</h5>
          <input className="d-block w-100 form-control" type="number" min={1} max={1440} name="Servings" id="Servings" defaultValue={1}></input>
        </div>

        <div>
          <h5 className="card-title">Price</h5>
          <div className="form-check form-check-inline">
            <input className="form-check-input" type="radio" name="IsExpensive" id="inlineRadio1" value="Cheap" checked={isExpensive === "Cheap"} onChange={(e) => setIsExpensive(e.target.value)} />
            <label className="form-check-label" htmlFor="inlineRadio1">Cheap</label>
          </div>
          <div className="form-check form-check-inline">
            <input className="form-check-input" type="radio" name="IsExpensive" id="inlineRadio2" value="Expensive" checked={isExpensive === "Expensive"} onChange={(e) => setIsExpensive(e.target.value)} />
            <label className="form-check-label" htmlFor="inlineRadio2">Expensive</label>
          </div>
        </div>

        <div>
          <h5 className="card-title">Vegan</h5>
          <div className="form-check form-switch">
            <input className="form-check-input" name="IsVegan" type="checkbox" value="" id="checkNativeSwitch" switch />
            <label className="form-check-label" for="checkNativeSwitch">Vegan</label>
          </div>
        </div>

        <div>
          <h5 className="card-title">Dish type</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Type == null ? "d-none" : "")}>{errors?.Type}&nbsp;</div>
          <select className="w-100 form-select" name="Type">
            <option value="Type" selected>Type</option>
            <option value="Appetizer">Appetizer</option>
            <option value="MainDish">Main dish</option>
            <option value="Dessert">Dessert</option>
          </select>
        </div>
      </div>
  );
}

export default RecipeProperties;
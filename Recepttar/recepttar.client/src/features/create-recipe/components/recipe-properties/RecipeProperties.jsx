import { useState, useEffect } from 'react';

function RecipeProperties({ errors, recipeData }) {
  const [isExpensive, setIsExpensive] = useState("Cheap");
  const [difficulty, setDifficulty] = useState("Difficulty");
  const [type, setType] = useState("Type");
  const [isVegan, setIsVegan] = useState(false);
  const [prepTime, setPrepTime] = useState(1);
  const [servings, setServings] = useState(1);

  useEffect(() => {
    if(recipeData != null){
      setIsExpensive(recipeData.isExpensive ? "Expensive" : "Cheap");
      setDifficulty(recipeData.difficulty);
      setType(recipeData.type);
      setIsVegan(recipeData.isVegan);
      setPrepTime(recipeData.timeMinutes);
      setServings(recipeData.servings);
    }
  }, [recipeData]);

    return (
      <div className="container p-3 bg-disabled d-flex flex-column gap-3 card shadow">
        <div className="w-100">
          <h5 className="form-check-label d-block card-title" htmlFor="RecipeName">Recipe difficulty</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Difficulty == null ? "d-none" : "")}>{errors?.Difficulty}&nbsp;</div>
          <select className="w-100 form-select" name="Difficulty" value={difficulty} onChange={e => setDifficulty(e.target.value)}>
            <option key={"Difficulty"} value="Difficulty">Difficulty</option>
            <option key={"Easy"} value="Easy">Easy</option>
            <option key={"Medium"} value="Medium">Medium</option>
            <option key={"Hard"} value="Hard">Hard</option>
          </select>
        </div>
        
        <div>
          <h5 className="form-check-label d-block card-title" htmlFor="PrepMin">Preparation time (min)</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.TimeMinutes == null ? "d-none" : "")}>{errors?.TimeMinutes}&nbsp;</div>
          <input type="number" min={1} max={1440} id="PrepMin" name="TimeMinutes" value={prepTime} onChange={e => setPrepTime(e.target.value)} className="d-block w-100 form-control"></input>
        </div>

        <div>
          <h5 className="form-check-label d-block card-title" htmlFor="Servings">Servings</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Servings == null ? "d-none" : "")}>{errors?.Servings}&nbsp;</div>
          <input className="d-block w-100 form-control" type="number" min={1} max={1440} name="Servings" id="Servings" value={servings} onChange={e => setServings(e.target.value)}></input>
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
            <input className="form-check-input" name="IsVegan" type="checkbox" checked={isVegan} onChange={e => setIsVegan(e.target.checked)} id="checkNativeSwitch" switch="false" />
            <label className="form-check-label" htmlFor="checkNativeSwitch">Vegan</label>
          </div>
        </div>

        <div>
          <h5 className="card-title">Dish type</h5>
          <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Type == null ? "d-none" : "")}>{errors?.Type}&nbsp;</div>
          <select className="w-100 form-select" name="Type" value={type} onChange={e => setType(e.target.value)}>
            <option value="Type">Type</option>
            <option value="Appetizer">Appetizer</option>
            <option value="MainDish">Main dish</option>
            <option value="Dessert">Dessert</option>
          </select>
        </div>
      </div>
  );
}

export default RecipeProperties;
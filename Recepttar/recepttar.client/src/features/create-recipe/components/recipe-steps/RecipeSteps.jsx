import { ReactSortable } from "react-sortablejs";
import { useState } from "react";

function RecipeSteps({ errors }){
    const [steps, setSteps] = useState([
        { id: 1, text: "" },
        { id: 2, text: "" }
      ]);

    const addStep = () => {
        const newId = steps.length > 0 ? Math.max(...steps.map(step => step.id)) + 1 : 1;
        setSteps([...steps, { id: newId, text: "" }]);
    }

    const updateStepText = (index, text) => {
        const updatedSteps = [...steps];
        updatedSteps[index].text = text;
        setSteps(updatedSteps);
    }

    const removeStep = (id) => {
        setSteps(steps.filter(step => step.id !== id));
    }

    return (
        <div className="card container m-0 mt-3 p-3 shadow w-min-100 main-dark-green text-light mb-3">
            <h3 className="text-decoration-underline">Instructions</h3>
            <div className={"p-2 bg-danger text-white text-start " + (errors?.Steps == null ? "d-none" : "")}>{errors?.Steps?.[0]}&nbsp;</div>
            <ReactSortable
                list={steps}
                setList={setSteps}
                animation={150}
                className="list-group w-100 pe-5 border-0"
                >
                {steps.map((step, index) => (
                    <div className="d-flex gap-2 align-items-center m-2 w-100">
                        <span className="border border-white border-3 rounded-circle d-flex justify-content-center align-items-center fs-5 me-2" style={{ width: "40px", height: "40px" }}>{index + 1}</span>
                        <input type="hidden" name={`Steps[${index}].StepNumber`} value={step.id} />
                        <div className="w-100">
                            <div className={"p-2 bg-danger text-white text-start " + (errors?.[`Steps[${index}].StepDescription`]?.[0] == null ? "d-none" : "")}>{errors?.[`Steps[${index}].StepDescription`]?.[0]}&nbsp;</div>
                            <div key={step.id} className="list-group-item w-100 rounded-3 d-flex align-items-center">
                                <textarea className="form-control border-0" name={`Steps[${index}].StepDescription`} defaultValue={step.text} onChange={(e) => updateStepText(index, e.target.value)} style={{ resize: 'none' }} rows={2} />
                                <a onClick={() => removeStep(step.id)}><i className="bi bi-trash3 text-danger fs-3 ms-2"></i></a>
                            </div>
                        </div>
                    </div>
                ))}
            </ReactSortable>
            <button onClick={(e) => {e.preventDefault(); addStep();}} className="w-75 ms-auto me-auto mt-2 d-flex align-items-center gap-2 justify-content-center rounded-4 fw-bold border-0"><i className="bi bi-plus-circle fs-3 fw-bold text-black"></i>Add option</button>
        </div>
    );
}

export default RecipeSteps;
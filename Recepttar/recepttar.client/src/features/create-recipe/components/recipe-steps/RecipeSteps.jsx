import { ReactSortable } from "react-sortablejs";
import { useState, useEffect } from "react";
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';

function RecipeSteps({ errors, recipeData, steps, setSteps, t }){
    const [loaded, setLoaded] = useState(false);

    const addStep = () => {
        const newId = steps.length > 0 ? Math.max(...steps.map(step => step.id)) + 1 : 1;
        setSteps([...steps, { id: newId, text: "" }]);
    }

    const updateStepText = (index, text) => {
        let trimmed = text.trimStart();
        if(trimmed.length > 400) trimmed = trimmed.slice(0, 400);

        const updatedSteps = [...steps];
        updatedSteps[index].text = trimmed;
        setSteps(updatedSteps);
    };

    const removeStep = (id) => {
        setSteps(steps.filter(step => step.id !== id));
    }

    useEffect(() => {
        if(recipeData != null && !loaded){
            const loadedSteps = recipeData.steps.map((step, index) => ({ id: index + 1, text: step.stepDescription }));
            setSteps(loadedSteps);
            setLoaded(true);
        }
    }, [recipeData]);

    return (
        <div className="card container m-0 mt-3 p-3 shadow w-min-100 main-dark-green text-light mb-3">
            <div className="d-flex gap-2 align-items-center">
                <h3 className="text-decoration-underline">{t("recipeViewPage.instructions")}</h3>
                <OverlayTrigger placement="right" overlay={<Tooltip id="tooltip-top">{t("createEditRecipePage.recipeStepTooltip")}</Tooltip>}>
                    <i className="bi bi-question-circle fs-5"></i>
                </OverlayTrigger>
            </div>
            <div className={"p-2 bg-danger text-white text-start " + (errors?.Steps == null ? "d-none" : "")}>{errors?.Steps?.[0]}&nbsp;</div>
            <ReactSortable
                list={steps}
                setList={setSteps}
                animation={150}
                handle=".drag-handle"
                delay={0}
                delayOnTouchStart={false}
                touchStartThreshold={0}
                className="list-group w-100 pe-md-5 pe-3 border-0"
            >
                {steps.map((step, index) => (
                    <div className="d-flex gap-2 align-items-center m-2 w-100" key={`Step${index}`}>
                        <span className="drag-handle border border-white border-3 rounded-circle d-md-flex justify-content-center align-items-center fs-5 me-2 d-none" style={{ width: "40px", height: "40px", cursor: "grab" }}>{index + 1}</span>
                        <span className="drag-handle fs-1 d-md-none" style={{ cursor: "grab" }}><i className="bi bi-grip-vertical"></i></span>
                        <input type="hidden" name={`Steps[${index}].StepNumber`} value={index + 1} />
                        <div className="w-100">
                            <div className={"p-2 bg-danger text-white text-start " + (errors?.[`Steps[${index}].StepDescription`]?.[0] == null ? "d-none" : "")}>{errors?.[`Steps[${index}].StepDescription`]?.[0]}&nbsp;</div>
                            <div key={step.id} className="list-group-item w-100 rounded-3 d-flex align-items-center">
                                <textarea className="form-control border-0" name={`Steps[${index}].StepDescription`} value={step.text} onChange={(e) => updateStepText(index, e.target.value)} style={{ resize: "none", fieldSizing: "content" }} rows={5} />
                                <a onClick={() => removeStep(step.id)}><i className="bi bi-trash3 text-danger fs-3 ms-2"></i></a>
                            </div>
                        </div>
                    </div>
                ))}
            </ReactSortable>
            <button onClick={(e) => {e.preventDefault(); addStep();}} className="w-75 ms-auto me-auto mt-2 d-flex align-items-center gap-2 justify-content-center rounded-4 fw-bold border-0"><i className="bi bi-plus-circle fs-3 fw-bold text-black"></i>{t("createEditRecipePage.addStep")}</button>
        </div>
    );
}

export default RecipeSteps;
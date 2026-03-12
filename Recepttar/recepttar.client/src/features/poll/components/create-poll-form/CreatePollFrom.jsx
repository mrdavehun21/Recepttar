import usePollForm from "../../hooks/usePollForm";
import './CreatePollForm.css';

export default function CreatePollForm({isFormOpen, openForm, preData = [null, null], errorMessage, children}) {
    const { options, handleSubmit, handleAddOption } = usePollForm(preData, errorMessage);

    return (
        <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center poll-form-background z-index-max" >
            <form onSubmit={handleSubmit} className="p-4 rounded-4 w-75 polls-bg-additional-5 w-min-320px w-max-800px">
                <h2 className="text-decoration-underline">Question</h2>
                <input name="Question" type="text" className="border border-black form-control mb-3 rounded-4 p-2" placeholder="Enter your question here..." defaultValue={(preData?.id == undefined) ? "" : preData?.question}/>

                <h2 className="text-decoration-underline">Options</h2>
                {options.map((option, index) => (
                <div className="d-flex border border-black form-control mb-3 rounded-4">
                    <div className="d-flex align-items-center p-1 border-end border-black">
                    <span className="fw-bold me-2">{index + 1}</span>
                    </div>
                    <input name={`options[${index}].OptionText`} key={index} type="text" className="d-block form-control border-0 shadow-none" placeholder={`Option ${index + 1}`} defaultValue={(preData?.options?.[index]?.optionText == undefined) ? "" : preData?.options?.[index]?.optionText } />
                </div>
                ))}

                <button onClick={handleAddOption} className="w-100 d-flex align-items-center gap-2 justify-content-center rounded-4 polls-bg-additional-7 text-light fw-bold"><i className="bi bi-plus-circle fs-3 fw-bold text-black"></i>Add option</button>

                <div className="d-flex justify-content-sm-end justify-content-center gap-3 w-100 mt-4">
                <button type="button" onClick={openForm} className="d-block btn rounded-4 btn-light fw-bold border border-black ps-4 pe-4">Cancel</button>
                <button type="submit" className="d-block btn rounded-4 fw-bold polls-bg-additional-8 border border-black text-light ps-4 pe-4">{(preData?.id == undefined) ? "Submit" : "Update"}</button>
                </div>
            </form>
            {children}
        </div>
    );
}
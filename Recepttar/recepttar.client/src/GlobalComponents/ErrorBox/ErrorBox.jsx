import './ErrorBox.css';

function ErrorBox({ errorCode = '', errorMessage = "The data you're trying to read is not found!", visible = false, clearError, closeError }) {
    return (
        <div className={"p-4 w-100 polls-bg-additional-5 rounded-2 shadow BoxBody position-absolute " + (!visible ? "d-none" : "d-flex")}>
            <div>
                <h3>Error! 
                { 
                    errorCode === '' ? null : "(" + errorCode + ")"
                }
                </h3>
                <hr className="w-100" />
                <h5>An error ocured: {errorMessage}</h5>
                <input className="d-block ms-auto mt-4 polls-bg-additional-8 text-white border border-black p-2 rounded-3" type="submit" value="Ok" style={{ width: "100px" }} onClick={() => { closeError(false); clearError('');}}/>
            </div>
        </div>
  );
}

export default ErrorBox;
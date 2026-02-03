import './ErrorBox.css';

function ErrorBox({ errorCode = 404, errorMessage = "The data you're trying to read is not found!" }) {
    return (
        <div className="p-4 ms-auto me-auto bg-light rounded-2 shadow BoxBody">
            <h3>Error! ({errorCode})</h3>
            <hr className="w-100" />
            <h5>An error ocoured: {errorMessage}</h5>
        </div>
  );
}

export default ErrorBox;
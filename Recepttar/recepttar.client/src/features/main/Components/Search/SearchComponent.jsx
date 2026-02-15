import './SearchComponent.css';

function SearchComponent({ setSearch }) {
    return (
        <div className="d-flex flex-column align-items-center order-3 order-lg-2 mx-lg-auto">
            <h3 className="mt-0 mb-3 ms-0 text-decoration-underline color-neutral-100 fs-3">Search</h3>
            <div className="ms-auto me-auto d-flex rounded-5 d-flex border border-dark overflow-hidden">
                <input type="text" className="border-0 p-2 d-block bg-white" onKeyUp={(e) => setSearch(e.target.value)} />
                <button className="border-0 p-2 d-block bg-white"><i className="bi bi-search"></i></button>
            </div>
        </div>
  );
}

export default SearchComponent;
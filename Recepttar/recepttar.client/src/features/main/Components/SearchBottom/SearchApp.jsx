import './SearchBottom.css'

function SearchApp() {
    return (
        <div className="d-sm-flex bg-light m-3 p-3 rounded-2 shadow">
            <div className="w-50 LeftSegment">
                <h3>Search by tags</h3>
                <div className="d-flex flex-wrap gap-3 mt-4">
                    <button className="OptionButton">Vegan</button>
                    <button className="OptionButton">Easy</button>
                    <button className="OptionButton">Medium</button>
                    <button className="OptionButton">Hard</button>
                    <button className="OptionButton">Appetizer</button>
                    <button className="OptionButton">Main dish</button>
                    <button className="OptionButton">Dessert</button>
                </div>
            </div>
            <div className="w-50 RightSegment">
                <div style={{ height: "45px" }} className="ps-4 w-100 overflow-hidden rounded-1 RightSegment d-flex justify-content-sm-end">
                    <input type="text" name="Search" id="SearchBox" className="h-100 w-50 border-0 ContentBox" />
                    <button className="h-100 border-0">SearchBtn</button>
                </div>
            </div>
      </div>
  );
}

export default SearchApp;
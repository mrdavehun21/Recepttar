function Card({ data }) {
    return (
        <div className="card" style={{ width: '30%' }}>
            <img className="card-img-top img-fluid" src={"https://localhost:7035" + data.dishPicture} alt={data.title} style={{ height: '200px', objectFit: 'cover' }} />
            <div className="card-body">
                <h4 className="card-title">{data.title}</h4>
                <h6 className="fw-bold">Description:</h6>
                <p className="card-text" style={{ textIndent: '0.5em', fontStyle: 'italic' }}>{data.description}</p>
            </div>
          <ul className="list-group list-group-flush list-unstyled">
              <li className="ms-3">Serving: { data.servings } person</li>
              <li className="ms-3">Difficulty: { data.difficulty }</li>
              <li className="ms-3">Time: { data.timeMinutes } minutes</li>
            </ul>
            <div className="card-body text-center">
                <a href="#" className="w-100">Take a look</a>
            </div>
      </div>
  );
}

export default Card;
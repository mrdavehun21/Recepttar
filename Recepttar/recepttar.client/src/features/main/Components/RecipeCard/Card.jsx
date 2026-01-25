import { Link } from "react-router-dom";
import EmptyHeart from "../../../../assets/emptyHeart.svg";
import FilledHeart from "../../../../assets/fullHeart.svg";

function Card({ data }) {
    function ChangeFavouriteState(e, recipeId) {
        e.stopPropagation();
        alert(recipeId)
    }

    return (
        <Link to={`/recipe/${data.id}`} style={{ textDecoration: 'none', color: 'inherit', width: '30%', maxWidth: "320px", minWidth: "250px", cursor: 'pointer' }}>
            <div className="card shadow">
                <img
                    className="card-img-top img-fluid"
                    src={`https://localhost:7035/${data.dishPicture}`}
                    alt={data.title}
                    style={{ height: '200px', objectFit: 'cover' }}
                />
                <div className="position-absolute" style={{ width: "30px", height: "30px", right: "10px", top: "10px" }} onClick={() => ChangeFavouriteState(e, data.id)}>
                    <img src={EmptyHeart} className="w-100"/>
                </div>
                <div className="card-body">
                    <h4 className="card-title">{data.title}</h4>
                </div>
                <ul className="list-group list-group-flush list-unstyled">
                    <div className="d-flex justify-content-between p-3">
                        <li className="card-title">{data.averageRating} star(s)</li>
                        <li className="card-title">{data.reviewCount} review(s)</li>
                    </div>
                </ul>
            </div>
        </Link>
    );
}

export default Card;
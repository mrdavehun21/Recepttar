import { Link } from "react-router-dom";
import { useFavorites } from "../../hooks/useFavorites";
import EmptyHeart from "../../../../assets/emptyHeart.svg";
import FilledHeart from "../../../../assets/fullHeart.svg";

function Card({ data }) {
    const { isFavorite, toggleFavorite } = useFavorites(data.isFavorite);

    return (
        <div
            className="card shadow"
            style={{
                width: '30%',
                maxWidth: '320px',
                minWidth: '250px',
                cursor: 'pointer',
                height: '-webkit-fill-available',
                position: 'relative'
            }}
        >
            <div
                className="position-absolute bg-light p-1 rounded-2"
                style={{ width: "40px", height: "40px", right: "10px", top: "10px", zIndex: 10 }}
                onClick={(e) => { e.stopPropagation(); toggleFavorite(data.id); }}
            >
                <img src={isFavorite ? FilledHeart : EmptyHeart} className="w-100" />
            </div>

            <Link
                to={`/recipe/${data.id}`}
                style={{ textDecoration: 'none', color: 'inherit', maxWidth: "320px", minWidth: "250px", cursor: 'pointer', height: '100%' }}
            >
                <div className="card h-100">
                    <img
                        className="card-img-top img-fluid"
                        src={`https://localhost:7035/${data.dishPicture}`}
                        alt={data.title}
                        style={{ height: '200px', objectFit: 'cover' }}
                    />
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
        </div>
    );
}

export default Card;

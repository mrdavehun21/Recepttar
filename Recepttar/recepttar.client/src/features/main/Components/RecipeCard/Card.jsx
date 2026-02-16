import { Link } from "react-router-dom";
import { useFavorites } from "../../hooks/useFavorites";
import EmptyHeart from "../../../../assets/emptyHeart.svg";
import FilledHeart from "../../../../assets/fullHeart.svg";
import './Card.css'

function Card({ data, allowFavorites = false }) {
    const { isFavorite, toggleFavorite } = useFavorites(data.isFavorite);

    const StarRating = ({ rating, maxStars = 5 }) => {
        const roundedRating = Math.ceil(rating);

        return (
            <>
                {[...Array(maxStars)].map((_, index) => (
                    <i
                        key={index}
                        className={`me-1 bi ${index < roundedRating ? "bi-star-fill star-additional-9" : "bi-star-fill star-additional-10"
                            }`}
                    ></i>
                ))}
            </>
        );
    };


    return (
        <div className="card border-0 shadow MainCard">
            {
                allowFavorites ? (
                    <div
                        className="position-absolute bg-light p-1 rounded-2 CardHeart"
                        onClick={(e) => { e.stopPropagation(); toggleFavorite(data.id); }}
                    >
                        <img src={isFavorite ? FilledHeart : EmptyHeart} className="w-100" />
                    </div>
                ) : (
                    <div></div>
                )
            }

            <Link
                to={`/recipe/${data.id}`} className="CardLink">
                <div className="card border-0 h-100">
                    <img
                        className="card-img-top img-fluid CardImage"
                        src={`https://localhost:7035/${data.dishPicture}`}
                        alt={data.title}
                    />
                    <div className="card-body">
                        <h4 className="card-title text-decoration-underline font-neutral-100 fw-bold">{data.title}</h4>
                        <p className=".font-neutral-100 mt-4 fw-bold">{
                            (data.description.length > 150) ? (data.description.substring(0, 150) + "...") :
                                (data.description)
                        }</p>
                    </div>
                    <div className="border-top border-dark list-group list-group-flush list-unstyled">
                        <div className="d-flex justify-content-between p-3">
                            <div>
                                <StarRating rating={data.averageRating} />
                                <span className="ms-2">
                                    {Math.round(data.averageRating * 10) / 10}
                                </span>
                            </div>
                            <div>{data.reviewCount} Review{
                                (data.reviewCount > 1) ? "s" : ""
                            }</div>
                        </div>
                    </div>
                </div>
            </Link>
        </div>
    );
}

export default Card;

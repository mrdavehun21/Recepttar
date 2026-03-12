import { useEffect, useState } from "react";
import { useFavorites } from "../../hooks/useFavorites";
import { ImageAvailable } from '../../../../shared/hooks/usePictureChecker';
import EmptyHeart from "../../../../assets/emptyHeart.svg";
import FilledHeart from "../../../../assets/fullHeart.svg";
import './Card.css';

function Card({ data, allowFavorites = true }) {
    const API_BASE = import.meta.env.VITE_API_URL;

    const { isFavorite, toggleFavorite } = useFavorites(data.isFavorite);
    const [imageExists, setImageExists] = useState(false);

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

    useEffect(() => {
        async function checkImage() {
            const exists = await ImageAvailable(`${API_BASE}/${data.dishPicture}`);
            setImageExists(exists);
        }

        checkImage();
    }, [data.dishPicture]);

    return (
        <div className="card border-0 shadow MainCard">
            {
                allowFavorites ? (
                    <div
                        className="position-absolute bg-light p-1 rounded-2 CardHeart"
                        onClick={(e) => { e.stopPropagation(); toggleFavorite(data.recipeId); }}
                    >
                        <img src={isFavorite ? FilledHeart : EmptyHeart} className="w-100" />
                    </div>
                ) : (
                    null
                )
            }

            <a
                href={`/recipe/${data.recipeId}`} className="CardLink">
                <div className="card border-0 h-100 overflow-hidden">
                    {
                        (imageExists == true) ? (
                            <img
                                src={`${API_BASE}/${data.dishPicture}`}
                                className="w-100 object-cover h-200px"
                            />
                        ) : (
                            <div className="w-100 bg-secondary h-200px">

                            </div>
                        )
                    }
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
                                    {data.averageRating}
                                </span>
                            </div>
                            <div>{data.reviewCount} Review{
                                (data.reviewCount > 1) ? "s" : ""
                            }</div>
                        </div>
                    </div>
                </div>
            </a>
        </div>
    );
}

export default Card;

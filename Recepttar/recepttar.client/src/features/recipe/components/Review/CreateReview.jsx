import { useState } from 'react';
import { addReviewByRecipeId } from '../../api/recipe.api';
import { useIsLoggedIn } from '../../../../shared/hooks/useLoginState';
import "../../../../shared/styles/ColorPalette.css";

const CreateReview = ({ recipeId, onReviewAdded, t }) => {
    const [stars, setStars] = useState(0);
    const [hoveredStar, setHoveredStar] = useState(0);
    const [comment, setComment] = useState('');
    const { isLoggedIn } = useIsLoggedIn();
    const [error, setError] = useState(null);

    const MAX_CHARS = 100;
    const remaining = MAX_CHARS - comment.length;

    const handleSubmit = async () => {
        if (stars === 0) return setError('Please select a star rating.');
        if (!comment.trim()) return setError('Please write a comment.');

        setError(null);
        try {
            await addReviewByRecipeId(recipeId, { stars, comment });
            setStars(0);
            setComment('');
            onReviewAdded?.();
        } catch (e) {
            setError(`Failed to submit review. ${e.response.data}`);
        }
    };

    if (isLoggedIn === null) return null;

    if (!isLoggedIn) {
        return (
            <div className="card mb-3 text-center text-muted py-3" style={{ border: 'none', borderRadius: '12px' }}>
                <div className="card-body">
                    <i className="bi bi-lock fs-4 mb-2 d-block" />
                    <p className="mb-1 fw-semibold">Want to leave a review?</p>
                    <p className="small mb-0">
                        Please <a href="/login" className="text-danger">log in</a> to share your thoughts about this recipe.
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="card mb-3" style={{ border: 'none', borderRadius: '12px' }}>
            <div className="card-body">
                <div className="fw-bold mb-2">{t("recipeViewPage.WriteAReview")}</div>

                <div className="d-flex justify-content-center">
                    <div className="mb-2">
                        {[1, 2, 3, 4, 5].map(star => (
                            <i
                                key={star} className={star <= (hoveredStar || stars) ? 'bi bi-star-fill star-additional-9 me-2' : 'bi bi-star star-additional-10 me-2'}
                                style={{cursor: 'pointer'}}
                                onClick={() => setStars(star)}
                                onMouseEnter={() => setHoveredStar(star)}
                                onMouseLeave={() => setHoveredStar(0)}
                            />
                        ))}
                    </div>
                </div>

                <textarea className="form-control review-bg mb-2" placeholder={t("recipeViewPage.shareThoughts")} value={comment}
                    onChange={e => setComment(e.target.value.slice(0, MAX_CHARS))}
                    style={{ resize: 'none' }} rows={5} />

                {error && <div className="text-danger mb-2">{error}</div>}

                <div className="d-flex justify-content-between align-items-center mb-2">
                    <div className={`text - end small mb-2 ${remaining <= 20 ? 'text-danger' : 'text-muted'}`}>
                        {remaining} / {MAX_CHARS}
                    </div>
                    <button className="btn btn-sm btn-danger" onClick={handleSubmit}>{t("recipeViewPage.submitReview")}</button>
                </div>
            </div>
        </div>
    );
};
export default CreateReview;
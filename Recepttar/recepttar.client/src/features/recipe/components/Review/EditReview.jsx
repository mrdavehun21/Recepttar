import { useState } from 'react';
import { updateReviewById } from '../../api/recipe.api';

const EditReview = ({ review, onCancel, onReviewUpdated, t }) => {
    const [stars, setStars] = useState(review.stars);
    const [hoveredStar, setHoveredStar] = useState(0);
    const [comment, setComment] = useState(review.comment);
    const [error, setError] = useState(null);
    const MAX_CHARS = 100;
    const remaining = MAX_CHARS - comment.length;

    const handleSubmit = async () => {
        if (stars === 0) return setError('Please select a star rating.');
        if (!comment.trim()) return setError('Please write a comment.');
        setError(null);
        try {
            await updateReviewById(review.reviewId, { stars, comment });
            onReviewUpdated?.();
        } catch (e) {
            setError(`Failed to update review. ${e.response.data}`);
        }
    };

    return (
        <div className="card mb-3" style={{ border: '1px solid #dee2e6', borderRadius: '12px' }}>
            <div className="card-body">
                <div className="fw-bold mb-2">{t("recipeViewPage.editReviewHeader")}</div>
                <div className="d-flex justify-content-center">
                    <div className="mb-2">
                        {[1, 2, 3, 4, 5].map(star => (
                            <i
                                key={star} className={star <= (hoveredStar || stars) ? 'bi bi-star-fill star-additional-9 me-2' : 'bi bi-star star-additional-10 me-2'}
                                style={{ cursor: 'pointer' }}
                                onClick={() => setStars(star)}
                                onMouseEnter={() => setHoveredStar(star)}
                                onMouseLeave={() => setHoveredStar(0)} />
                        ))}
                    </div>
                </div>
                <textarea className="form-control review-bg mb-2" value={comment}
                    onChange={e => setComment(e.target.value.slice(0, MAX_CHARS))}
                    style={{ resize: 'none' }} rows={5} />

                {error && <div className="text-danger mb-2">{error}</div>}

                <div className="d-flex justify-content-between align-items-center mb-2">
                    <div className={`text-end small mb-2 ${remaining <= 20 ? 'text-danger' : 'text-muted'}`}>
                        {remaining} / {MAX_CHARS}
                    </div>
                    <div className="d-flex gap-2">
                        <button className="btn btn-sm btn-outline-secondary" onClick={onCancel}>{t("recipeViewPage.cancel")}</button>
                        <button className="btn btn-sm btn-danger" onClick={handleSubmit}>{t("recipeViewPage.saveChanges")}</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default EditReview;
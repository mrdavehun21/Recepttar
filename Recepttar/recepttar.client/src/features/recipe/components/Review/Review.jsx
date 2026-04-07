import { useIsLoggedIn } from '../../../../shared/hooks/useLoginState';
import { useState, useRef, useEffect } from 'react';
import CreateReview from './CreateReview';
import EditReview from './EditReview';
import ReviewCard from './ReviewCard';
import { deleteReviewById, getRecipeById } from '../../api/recipe.api';
import { Modal } from 'bootstrap';

const Reviews = ({ reviews, recipeId, onReviewAdded, t }) => {
    const [editingReview, setEditingReview] = useState(null);
    const [reviewToDelete, setReviewToDelete] = useState(null);
    const [deleteError, setDeleteError] = useState(null);
    const modalRef = useRef(null);
    const bsModalRef = useRef(null);
    const { isLoggedIn, profileData } = useIsLoggedIn();
    const [recipe, setRecipe] = useState(null);
    const canReview = isLoggedIn === true && !reviews.some(r => r.userId === profileData?.id) && profileData?.id !== recipe?.authorId;

    useEffect(() => {
        getRecipeById(recipeId).then(res => setRecipe(res.data));
    }, [recipeId]);

    useEffect(() => {
        if (modalRef.current) {
            bsModalRef.current = new Modal(modalRef.current);
        }
    }, []);

    const handleEditClick = (review) => {
        setEditingReview(review);
    };

    const handleDeleteClick = (review) => {
        setReviewToDelete(review);
        setDeleteError(null);
        bsModalRef.current?.show();
    };

    const handleConfirmDelete = async () => {
        try {
            await deleteReviewById(reviewToDelete.reviewId);
            bsModalRef.current?.hide();
            onReviewAdded();
        } catch {
            setDeleteError("Failed to delete review.");
        }
    };

    return (
        <div className="card mt-3 reviews-bg" style={{ border: 'none' }}>
            <div className="card-body">
                <h5 className="card-title fw-bold text-decoration-underline">{t("recipeViewPage.reviews")}</h5>

                {editingReview ? (
                    <EditReview
                        review={editingReview}
                        onCancel={() => setEditingReview(null)}
                        onReviewUpdated={() => {
                            setEditingReview(null);
                            onReviewAdded();
                        }}
                        t={t}
                    />
                ) : canReview ? (
                    <CreateReview recipeId={recipeId} onReviewAdded={onReviewAdded} t={t} />
                ) : null}

                {reviews.map((review, index) => (
                    <ReviewCard
                        key={index}
                        review={review}
                        onEditClick={handleEditClick}
                        onDeleteClick={handleDeleteClick}
                    />
                ))}
            </div>

            <div className="modal fade" ref={modalRef} tabIndex="-1">
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Delete Review</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" />
                        </div>
                        <div className="modal-body">
                            Are you sure you want to delete your review?
                            {deleteError && <div className="text-danger mt-2">{deleteError}</div>}
                        </div>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                            <button className="btn btn-danger" onClick={handleConfirmDelete}>Delete</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Reviews;
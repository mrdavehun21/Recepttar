import { Link, useNavigate } from 'react-router-dom';
import { useState, useRef, useEffect } from 'react';
import defaultAvatar from "../../../../assets/default-avatar.jpg";
import { useIsLoggedIn } from '../../../../shared/hooks/useLoginState';
import useRecipeInfo from '../../hooks/useRecipeInfo';
import { deleteRecipeById } from '../../api/recipe.api';
import { Modal } from 'bootstrap';

const API_BASE = import.meta.env.VITE_API_URL;

const RecipeInfo = ({ recipe, author, reviews, t }) => {
    const avg = reviews?.length ? (reviews.reduce((s, r) => s + r.stars, 0) / reviews.length).toFixed(1) : null;
    const { isLoggedIn, profileData } = useIsLoggedIn();
    const { formatTime, getDifficultyColor, getDifficultyIcon, getDishTypeIcon, renderStars } = useRecipeInfo();
    const [deleteError, setDeleteError] = useState(null);
    const modalRef = useRef(null);
    const bsModalRef = useRef(null);
    const navigate = useNavigate();

    useEffect(() => {
        if (modalRef.current) {
            bsModalRef.current = new Modal(modalRef.current);
        }
    }, []);

    const isOwner = isLoggedIn === true && profileData?.id === recipe?.authorId;

    const handleDeleteClick = () => {
        setDeleteError(null);
        bsModalRef.current?.show();
    };

    const handleConfirmDelete = async () => {
        try {
            await deleteRecipeById(recipe.recipeId);
            bsModalRef.current?.hide();
            navigate('/');
        } catch {
            setDeleteError(`Failed to delete '${recipe?.title}' recipe`);
        }
    };

    return (
        <>
            {recipe.dishPicture && (
                <img src={`${API_BASE}/${recipe.dishPicture}`} alt={recipe.title}
                    className="w-100 rounded-4 mb-3" style={{ height: '340px', objectFit: 'cover' }} />
            )}
            
            <div className="mb-2">
                <h1 className="fw-bold text-center text-decoration-underline mb-2">
                    {recipe.title}
                </h1>
                <div className="d-flex justify-content-center gap-2">
                    <button type="button" className="btn btn-success print"
                        onClick={() => { document.title = `Recepttar_${recipe.title}`; window.print(); }}>
                        <i className="bi bi-printer" />
                    </button>
                    {isOwner && (
                        <>
                            <Link to={`/recipe/${recipe.recipeId}/editrecipe`} className="btn btn-warning">
                                <i className="bi bi-pencil" />
                            </Link>
                            <button type="button" className="btn btn-danger" onClick={handleDeleteClick}>
                                <i className="bi bi-trash" />
                            </button>
                        </>
                    )}
                </div>
            </div>

            {avg && (
                <div className="d-flex flex-column align-items-center mb-3">
                    <div className="d-flex gap-1">{renderStars(avg)}</div>
                    <div className="text-muted mt-1">{avg} &nbsp;-&nbsp; {reviews.length} {t("homePage.reviewPlural")}</div>
                </div>
            )}

            <p className="text-center text-muted mb-3">
                <i className="bi bi-calendar3 me-1" />
                {new Date(recipe.createdAt).toLocaleDateString('hu-HU')}
            </p>

            <div className="d-flex justify-content-center gap-2 flex-wrap mb-4">
                {recipe.isVegan && (
                    <span className="badge rounded-pill bg-success px-3 py-2">
                        <i className="bi bi-leaf-fill me-1" />{t("homePage.tagsList.vegan")}
                    </span>
                )}
                <span className={`badge rounded-pill px-3 py-2 ${recipe.isExpensive ? 'bg-warning text-dark' : 'bg-success'}`}>
                    <i className={`bi ${recipe.isExpensive ? 'bi-coin' : 'bi-cash'} me-1`} />
                    {recipe.isExpensive ? t("homePage.tagsList.expensive") : t("homePage.tagsList.cheap")}
                </span>
                <span className={`badge rounded-pill bg-${getDifficultyColor(recipe.difficulty)} px-3 py-2`}>
                    <i className={`${getDifficultyIcon(recipe.difficulty)} me-1`} />
                    {t(`homePage.tagsList.${recipe.difficulty.toLowerCase()}`)}
                </span>
            </div>

            <div className="row g-3 mb-4">
                <div className="col-6 col-md-3">
                    <div className="card h-100 text-center border-0 shadow-sm rounded-3">
                        <div className="card-body py-3 px-2">
                            <div className="fw-bold text-decoration-underline mb-1">{t("recipeViewPage.totalTime")}</div>
                            <div className="text-muted">{formatTime(recipe.timeMinutes, t)}</div>
                        </div>
                    </div>
                </div>
                <div className="col-6 col-md-3">
                    <div className="card h-100 text-center border-0 shadow-sm rounded-3">
                        <div className="card-body py-3 px-2">
                            <div className="fw-bold text-decoration-underline mb-1">{t("recipeViewPage.servings")}</div>
                            <div className="text-muted">{recipe.servings}</div>
                        </div>
                    </div>
                </div>
                <div className="col-6 col-md-3">
                    <div className="card h-100 text-center border-0 shadow-sm rounded-3">
                        <div className="card-body py-3 px-2">
                            <div className="fw-bold text-decoration-underline mb-1">{t("recipeViewPage.foodType")}</div>
                            <div className="text-muted" >
                                <i className={`${getDishTypeIcon(recipe.type)} me-1`} />{t(`homePage.tagsList.${recipe.type.toLowerCase()}`)}
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-6 col-md-3">
                    <div className="card h-100 text-center border-0 shadow-sm rounded-3">
                        <div className="card-body py-3 px-2">
                            <div className="fw-bold text-decoration-underline mb-2">{t("recipeViewPage.author")}</div>
                            <Link to={`/profile/${author?.id}`} className="d-flex flex-column align-items-center justify-content-center gap-2 text-decoration-none text-dark">
                                {author?.profilePicture ? (
                                    <img src={`${API_BASE}/${author.profilePicture}`} alt={author?.fullName} className="rounded-circle flex-shrink-0"
                                        style={{ width: '30px', height: '30px', objectFit: 'cover' }}
                                        onError={(e) => { e.target.onerror = null; e.target.src = defaultAvatar; }} />
                                ) : (
                                    <div className="rounded-circle bg-secondary d-flex align-items-center justify-content-center flex-shrink-0"
                                        style={{ width: '30px', height: '30px' }}>
                                        <i className="bi bi-person-fill text-white" />
                                    </div>
                                )}
                                <div className="text-center" style={{ minWidth: 0 }}>
                                    <div style={{
                                        display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical', overflow: 'hidden'
                                    }}>{author?.fullName}</div>
                                    <div className="text-muted mt-1 text-decoration-underline" style={{
                                        overflow: 'hidden', whiteSpace: 'nowrap', textOverflow: 'ellipsis'
                                    }}>{t(`userRank.${author?.rank}`)}</div>
                                </div>
                            </Link>
                        </div>
                    </div>
                </div>
            </div>

            <div className="card border-0 shadow-sm rounded-4 mb-4">
                <div className="card-body p-4">
                    <h5 className="fw-bold text-decoration-underline mb-3">{t("recipeViewPage.description")}</h5>
                    <p className="text-muted mb-0">{recipe.description}</p>
                </div>
            </div>

            <div className="card border-0 rounded-4 mb-4 main-green">
                <div className="card-body p-4">
                    <h5 className="fw-bold text-decoration-underline text-white mb-4">{t("recipeViewPage.instructions")}</h5>
                    <div className="d-flex flex-column gap-3">
                        {recipe.steps?.map((step) => (
                            <div key={step.stepNumber} className="d-flex align-items-start gap-3">
                                <span className="text-light border border-white border-3 rounded-circle d-flex justify-content-center align-items-center fs-5 flex-shrink-0 main-green fw-bold"
                                    style={{ width: '38px', height: '38px', marginTop: '4px' }}>
                                    {step.stepNumber}
                                </span>
                                <div className="bg-white rounded-3 p-3 flex-grow-1 shadow-sm">
                                    <p className="mb-0">
                                        {step.stepDescription}
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            <div className="modal fade" ref={modalRef} tabIndex="-1">
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{t("recipeViewPage.deleteRecipeHeader")}</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" />
                        </div>
                        <div className="modal-body">
                            {t("recipeViewPage.deleteRecipeMessage")} <strong>{recipe?.title}</strong>
                            {deleteError && <div className="text-danger mt-2">{deleteError}</div>}
                        </div>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" data-bs-dismiss="modal">{t("recipeViewPage.cancel")}</button>
                            <button className="btn btn-danger" onClick={handleConfirmDelete}>{t("recipeViewPage.deleteButton")}</button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default RecipeInfo;

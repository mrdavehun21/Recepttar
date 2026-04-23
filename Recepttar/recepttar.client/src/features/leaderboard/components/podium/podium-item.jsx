import './podium-item.css';

export default function PodiumItem({ profile, index, t, nav }) {
    const baseURL = import.meta.env.VITE_API_URL;

    return (
        <div className={`podium-${index} d-flex flex-column align-items-center justify-content-end hover-click`} onClick={() => nav(`/profile/${profile.userId}`)}>
            <img className="rounded-circle" src={baseURL + "/" + profile?.profilePicture} alt={profile?.fullName} style={{width: "90px", height: "90px"}} />
            <span className="podium-username p-2 fs-4">{profile.fullName}</span>
            <div className="text-end">
                <span className="d-block podium-points fs-5">{profile.recipeCount} {t("leaderboardPage.recipes")}</span>
                <span className="d-block podium-points fs-5">{profile.favoriteCount} <i className="bi bi-heart-fill text-danger"></i></span>
                <div className="d-block podium-points fs-5"><i className="bi bi-star-fill text-warning me-2"></i>{profile.avgRating}</div>
            </div>
            <div className={`podium-step-${index} podium d-flex justify-content-center align-items-center mt-2`}>
                <span className="podium-rank fw-bold" style={{fontSize: "32px"}}>#{index}</span>
            </div>
        </div>
    );
}
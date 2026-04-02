import './podium-item.css';

export default function PodiumItem({ profile, index }) {
    
    return (
        <div className={`podium-${index}`}>
            <div className="podium podium-rank">#{index}</div>
            <div className="podium-username">{profile.fullName}</div>
            <div className="podium-points">{profile.recipeCount} recipe</div>
            <div className="podium-points">{profile.favoriteCount} favorite</div>
            <div className="podium-points">{profile.avgRating} rating</div>
            <div className={`podium-step-${index}`}></div>
        </div>
    );
}
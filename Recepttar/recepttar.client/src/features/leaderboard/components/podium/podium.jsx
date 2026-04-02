import PodiumItem from "./podium-item";

export default function Podium({ profiles }) {
    return (
        <div className="podium-container">
            {profiles.slice(0, 3).map((profile, index) => (
                <PodiumItem key={index} profile={profile} index={index + 1} />
            ))}
        </div>
    );
}
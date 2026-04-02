import PodiumItem from "./podium-item";

export default function Podium({ profiles }) {
    return (
        <div className="podium-container d-flex flex-column flex-md-row gap-5 justify-content-middle mx-auto w-fit mb-3">
            {profiles.slice(0, 3).map((profile, index) => (
                <PodiumItem key={index} profile={profile} index={index + 1} />
            ))}
        </div>
    );
}
import { useEffect, useState } from "react";
import { getLeaderboardData } from "../hook/useGetLeaderboard";
import Podium from "../components/podium/podium";

export default function Leaderboard() {
    const [leaderboardData, setLeaderboardData] = useState([]);

    useEffect(() => {
        async function fetchData() {
            const result = await getLeaderboardData();
            setLeaderboardData(result);
        }
        fetchData();
    }, []);

    return (
        <div className="leaderboard-page">
            <h1>Leaderboard</h1>
            <Podium profiles={leaderboardData}></Podium>
        </div>
    );
}
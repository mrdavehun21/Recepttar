import { useEffect, useState } from "react";
import { getLeaderboardData } from "../hook/useGetLeaderboard";
import OtherUsersTable from "../components/ther-users-table/other-users-table";
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
        <div className="leaderboard-page bg-dark h-100 text-white pt-4">
            <h1 className="fs-1 w-fit mx-auto mb-5">Leaderboard</h1>
            <Podium profiles={leaderboardData}></Podium>
            <OtherUsersTable profiles={leaderboardData.slice(3)}></OtherUsersTable>
        </div>
    );
}
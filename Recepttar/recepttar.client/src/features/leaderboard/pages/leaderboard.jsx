import { useEffect, useState } from "react";
import { getLeaderboardData, getSortOptions } from "../hook/useGetLeaderboard";
import { useTranslation } from 'react-i18next';
import OtherUsersTable from "../components/other-users-table/other-users-table";
import Podium from "../components/podium/podium";

export default function Leaderboard() {
    const [leaderboardData, setLeaderboardData] = useState([]);
    const [sortOption, setSortOption] = useState([]);

    const { t } = useTranslation();

    useEffect(() => {
        async function fetchData() {
            const result = await getLeaderboardData();
            setLeaderboardData(result);
        }
        fetchData();
    }, []);

    useEffect(() => {
        async function fetchData() {
            const result = await getSortOptions();
            setSortOption(result);
        }
        fetchData();
    }, []);

    function handleSortChange(event) {
        const selectedOption = event.target.value;
        async function fetchData() {
            const result = await getLeaderboardData(selectedOption);
            setLeaderboardData(result);
        }
        fetchData();
    }

    return (
        <div className="leaderboard-page bg-dark h-100 text-white pt-4">
            <div className="d-flex flex-column flex-md-row align-items-center justify-content-between mb-4 px-2 px-md-3">
                <h1 className="fs-1 mb-3 mb-md-0 text-center text-md-start">{t("leaderboardPage.leaderboardHeader")}</h1>
                <select className="form-select w-auto" onChange={e => handleSortChange(e)} style={{ minWidth: "160px" }}>
                    {sortOption.map((option, index) => (
                        <option key={index} value={option}>
                            {t(`leaderboardPage.sortBy.${option}`)}
                        </option>
                    ))}
                </select>

            </div>
            <Podium profiles={leaderboardData} t={t}></Podium>
            <OtherUsersTable profiles={leaderboardData.slice(3)}></OtherUsersTable>
        </div>
    );
}
export default function OtherUsersTable({ profiles, nav }) {
    const API_BASE = import.meta.env.VITE_API_URL;

    return ( 
        <table className="w-95 mx-auto mt-5 mb-3">
            <tbody>
                {
                    profiles.map((profile, index) => (
                        <tr key={index} className="border-bottom border-gray hover-click" onClick={() => nav(`/profile/${profile.userId}`)}>
                            <td className="py-4">{index + 4}.</td>
                            <td>
                                <img src={API_BASE + "/" + profile?.profilePicture} alt={profile?.fullName} className="ms-2 rounded-circle" style={{width: "50px", height: "50px"}} />
                            </td>
                            <td className="py-4">{profile.fullName}</td>
                            <td className="d-flex gap-2 justify-content-end py-4">
                                <div>
                                    {profile.favoriteCount} <i className="bi bi-heart-fill text-danger"></i>
                                </div>
                                <div>
                                    <i className="bi bi-star-fill text-warning me-2"></i>{profile.avgRating}
                                </div>
                            </td>
                        </tr>
                    ))
                }
            </tbody>
        </table>
    );
}
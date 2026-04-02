namespace Recepttar.Server.BLL.DTOs.Leaderboard
{
    public class LeaderboardEntryDto
    {
        public int UserId { get; set; }
        public string ProfilePicture { get; set; }
        public string FullName { get; set; }
        public int RecipeCount { get; set; }
        public float AvgRating { get; set; }
        public int FavoriteCount { get; set; }
    }
}

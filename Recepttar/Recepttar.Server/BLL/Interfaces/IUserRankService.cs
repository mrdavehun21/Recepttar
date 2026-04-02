namespace Recepttar.Server.BLL.Interfaces
{
    public interface IUserRankService
    {
        Task EvaluateUserRankAsync(int userId);
    }
}

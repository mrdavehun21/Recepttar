namespace Recepttar.Server.DTO
{
    public class UpdateProfileData
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Bio { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

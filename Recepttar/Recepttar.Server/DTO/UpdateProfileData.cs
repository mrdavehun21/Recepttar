namespace Recepttar.Server.DTO
{
    public class UpdateProfileData
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Bio { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}

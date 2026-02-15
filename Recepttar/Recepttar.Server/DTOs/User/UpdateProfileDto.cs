namespace Recepttar.Server.DTOs.User
{
    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Bio { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}

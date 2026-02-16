using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.DTOs.User;
using Recepttar.Server.Models;
using Recepttar.Server.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FavoriteService _favoriteService;

        public UserController(UserService userService, FavoriteService favoriteService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.RegisterUserAsync(registerDto);

            if(user == null)
            {
                return BadRequest(new { error = "User already exists with this email" });
            }

            return Created(String.Empty, new { message = "User created" });
        }

        [HttpGet("checkEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { error = "Email is required" });
            }

            var exists = await _userService.EmailExistsAsync(email);

            if (exists)
            {
                return Ok(new { message = "Email exists" });
            }

            return NotFound(new { error = "Email not found" });
        }
        
        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            return Ok(new { message = "User is logged in" });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromForm] LogInUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.LoginUserAsync(loginDto);

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            HttpContext.Session.SetInt32(SessionKeys.UserId, user.Id);

            return Ok(new { message = "Successfully logged in" });
        }
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            // Clears every session paramaters
            HttpContext.Session.Clear();

            // Clearing browser-side cookies
            Response.Cookies.Delete(".AspNetCore.Session");

            // Successful logout (Status code 200)
            return Ok(new { message = "Successfully logged out" });
        }

        #region Profile

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var profile = await _userService.GetUserByIdAsync(userId.Value);

            if (profile == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(profile);
        }

        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _userService.UpdateUserProfileAsync(userId.Value, updateDto);

            if (!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            if (result.wasUpdated)
            {
                return Ok(new { message = "User updated" });
            }
            else
            {
                return Ok(new { message = "No changes were made to the user" });
            }
        }
        
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetOthersProfile(int userId)
        {
            var profile = await _userService.GetUserByIdAsync(userId);

            if (profile == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(profile);
        }
        
        [HttpGet("profile/profilepicture")]
        public async Task<IActionResult> ReturnProfilePic()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var image = await _userService.GetUserProfilePictureAsync(userId.Value);

            if (image == null)
            {
                return NotFound(new { error = "Profile picture not found" });
            }

            return File(image, "image/jpeg");
        }
        
        [HttpGet("profile/profilepicture/{userId}")]
        public async Task<IActionResult> ReturnProfilePic(int userId)
        {
            var image = await _userService.GetUserProfilePictureAsync(userId);

            if (image == null)
            {
                return NotFound(new { error = "User or profile piture not found" });
            }

            return File(image, "image/jpeg");
        }

        #endregion Profile

        #region Favorites

        [HttpGet("favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId.Value);

            if (favorites == null)
            {
                return NotFound("No favorites found");
            }

            return Ok(favorites);
        }
        
        [HttpPost("favorites/{recipeId}")]
        public async Task<IActionResult> AddToUserFavorites(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var dto = new CreateFavoriteRecipeDto
            {
                UserId = userId.Value,
                RecipeId = recipeId
            };

            var result = await _favoriteService.AddFavoriteAsync(dto);

            if (!result.success)
            {
                return result.message == "Recipe not found" 
                    ? NotFound(new { error = result.message }) 
                    : Conflict(new { error = result.message });
            }

            return Ok(new { result.message });
        }
        
        [HttpDelete("favorites/{recipeId}")]
        public async Task<IActionResult> RemoveFromFavorites(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _favoriteService.RemoveFavoriteAsync(userId.Value, recipeId);

            if (!result.success)
            {
                return NotFound(new { error = result.message });
            }

            return NoContent();
        }

        #endregion Favorites
    }
}

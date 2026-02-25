using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.DTOs.User;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;

        public UserController(IUserService userService, IFavoriteService favoriteService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto registerDto)
        {
            var user = await _userService.RegisterUserAsync(registerDto);

            if(user == null)
            {
                return BadRequest(Messages.Auth.UserAlreadyExists);
            }

            return Created(String.Empty, Messages.Auth.Created);
        }

        [HttpGet("checkEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(Messages.Auth.EmailRequired);
            }

            var exists = await _userService.EmailExistsAsync(email);

            if (exists)
            {
                return Ok(Messages.Auth.EmailExists);
            }

            return NotFound(Messages.Auth.EmailNotFound);
        }
        
        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            return Ok(Messages.Auth.UserLoggedIn);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromForm] LogInUserDto loginDto)
        {
            var user = await _userService.LoginUserAsync(loginDto);

            if (user == null)
            {
                return Unauthorized(Messages.Auth.InvalidCredentials);
            }

            HttpContext.Session.SetInt32(SessionKeys.UserId, user.Id);

            return Ok(Messages.Auth.UserLoggedIn);
        }
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            // Clears every session paramaters
            HttpContext.Session.Clear();

            // Clearing browser-side cookies
            Response.Cookies.Delete(".AspNetCore.Session");

            return Ok(Messages.Auth.UserLoggedOut);
        }

        #region Profile

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var profile = await _userService.GetUserByIdAsync(userId.Value);

            if (profile == null)
            {
                return NotFound(Messages.Auth.UserNotFound);
            }

            return Ok(profile);
        }

        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, wasUpdated, error) = await _userService.UpdateUserProfileAsync(userId.Value, updateDto);

            if (!success)
            {
                return BadRequest(error);
            }

            if (wasUpdated)
            {
                return Ok(Messages.Auth.Updated);
            }
            else
            {
                return Ok(Messages.Auth.NoChanges);
            }
        }
        
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetOthersProfile(int userId)
        {
            var profile = await _userService.GetUserByIdAsync(userId);

            if (profile == null)
            {
                return NotFound(Messages.Auth.UserNotFound);
            }

            return Ok(profile);
        }
        
        [HttpGet("profile/profilepicture")]
        public async Task<IActionResult> ReturnProfilePic()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (picture, error) = await _userService.GetUserProfilePictureAsync(userId.Value);

            if(error == null)
            {
                return File(picture!, "image/jpeg");
            }

            switch (error)
            {
                case Messages.Auth.UserNotFound:
                    return NotFound(error);

                case Messages.Auth.NoPicture:
                    return NotFound(error);
                    
                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }
        
        [HttpGet("profile/profilepicture/{userId}")]
        public async Task<IActionResult> ReturnProfilePic(int userId)
        {
            var (picture, error) = await _userService.GetUserProfilePictureAsync(userId);

            if (error == null)
            {
                return File(picture!, "image/jpeg");
            }

            switch (error)
            {
                case Messages.Auth.UserNotFound:
                    return NotFound(error);

                case Messages.Auth.NoPicture:
                    return NotFound(error);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }

        #endregion Profile

        #region Favorites

        [HttpGet("favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId.Value);

            return Ok(favorites);
        }
        
        [HttpPost("favorites/{recipeId}")]
        public async Task<IActionResult> AddToUserFavorites(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var dto = new CreateFavoriteRecipeDto
            {
                UserId = userId.Value,
                RecipeId = recipeId
            };

            var (success, message) = await _favoriteService.AddFavoriteAsync(dto);

            if (success)
            {
                return Ok(message);
            }

            switch (message)
            {
                case Messages.Recipe.NotFound:
                    return NotFound(message);
                
                case Messages.Recipe.AlreadyInFavorites:
                    return Conflict(message);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }
        
        [HttpDelete("favorites/{recipeId}")]
        public async Task<IActionResult> RemoveFromFavorites(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, message) = await _favoriteService.RemoveFavoriteAsync(userId.Value, recipeId);

            if (!success)
            {
                return NotFound(message);
            }

            return NoContent();
        }

        #endregion Favorites
    }
}

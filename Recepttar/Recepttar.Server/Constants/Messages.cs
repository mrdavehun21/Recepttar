namespace Recepttar.Server.Constants
{
    public static class Messages
    {
        public static class Server
        {
            public const string Error = "An unexpected error occurred";
        }

        public static class Auth
        {
            public const string Created = "User created";
            public const string UserAlreadyExists = "User already exists with this email";
            public const string InvalidCredentials = "Invalid email or password";
            public const string EmailRequired = "Email is required";
            public const string EmailExists = "Email exists";
            public const string EmailNotFound = "Email not found";

            public const string UserLoggedIn = "User is logged in";
            public const string UserLoggedOut = "User is logged out";
            public const string UserNotFound = "User not found";
            public const string Unauthorized = "You are not authorized";

            public const string Updated = "User profile updated";
            public const string NoChanges = "No changes were made to the user profile";
            public const string NoPicture = "Profile picture not found";
        }

        public static class Recipe
        {
            public const string Created = "Recipe created";
            public const string NotFound = "Recipe not found";
            public const string NotOwner = "You are not allowed to edit this recipe";
            public const string NotOwnerDelete = "You are not allowed to delete this recipe";
            public const string NoChanges = "No changes were made to the recipe";
            public const string Updated = "Recipe updated";
            public const string InvalidData = "Invalid recipe data";

            // Favorite
            public const string AlreadyInFavorites = "Recipe already in favorites";
            public const string AddToFavorites = "Recipe added to favorites";
            public const string NotInFavorites = "Recipe not in favorites";
            public const string RemovedFavorite = "Recipe removed from favorites";

            // AddRecipe
            public const string InvalidTime = "Time must be greater than 0";
            public const string InvalidServings = "Servings must be greater than 0";
            public const string NoIngredients = "At least one ingredient is required";
            public const string NoSteps = "At least one step is required";
            public const string NoPicture = "A dish picture is required";
        }

        public static class Review
        {
            public const string Created = "Review created";
            public const string NotFound = "Review not found";
            public const string InvalidStars = "Invalid stars value";
            public const string AlreadyReviewed = "You have already reviewed this recipe";
            public const string NotOwner = "You are not allowed to edit this review";
            public const string NotOwnerDelete = "You are not allowed to delete this review";
            public const string NoChanges = "No changes were made to the review";
            public const string Updated = "Review updated";
        }

        public static class Poll
        {
            public const string Created = "Poll created";
            public const string NotFound = "Poll not found";
            public const string Voted = "User already voted";
            public const string InvalidOption = "Invalid option";
            public const string Recorded = "Vote recorded";
            public const string NotOwnerDelete = "You are not allowed to delete this poll";
            public const string NotOwner = "You are not allowed to update this poll";

            public const string NoChanges = "No changes were made to the poll";
            public const string Updated = "Poll updated";

            public const string LowRank = "Rank level too low";
            public const string LowOptions = "At least 2 options are needed";
            public const string NoQuestion = "A question is needed";
        }
    }
}

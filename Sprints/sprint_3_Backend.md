# Sprint 3 - Recipes, Reviews, Polls, and Favorites

## Goal

Implement recipe management, search, reviews, polls, and user favorites.
Users should be able to:

- View all recipes and recipe details.
- Create, update, and delete recipes (if authorized).
- Search recipes with multiple filters.
- Add, edit, and delete reviews.
- Vote on polls and view active polls.
- Add and remove favorite recipes.

## Tasks

### Recipes

1. `/recipes`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Successfully gets all recipes | Array of recipe objects |

    **Response Body**

    ```json
    [
        {
            "id": "integer",
            "title": "string",
            "description": "string",
            "difficulty": "string",
            "timeMinutes": "integer",
            "servings": "integer",
            "priceCategory": "string",
            "vegan": "boolean",
            "type": "string"
        }
    ]
    ```

2. `/recipes/create`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | POST   | 201 | Successfully created a new recipe | JSON: `{ "message": "Recipe created" }` |
    | POST   | 400 | Bad request (missing or invalid fields) | JSON: `{ "error": "Invalid request body" }` |
    | POST   | 401  | Unauthorized | JSON: `{ "error": "Unauthorized" }` |

    **Request Body**

    ```json
    {
        "title": "string",
        "description": "string",
        "difficulty": "string",
        "timeMinutes": "integer",
        "servings": "integer",
        "priceCategory": "string",
        "vegan": "boolean",
        "type": "string"
    }
    ```

3. `/recipes/{recipeId}`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | GET    | 200 | Successfully gets a recipe by ID | Recipe object | `recipeId` (recipe ID) |
    | GET    | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    |-|-|-|-|-|
    | PUT    | 200 | Successfully updated recipe | JSON: `{ "message": "Recipe updated" }` | `recipeId` (recipe ID) |
    | PUT    | 400 | Invalid request body | JSON: `{ "error": "Invalid request body" }` | `recipeId` (recipe ID) |
    | PUT    | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` | `recipeId` (recipe ID) |
    | PUT    | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    |-|-|-|-|-|
    | DELETE | 200 | Successfully deleted recipe | JSON: `{ "message": "Recipe deleted successfully" }` | `recipeId` (recipe ID) |
    | DELETE | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` | `recipeId` (recipe ID) |
    | DELETE | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |

### Search

1. `recipes/search`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Successfully searched recipes | Array of recipe objects |
    | GET    | 400 | Invalid or missing search parameters | JSON: `{ "error": "Invalid search parameters" }` |

    **Query Parameters (required)**

    | Parameter       | Type    | Description                 |
    |-----------------|---------|-----------------------------|
    | `difficulty`    | string  | Filter by recipe difficulty |
    | `type`          | string  | Filter by recipe type |
    | `vegan`         | boolean | Filter by vegan status |
    | `priceCategory` | string  | Filter by price category |
    | `search`        | string  | Search by title or description text |

    **Example Request**

    `GET /recipes/search?type=dessert&difficulty=easy&vegan=true&priceCategory=medium&search=chocolate`

### Reviews

1. `/recipes/{recipeId}/reviews`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | GET    | 200 | Successfully gets all reviews for a recipe | Array of review objects | `recipeId` (recipe ID) |
    | GET    | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    | POST   | 201 | Successfully added review | Created review object | `recipeId` (recipe ID) |
    | POST   | 400 | Bad request (invalid stars or missing fields) | JSON: `{ "error": "Invalid request body" }` | `recipeId` (recipe ID) |
    | POST   | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` | `recipeId` (recipe ID) |

    **Request Body**

    ```json
    [
        {
            "stars": "integer",
            "comment": "string"
        }
    ]
    ```

    **Response Body**

    ```json
    [
        {
            "id": "integer",
            "recipeId": "integer",
            "userId": "integer",
            "userName": "string",
            "stars": "integer",
            "comment": "string",
            "createdAt": "DateTime"
        }
    ]
    ```

2. `/reviews/{id}`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | DELETE | 204 | Successfully deleted review | JSON: `{ "message": "Review deleted" }` |
    | DELETE | 403 | Forbidden (not review owner) |JSON: `{ "error": "Forbidden" }` |
    | DELETE | 404 | Review not found | JSON: `{ "error": "Review not found" }` |
    |-|-|-|-|
    | PUT    | 200 | Successfully updated review  | Updated review object |
    | PUT    | 400 | Invalid request body | JSON: `{ "error": "Invalid request body" }` |
    | PUT    | 403 | Forbidden (not review owner) | JSON: `{ "error": "Forbidden" }` |
    | PUT    | 404 | Review not found | JSON: `{ "error": "Review not found" }` |

    **Request Body**

    ```json
    {
        "stars": "integer",
        "comment": "string"
    }
    ```

### Polls

1. `polls/{id}/vote`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-----------|---------------|----------------|
    | POST | 200 | Successfully voted for an option | JSON: `{ "success": true, "message": "Vote recorded" }` | `id` (poll ID) |
    | POST | 400 | Invalid option ID | JSON: `{ "error": "Invalid option" }` | `id` (poll ID) |
    | POST | 404 | Poll not found |  JSON: `{ "error": "Poll not found" }` | `id` (poll ID) |

    **Request Body**

    ```json
    {
      "optionId": 2
    }
    ```

2. `/polls/active`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET | 200 | Returns the current active poll | JSON object |
    | GET | 404 | No active poll found | JSON: `{ "error": "No active poll" }` |

    **Response Body**

    ```json
    [
        {
            "id": "integer",
            "question": "string",
            "isActive": "bool",
            "options": [
                { "id": "integer", "text": "string", "voteCount": "integer" },
                { "id": "integer", "text": "string", "voteCount": "integer" },
                { "id": "integer", "text": "string", "voteCount": "integer" }
            ]
        }
    ]
    ```

### Favorites

1. `/user/favorites`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET | 200 | Gets all favorite recipes for user  as a **list** | JSON array of favourites |

    **Response Body**

    ```json
    [
        {
            "title": "string",
            "difficulty": "string",
            "timeMinutes": "integer",
            "servings": "integer",
            "dishPicture": "string"
        }
    ]
    ```

2. `user/favorites/{recipeId}`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | POST | 201 | Successfully added recipe to favorites | JSON: `{ "message": "Recipe added to favorites" }` | `recipeId` (recipe ID) |
    | POST | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    | DELETE | 204 | Successfully removed recipe from favorites | JSON: `{ "message": "Recipe removed from favorites" }` | `recipeId` (recipe ID) |
    | DELETE | 404 | Recipe not found | JSON: `{ "error": " Recipe not found" }` | `recipeId` (recipe ID) |

## Testing

Manual and functional tests:

Recipes

- Fetch all recipes and a single recipe.
- Create, edit, delete recipes (check auth/validation).
- Search recipes using multiple filter combinations.

Reviews:

- Fetch reviews for a recipe.
- Add, update, delete reviews (check ownership and auth).
- Validate stars range (1–5) and required comment.

Polls:

- Vote on active poll options.
- Check error when voting for invalid option or non-existing poll.
- Fetch current active poll.

Favorites:

- Add/remove recipes from favorites.
- Check listing of favorite recipes.

## Deliverables

- Fully working recipes CRUD with search.
- Review system with ownership validation.
- Poll voting and active poll retrieval.
- User favorites functionality.
- Proper error handling for all endpoints.

## Estimated Duration

~ 2 weeks

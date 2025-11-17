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
            "isExpensive": "boolean",
            "isVegan": "boolean",
            "type": "string",
            "DishPicture": "string URL"
        }
    ]
    ```

2. `/recipes/create`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | POST   | 201 | Successfully created a new recipe | JSON: `{ "message": "Recipe created" }` |
    | POST   | 400 | Bad request (missing or invalid fields) | JSON: `{ "error": "Invalid request body" }` |
    | POST   | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` |

    **Request Body**

    ```json
    {
        "title": "string",
        "description": "string",
        "difficulty": "string",
        "timeMinutes": "integer",
        "servings": "integer",
        "isExpensive": "boolean",
        "isVegan": "boolean",
        "type": "string",
        "DishPicture": "string URL"
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

1. `/recipes/search`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Successfully searched recipes | Array of recipe objects |
    | GET    | 400 | Invalid or missing search parameters | JSON: `{ "error": "Invalid search parameters" }` |

    **Query Parameters (required)**

    | Parameter     | Type    | Description                         |
    |---------------|---------|-------------------------------------|
    | `difficulty`  | string  | Filter by recipe difficulty         |
    | `type`        | string  | Filter by recipe type               |
    | `vegan`       | boolean | Filter by vegan status              |
    | `isExpensive` | boolean | Filter by price category            |
    | `search`      | string  | Search by title or description text |

    **Example Request**

    `GET /recipes/search?type=dessert&difficulty=easy&vegan=true&isExpensive=true&search=chocolate`

### Reviews

1. `/recipes/{recipeId}/reviews`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | GET    | 200 | Successfully gets all reviews for a recipe | Array of review objects | `recipeId` (recipe ID) |
    | GET    | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    | POST   | 201 | Successfully added review | JSON: `{ "message": "Review added successfully" }` | `recipeId` (recipe ID) |
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
            "Name": "string",
            "stars": "integer",
            "comment": "string",
            "createdAt": "DateTime"
        }
    ]
    ```

2. `/reviews/{reviewId}`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | PUT    | 200 | Successfully updated review  | Updated review object | `reviewId` (review ID) |
    | PUT    | 400 | Invalid request body | JSON: `{ "error": "Invalid request body" }` | `reviewId` (review ID) |
    | PUT    | 403 | Forbidden (not review owner) | - | `reviewId` (review ID) |
    | PUT    | 404 | Review not found | JSON: `{ "error": "Review not found" }` | `reviewId` (review ID) |
    |-|-|-|-|-|
    | DELETE | 204 | Successfully deleted review | - | `reviewId` (review ID) |
    | DELETE | 403 | Forbidden (not review owner) | - | `reviewId` (review ID) |
    | DELETE | 404 | Review not found | JSON: `{ "error": "Review not found" }` | `reviewId` (review ID) |

    **Request Body**

    ```json
    {
        "stars": "integer",
        "comment": "string"
    }
    ```

### Polls

1. `/polls/active`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Returns the current active poll | JSON object |
    | GET    | 404 | No active poll found | JSON: `{ "error": "No active poll" }` |

    **Response Body**

    ```json
    {
        "id": "integer",
        "question": "string",
        "options": [
            { "id": "integer", "OptionText": "string", "voteCount": "integer" },
            { "id": "integer", "OptionText": "string", "voteCount": "integer" },
            { "id": "integer", "OptionText": "string", "voteCount": "integer" }
        ]
    }
    ```

2. `polls/{pollId}/vote`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | POST   | 200 | Successfully voted for an option | JSON: `{ "message": "Vote recorded" }` | `pollId` (poll ID) |
    | POST   | 400 | Invalid option ID | JSON: `{ "error": "Invalid option" }` | `pollId` (poll ID) |
    | POST   | 404 | Poll not found | JSON: `{ "error": "Poll not found" }` | `pollId` (poll ID) |

    **Request Body**

    ```json
    {
      "optionId": "integer"
    }
    ```

### Favorites

1. `/user/favorites`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Gets all favorite recipes for user | Array of favourite objects |

    **Response Body**

    ```json
    [
        {
            "title": "string",
            "difficulty": "string",
            "timeMinutes": "integer",
            "servings": "integer",
            "dishPicture": "string URL"
        }
    ]
    ```

2. `/user/favorites/{recipeId}`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | POST   | 201 | Successfully added recipe to favorites | JSON: `{ "message": "Recipe added to favorites" }` or `{ message = "Recipe removed from favorites" }` | `recipeId` (recipe ID) |
    | POST   | 404 | Recipe not found | JSON: `{ "error": "Recipe not found" }` | `recipeId` (recipe ID) |
    | DELETE | 204 | Successfully removed recipe from favorites | - | `recipeId` (recipe ID) |
    | DELETE | 404 | Recipe not found | JSON: `{ "error": " Recipe not found" }` | `recipeId` (recipe ID) |

## Testing

Create tests for each endpoint using Postman.

### RecipesTests

`GET /recipes`

- 200 - returns list of recipes

---

`POST /recipes/create`

- 201 - valid and authorized
- 400 - invalid or missing data
- 401 - unauthorized

---

`GET /recipes/{id}`

- 200 - valid, returns single recipe
- 404 - recipe not found

`PUT /recipes/{id}`

- 200 - valid and authorized
- 400 - invalid request body
- 401 - unauthorized
- 404 - recipe not found

`DELETE /recipes/{id}`

- 200 - valid and authorized
- 401 - unauthorized
- 404 - recipe not found

### SearchTests

`GET /recipes/search`

- 200 - returns list of recipes based on search params
- 400 - invalid or missing parameters

### ReviewTests

`GET /recipes/{id}/reviews`

- 200 - returns list of reviews
- 404 - recipe not found

`POST /recipes/{id}/reviews`

- 201 - valid and authorized
- 400 - invalid or missing data
- 401 - unauthorized

---

`PUT /reviews/{id}`

- 200 - valid and authorized
- 400 - invalid or missing data
- 403 - forbidden
- 404 - review not found

`DELETE /review/{id}`

- 204 - valid and authorized
- 403 - forbidden
- 404 - review not found

### PollTests

`GET polls/active`

- 200 - returns the active poll
- 404 - poll not found

---

`POST polls/{id}/vote`

- 200 - valid and authorized
- 400 - invalid id
- 404 - poll not found

### FavoriteTests

`GET /user/favorites`

- 200 - returns a list of the user's favourite recipes

---

`POST /user/favorites/{id}`

- 201 - valid and authorized
- 404 - recipe not found

`DELETE /user/favorites/{id}`

- 204 - valid and authorized
- 404 - recipe not found

## Deliverables

- Fully working recipes CRUD with search.
- Review system with ownership validation.
- Poll voting and active poll retrieval.
- User favorites functionality.
- All Postman tests should pass successfully.

## Estimated Duration

~ 2 weeks

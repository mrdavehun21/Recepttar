# API Endpoints Reference

## Authentication & User Management

1. `/user/register`

    | Method | Status Code | Description                                | Response Body                         |
    |--------|-------------|--------------------------------------------|---------------------------------------|
    | POST   | 201         | Created a new user account                 | JSON: `{ "message": "User created" }` |
    | POST   | 400         | Bad request (missing fields, email exists) | JSON: `{ "error": "Bad request" }`    |

    **Request Body**

    ```json
    {
        "name": "string",
        "email": "string",
        "password": "string"
    }
    ```

2. `/user/checkEmail`

    | Method | Status Code | Description                                | Response Body                          |
    |--------|-------------|--------------------------------------------|----------------------------------------|
    | GET    | 200         | Email does exist                           | JSON: `{ "message": "Email exists" }`  |
    | GET    | 400         | Bad request (missing fields, email exists) | JSON: `{ "error": "Bad request" }`     |
    | GET    | 404         | Email not found                            | JSON: `{ "error": "Email not found" }` |

    **Query parameter (required)**

    | Parameter     | Type    |
    |---------------|---------|
    | `email`       | string  |

    **Example request**

    `GET /user/checkemail?email=info@recepttar.hu`

3. `/user/isLoggedIn`

    | Method | Status Code | Description       | Response Body                       |
    |--------|-------------|-------------------|-------------------------------------|
    | POST   | 200         | User is logged in | -                                   |
    | POST   | 401         | Unauthorized      | JSON: `{ "error": "Unauthorized" }` |

4. `/user/login`

    | Method | Status Code | Description                             | Response Body                                    |
    |--------|-------------|-----------------------------------------|--------------------------------------------------|
    | POST   | 200         | Logged in                               | JSON: `{ "message": "Successfully logged in" }`  |
    | POST   | 400         | Bad request (missing or invalid fields) | JSON: `{ "error": "Invalid request body" }`      |  
    | POST   | 401         | Invalid credentials                     | JSON: `{ "error": "Invalid email or password" }` |

    **Request Body**

    ```json
    {
        "email": "string",
        "password": "string",
    }
    ```

5. `/user/logout`

    | Method | Status Code | Description  | Response Body                                    |
    |--------|-------------|--------------|--------------------------------------------------|
    | POST   | 200         | Logged out   | JSON: `{ "message": "Successfully logged out" }` |
    | POST   | 401         | Unauthorized | JSON: `{ "error": "Unauthorized" }`              |

6. `/user/profile`

    | Method | Status Code | Description          | Response Body                                             |
    |--------|-------------|----------------------|-----------------------------------------------------------|
    | GET    | 200         | Got user's profile   | JSON object                                               |
    | GET    | 401         | Unauthorized         | JSON: `{ "error": "Unauthorized" }`                       |
    | -      | -           | -                    | -                                                         |
    | PATCH  | 200         | Updated profile      | JSON object                                               |
    | PATCH  | 200         | No changes were made | JSON: `{ "message": "No changes were made to the user" }` |
    | PATCH  | 401         | Unauthorized         | JSON: `{ "error": "Unauthorized" }`                       |

    **Response Body**

    ```json
    {
        "name": "string",
        "bio": "string",
        "profilepicture": "string URL",
        "rank": "string"
    }
    ```

7. `/user/profile/{userId}`

    | Method | Status Code | Description                | Response Body                        | Path Parameter     |
    |--------|-------------|----------------------------|--------------------------------------|--------------------|
    | GET    | 200         | Got another user's profile | JSON object                          | `userId` (user ID) |
    | GET    | 404         | User not found             | JSON: `{ "error": "User not found" }`| `userId` (user ID) |

    **Response Body**

    ```json
    {
        "name": "string",
        "bio": "string",
        "profilepicture": "string URL",
        "rank": "string"
    }
    ```

8. `/user/profile/profilepicture`

    | Method | Status Code | Description                   | Response Body                         |
    |--------|-------------|-------------------------------|---------------------------------------|
    | GET    | 200         | Returns with a raw image file | Binary file                           |
    | GET    | 401         | Unauthorized                  | JSON: `{ "error": "Unauthorized" }`   |
    | -      | -           | -                             | -                                     |
    | POST   | 200         | Returns with a raw image file | Binary file                           |
    | POST   | 401         | Unauthorized                  | JSON: `{ "error": "Unauthorized" }`   |

9. `/user/profile/profilepicture/{userId}`

    | Method | Status Code | Description                   | Response Body                         | Path Parameter     |
    |--------|-------------|-------------------------------|---------------------------------------|--------------------|
    | GET    | 200         | Returns with a raw image file | Binary file                           | `userId` (user ID) |
    | GET    | 404         | User not found                | JSON: `{ "error": "User not found" }` | `userId` (user ID) |

### User Favorites

1. `/user/favorites`

    | Method | Status Code | Description                       | Response Body                       |
    |--------|-------------|-----------------------------------|-------------------------------------|
    | GET    | 200         | Got all favorite recipes for user | Array of Recipe objects             |
    | GET    | 401         | Unauthorized                      | JSON: `{ "error": "Unauthorized" }` |

    **Response Body**

    ```json
    [
        {
            "recipeId": "integer",
            "title": "string",
            "description": "string",
            "dishPicture": "string URL"
        }
    ]
    ```

2. `/user/favorites/{recipeId}`

    | Method | Status Code | Description             | Response Body                                      | Path Parameter         |
    |--------|-------------|-------------------------|----------------------------------------------------|------------------------|
    | POST   | 200         | Added to favorites      | JSON: `{ "message": "Recipe added to favorites" }` | `recipeId` (recipe ID) |
    | POST   | 404         | Recipe not found        | JSON: `{ "error": "Recipe not found" }`            | `recipeId` (recipe ID) |
    | POST   | 401         | Unauthorized            | JSON: `{ "error": "Unauthorized" }`                | `recipeId` (recipe ID) |
    | POST   | 409         | Already in favorites    | JSON: `{ "error": "Recipe already in favorites" }` | `recipeId` (recipe ID) |
    | -      | -           | -                       | -                                                  | -                      |
    | DELETE | 204         | Removed from favorites  | -                                                  | `recipeId` (recipe ID) |
    | DELETE | 404         | Recipe not in favorites | JSON: `{ "error": "Recipe not in favorites" }`     | `recipeId` (recipe ID) |
    | DELETE | 401         | Unauthorized            | JSON: `{ "error": "Unauthorized" }`                | `recipeId` (recipe ID) |

### Recipe Management

1. `/recipe/all`

    | Method | Status Code | Description     | Response Body           |
    |--------|-------------|-----------------|-------------------------|
    | GET    | 200         | Got all recipes | Array of Recipe objects |

    **Response Body**

    ```json
    [
        {
            "recipeId": "integer",
            "title": "string",
            "description": "string",
            "difficulty": "string",
            "timeMinutes": "integer",
            "servings": "integer",
            "isExpensive": "bool",
            "isVegan": "bool",
            "type": "string",
            "dishPicture": "string URL",
            "authorId": "integer",
            "ingredients": [
                { "id": "integer", "ingredientName": "string", "Quantity": "float", "MeasurementUnit": "string" }
            ],
            "steps": [
                { "recipeStepNumber": "integer", "RecipeStepDescription": "string" }
            ]
        }
    ]
    ```

2. `/recipe/create`

    | Method | Status Code | Description               | Response Body                               |
    |--------|-------------|---------------------------|---------------------------------------------|
    | POST   | 201         | Created a new recipe      | JSON: `{ "message": "Recipe created" }`     |
    | POST   | 400         | Missing or invalid fields | JSON: `{ "error": "Invalid request body" }` |
    | POST   | 401         | Unauthorized              | JSON: `{ "error": "Unauthorized" }`         |

    **Request Body**

    ```json
    {
        "recipeId": "integer",
        "title": "string",
        "description": "string",
        "difficulty": "string",
        "timeMinutes": "integer",
        "servings": "integer",
        "isExpensive": "bool",
        "isVegan": "bool",
        "type": "string",
        "dishPicture": "string URL",
        "authorId": "integer",
        "ingredients": [
            { "id": "integer", "ingredientName": "string", "Quantity": "float", "MeasurementUnit": "string" }
        ],
        "steps": [
            { "recipeStepNumber": "integer", "RecipeStepDescription": "string" }
        ]
    }
    ```

3. `recipe/{recipeId}/image`

    | Method | Status Code | Description           | Response Body                                 | Path Parameter          |
    |--------|-------------|-----------------------|-----------------------------------------------|------------------------ |
    | GET    | 200         | Got recipe            | Recipe object                                 | `recipeId` (recipe ID)  |
    | GET    | 404         | Recipe not found      | JSON: `{ "error": "Recipe not found" }`       | `recipeId` (recipe ID)  |
    | GET    | 404         | DishPicture not found | JSON: `{ "error": "Dish picture not found" }` | `recipeId` (recipe ID)  |

4. `/recipe/{recipeId}`

    | Method | Status Code | Description               | Response Body                                                    | Path Parameter         |
    |--------|-------------|---------------------------|------------------------------------------------------------------|------------------------|
    | GET    | 200         | Got recipe                | Recipe object                                                    | `recipeId` (recipe ID) |
    | GET    | 404         | Recipe not found          | JSON: `{ "error": "Recipe not found" }`                          | `recipeId` (recipe ID) |
    | -      | -           | -                         | -                                                                | -                      |
    | PATCH  | 200         | Updated recipe            | JSON: `{ "message": "Recipe updated" }`                          | `recipeId` (recipe ID) |
    | PATCH  | 200         | No changes were made      | JSON: `{ "message": "No changes were made to the recipe" }`      | `recipeId` (recipe ID) |
    | PATCH  | 400         | Missing or invalid fields | JSON: `{ "error": "Invalid request body" }`                      | `recipeId` (recipe ID) |
    | PATCH  | 401         | Unauthorized              | JSON: `{ "error": "Unauthorized" }`                              | `recipeId` (recipe ID) |
    | PATCH  | 404         | Recipe not found          | JSON: `{ "error": "Recipe not found" }`                          | `recipeId` (recipe ID) |
    | PATCH  | 403         | Not recipe owner          | JSON: `{ "error": "You are not allowed to edit this recipe" }`   | `recipeId` (recipe ID) |
    | -      | -           | -                         | -                                                                | -                      |
    | DELETE | 204         | Deleted recipe            | -                                                                | `recipeId` (recipe ID) |
    | DELETE | 401         | Unauthorized              | JSON: `{ "error": "Unauthorized" }`                              | `recipeId` (recipe ID) |
    | DELETE | 404         | Recipe not found          | JSON: `{ "error": "Recipe not found" }`                          | `recipeId` (recipe ID) |
    | DELETE | 403         | Not recipe owner          | JSON: `{ "error": "You are not allowed to delete this recipe" }` | `recipeId` (recipe ID) |

### Recipe Search

1. `/recipe/search`

    | Method | Status Code | Description               | Response Body                                    |
    |--------|-------------|---------------------------|--------------------------------------------------|
    | GET    | 200         | Searched recipes          | Array of Recipe objects                          |
    | GET    | 400         | Invalid search parameters | JSON: `{ "error": "Invalid search parameters" }` |

    **Query Parameters**

    | Parameter     | Type    | Description                         |
    |---------------|---------|-------------------------------------|
    | `difficulty`  | string  | Filter by recipe difficulty         |
    | `type`        | string  | Filter by recipe type               |
    | `isVegan`     | boolean | Filter by vegan status              |
    | `isExpensive` | boolean | Filter by price category            |
    | `search`      | string  | Search by title or description text |
    | `ingredients` | integer | Filter by ingredients               |

    **Example Request**

    `GET /recipe/search?type=dessert&difficulty=easy&isVegan=true&isExpensive=true&search=chocolate&ingredients=1,2`

### Recipe Reviews & Ratings

1. `/recipe/{recipeId}/reviews`

    | Method | Status Code | Description                     | Response Body                                      | Path Parameter         |
    |--------|-------------|---------------------------------|----------------------------------------------------|------------------------|
    | GET    | 200         | Got all reviews                 | Array of Review objects                            | `recipeId` (recipe ID) |
    | GET    | 404         | Recipe not found                | JSON: `{ "error": "Recipe not found" }`            | `recipeId` (recipe ID) |
    | -      | -           | -                               | -                                                  | -                      |
    | POST   | 201         | Added review                    | JSON: `{ "message": "Review added successfully" }` | `recipeId` (recipe ID) |
    | POST   | 400         | Invalid stars or missing fields | JSON: `{ "error": "Invalid request body" }`        | `recipeId` (recipe ID) |
    | POST   | 404         | Recipe not found                | JSON: `{ "error": "Recipe not found" }`            | `recipeId` (recipe ID) |
    | POST   | 401         | Unauthorized                    | JSON: `{ "error": "Unauthorized" }`                | `recipeId` (recipe ID) |

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
            "recipeId": "integer",
            "userId": "integer",
            "stars": "integer",
            "comment": "string",
            "createdAt": "DateTime",
            "updatedAt": "DateTime"
        }
    ]
    ```

2. `/reviews/{reviewId}`

    | Method | Status Code | Description                     | Response Body                                                    | Path Parameter         |
    |--------|-------------|---------------------------------|------------------------------------------------------------------|------------------------|
    | PATCH  | 200         | Updated review                  | Updated Review object                                            | `reviewId` (review ID) |
    | PATCH  | 400         | Invalid stars or missing fields | JSON: `{ "error": "Stars must be between 1 and 5" }`             | `reviewId` (review ID) |
    | PATCH  | 403         | Not review owner                | JSON: `{ "error": "You are not allowed to edit this review" }`   | `reviewId` (review ID) |
    | PATCH  | 404         | Review not found                | JSON: `{ "error": "Review not found" }`                          | `reviewId` (review ID) |
    | PATCH  | 401         | Unauthorized                    | JSON: `{ "error": "Unauthorized" }`                              | `reviewId` (review ID) |
    | -      | -           | -                               | -                                                                | -                      |
    | DELETE | 204         | Deleted review                  | -                                                                | `reviewId` (review ID) |
    | DELETE | 403         | Not review owner                | JSON: `{ "error": "You are not allowed to delete this review" }` | `reviewId` (review ID) |
    | DELETE | 404         | Review not found                | JSON: `{ "error": "Review not found" }`                          | `reviewId` (review ID) |
    | DELETE | 401         | Unauthorized                    | JSON: `{ "error": "Unauthorized" }`                              | `reviewId` (review ID) |

    **Request Body**

    ```json
    {
        "stars": "integer",
        "comment": "string"
    }
    ```

### Community Polls & Voting

1. `/polls/active`

    | Method | Status Code | Description          | Response Body                         |
    |--------|-------------|----------------------|---------------------------------------|
    | GET    | 200         | Got active polls     | Array of Poll objects                 |
    | GET    | 404         | No active poll found | JSON: `{ "error": "No active poll" }` |

    **Response Body**

    ```json
    [
        {
            "id": "integer",
            "authorId": "integer",
            "question": "string",
            "options": [
                { "id": "integer", "OptionText": "string", "voteCount": "integer" }
            ],
            "votedOn": "integer"
        }
    ]
    ```

2. `polls/create`

    | Method | Status Code | Description                               | Response Body                                          |
    |--------|-------------|-------------------------------------------|--------------------------------------------------------|
    | POST   | 200         | Created an option                         | JSON: `{ "message": "Poll posted successfuly" }`       |
    | POST   | 400         | Rank level low                            | JSON: `{ "error": "Rank level too low" }`              |
    | POST   | 400         | Invalid size of options or missing fields | JSON: `{ "error": "Missing or incomplete field(s)" }`  |
    | POST   | 401         | Unauthorized                              | JSON: `{ "error": "Unauthorized" }`                    |

    **Request Body**

    ```json
    {
      "question": "string",
      "options[0].optionText": "string"
    }
    ```

3. `polls/{pollId}`

    | Method | Status Code | Description     | Response Body                                                  | Path Parameter     |
    |--------|-------------|-----------------|----------------------------------------------------------------|--------------------|
    | DELETE | 204         | Deleted a poll  | -                                                              | `pollId` (poll ID) |
    | DELETE | 403         | Not poll owner  | JSON: `{ "error": "You are not allowed to delete this poll" }` | `pollId` (poll ID) |
    | DELETE | 404         | Poll not found  | JSON: `{ "error": "Poll not found" }`                          | `pollId` (poll ID) |
    | DELETE | 401         | Unauthorized    | JSON: `{ "error": "Unauthorized" }`                            | `pollId` (poll ID) |

4. `polls/{pollId}/vote`

    | Method | Status Code | Description                 | Response Body                              | Path Parameter     |
    |--------|-------------|-----------------------------|--------------------------------------------|--------------------|
    | POST   | 200         | Voted for an option         | JSON: `{ "message": "Vote recorded" }`     | `pollId` (poll ID) |
    | POST   | 400         | Invalid option ID           | JSON: `{ "error": "Invalid option" }`      | `pollId` (poll ID) |
    | POST   | 409         | Already voted for an option | JSON: `{ "error": "User already voted" }`  | `pollId` (poll ID) |
    | POST   | 404         | Poll not found              | JSON: `{ "error": "Poll not found" }`      | `pollId` (poll ID) |
    | POST   | 401         | Unauthorized                | JSON: `{ "error": "Unauthorized" }`        | `pollId` (poll ID) |

    **Request Body**

    ```json
    {
        "UserId": "integer",
        "PollId": "integer",
        "optionId": "integer"
    }
    ```

### Ingredients & Measurement Units

1. `ingredients/search`

    | Method | Status Code | Description               | Response Body                |
    |--------|-------------|---------------------------|------------------------------|
    | GET    | 200         | Searched ingredients      | Array of Ingredients objects |

    **Query Parameters**

    | Parameter     | Type    | Description            |
    |---------------|---------|------------------------|
    | `search`      | string  | Search for ingredients |

    **Example Request**

    `GET /ingredients/search?search=chocolate`

2. `ingredients/units`

    | Method | Status Code | Description   | Response Body                    |
    |--------|-------------|---------------|----------------------------------|
    | GET    | 200         | Got all units | Array of Measurementunit objects |

    **Response Body**

    ```json
    [
        "string"
    ]
    ```



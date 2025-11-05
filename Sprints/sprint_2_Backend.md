# Sprint 2 - Profile management and User related endpoint creation

## Goal

Implement **session-based user authentication** and **basic profile management**.  
Users should be able to:

- Register.
- Log in/out.
- View other user's profile and update their own.

## Tasks

### Users

1. `/user/register`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | POST   | 201 | Successfully created a new user account | JSON: `{ "message": "User created" }` |
    | POST   | 400 | Bad request (missing fields, email exists) | JSON: `{ "error": "Bad request" }` |

    **Request Body**

    ```json
    {
        "name": "string",
        "email": "string",
        "password": "string"
    }
    ```

2. `/user/login`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | POST   | 200 | Successfully logged in | JSON: `{ "message": "Successfully logged in" }` |
    | POST   | 400 | Bad request (missing or invalid fields) | JSON: `{ "error": "Invalid request body" }` |  
    | POST   | 401 | Invalid credentials | JSON: `{ "error": "Invalid email or password" }` |

    **Request Body**

    ```json
    {
        "email": "string",
        "password": "string",
    }
    ```

3. `/user/logout`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | POST   | 200 | Successfully logged out | JSON: `{ "message": "Successfully logged out" }` |
    | POST   | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` |

4. `/user/profile`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|---------------|
    | GET    | 200 | Successfully gets user's profile | JSON object |
    | GET    | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` |
    |-|-|-|-|
    | PUT    | 200 | Successfully updated profile | JSON object |
    | PUT    | 401 | Unauthorized | JSON: `{ "error": "Unauthorized" }` |
    | PUT    | 404 | User not found | JSON: `{ "error": "User not found" }` |

    **Response Body**

    ```json
    {
        "name": "string",
        "bio": "string",
        "profilepicture": "string URL"
    }
    ```

5. `/user/profile/{userId}`

    | Method | Status Code | Description | Response Body | Path Parameter |
    |--------|-------------|-------------|---------------|----------------|
    | GET | 200 | Gets another user's profile | JSON object | `userId` (user ID) |
    | GET | 404 | User not found | JSON: `{ "error": "User not found" }`| `userId` (user ID) |

    **Response Body**

    ```json
    {
        "name": "string",
        "bio": "string",
        "profilepicture": "string URL"
    }
    ```

## Testing

Create tests for each endpoint using Postman.

### UserTests

`POST /user/register`

- 201 - valid and authorized
- 400 - missing fields or duplicate email

---

`POST /user/login`

- 200 - valid and authorized
- 400 - missing fields
- 401 - invalid credentials

---

`POST /user/logout`

- 200 - valid and authorized
- 401 - unauthorized

---

`GET /user/profile`

- 200 - valid and authorized
- 401 - unauthorized

`PUT /user/profile`

- 401 - unauthorized
- 404 - user not found

---

`GET /user/profile/{id}`

- 200 - returns other user’s profile
- 404 - user not found

## Deliverables

- Working session-based authentication.
- All Postman tests should pass successfully.

## Estimated Duration

~ A week

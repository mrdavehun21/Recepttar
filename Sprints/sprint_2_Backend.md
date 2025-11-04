# Sprint 2-Profile management and User related endpoint creation

## Goal

Implement **session-based user authentication** and **basic profile management**.  
Users should be able to:

- Register.
- Log in/out.
- View other user's profile and update their own.

## Tasks

### Endpoints to implement in `Models/User.cs`

1. `/user/register`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|------------|
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
    |--------|-------------|-------------|------------|
    | POST   | 200 | Successfully logged in | JSON: `{ "message": "Successfully logged in", "token": "jwt_token_string" }` |
    | POST   | 400| Missing required fields| JSON: `{ "error": "Email and password are required" }` |  
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
    |--------|-------------|-------------|------------|
    | POST   | 200 | Successfully logged out | JSON: `{ "message": "Successfully logged out" }` |

4. `/user/profile`

    | Method | Status Code | Description | Response Body |
    |--------|-------------|-------------|------------|
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
        "profilepicture": "string"
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
        "profilepicture": "string"
    }
    ```

## Testing

Create unit tests for each endpoint using NUnit.
Tests to Include:

1. `/user/register`
    - Should successfully create a new user (201)
    - Should fail with missing fields or duplicate email (400)

2. `/user/login`
    - Should log in with valid credentials (200)
    - Should fail with missing fields (400) or invalid credentials (401)

3. `/user/logout`
    - Should log out successfully with a valid session (200)
    - Should fail without a valid session (401)

4. `/user/profile`
    - Should get and update own profile with a valid session (200)
    - Should fail if unauthorized (401) or user not found (404)

5. `/user/profile/{userId}`
    - Should get another user’s profile (200)
    - Should fail if the user does not exist (404)

## Deliverables

- Working session-based authentication.
- All endpoints are covered by NUnit tests.
- All NUnit tests should pass.

## Estimated Duration

~ A week

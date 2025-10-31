# Sprint 1 - Project Setup

## Goal

Purpose of sprint 1 is to set up the foundation for **Recepttár** backend.
This sprint focuses on setting up the project envirement, configuring the connection to a MYSQL database, and creating the initial database models (Users and Recipes).

## Objective

- Create the base ASP.NET Core Web API project.  
- Configure a working connection to MySQL.  
- Design and generate the initial database schema.  
- Verify that data can be created, stored, and retrieved.

## Tasks

### 1. **Project Initialization**

- Create a new ASP.NET Core Web API project in Visual Studio 2022.
- Set up the folder structure (Controllers, Models, Services, Data).
- Configure appsettings.json with the MySQL connection string.

### 2. **Database Design**

Create models and fields

- `Models\User.cs`

    | Field            | Type              | Description                  |
    |------------------|-------------------|------------------------------|
    | `Id`             | integer (PK)      | Unique identifier            |
    | `Name`           | string            | Display name of the user     |
    | `Email`          | string            | Used for login (unique)      |
    | `PasswordHash`   | string            | Hashed password              |
    | `Bio`            | string            | Short user description       |
    | `ProfilePicture` | string (URL)      | A chooseable img             |
    | `Role`           | string            | “user” or “admin”            |

- `Models\Recipe.cs`

    | Field           | Type     | Description                         |
    |-----------------|----------|-------------------------------------|
    | `Id`            | integer (PK)| Unique identifier                |
    | `Title`         | string   | Recipe name                         |
    | `Description`   | string   | Overview and short instructions     |
    | `Difficulty`    | string   | “easy”, “medium”, “hard”            |
    | `TimeMinutes`   | integer  | Preparation time in minutes         |
    | `Servings`      | integer  | Number of servings                  |
    | `PriceCategory` | string   | “cheap” or “expensive”              |
    | `Vegan`         | bool     | True = vegan-friendly               |
    | `Type`          | string   | “appetizer”, “main dish”, “dessert” |
    | `AuthorId`      | integer (FK) | References `User.Id`            |

- `Models/Poll.cs`

    | Field      | Type         | Description       |
    |------------|--------------|-------------------|
    | `Id`       | integer (PK) | Unique identifier |
    | `Question` | string       | Poll question     |
    | `IsActive` | bool         | true if active    |

- `Models/PollOption.cs`

    | Field       | Type         | Description                     |
    |-------------|--------------|---------------------------------|
    | `Id`        | integer (PK) | Unique identifier               |
    | `PollId`    | integer (FK) | References the poll             |
    | `Text`      | string       | Option text                     |
    | `VoteCount` | integer      | Number of votes for this option |

- `Models/Review.cs`

    | Field       | Type                         | Description                                |
    |-------------|------------------------------|--------------------------------------------|
    | `Id`        | integer (PK, auto-increment) | Unique identifier for each review          |
    | `RecipeId`  | integer (FK -> `Recipes.Id`)   | Reference to the recipe being reviewed     |
    | `UserId`    | integer (FK -> `Users.Id`)     | Reference to the user who wrote the review |
    | `Name`      | string (FK -> `Users.Name`)    | Optional display name of the user          |
    | `Stars`     | integer                      | Rating from 1 to 5                         |
    | `Comment`   | string                       | The review text                            |
    | `CreatedAt` | DateTime                     | When the review was created                |
    | `UpdatedAt` | DateTime Null                | When the review was last updated           |

- Define the one-to-many relationships between the classes.

### 3. **Version Control**

- Initialize Github repository.
- Commit the base project structure.
- Add `.gitignore` file:

    ```text
        bin/
        obj/
        .vs/
        *.user
        *.db
    ```

## Testing

- Run the project locally and confirm the test endpoint returns a response.
- Verify the database schema exists in MYSQL.
- Confirm relationships work correctly between User, Recipe and Poll.

## Deliverables

- Working ASP .NET Core Web API project connected to MySQL.
- Initial database created with **User**, **Recipe** and **Poll** tables.

## Estimated Duration

~ Half a week

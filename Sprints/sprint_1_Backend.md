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

    | Field            | C# Type      | Database Type (MySQL) |  Description               |
    |------------------|--------------|-----------------------|----------------------------|
    | `Id`             | int (PK)     | INT AUTO_INCREMENT PK | Unique identifier          |
    | `Name`           | string       | VARCHAR(255)          | Display name of the user   |
    | `Email`          | string       | VARCHAR(255) UNIQUE   | Used for login (unique)    |
    | `PasswordHash`   | string       | VARCHAR(128)          | Hashed password            |
    | `Bio`            | string       | TEXT                  | Short user description     |
    | `ProfilePicture` | byte[]       | MEDIUMBLOB            | A chooseable profile img   |
    | `Role`           | bool         | BOOLEAN               | true = admin, false = user |

- `Models\Recipe.cs`

    | Field           | Type                  | Database Type (MySQL) | Description                         |
    |-----------------|-----------------------|-----------------------|-------------------------------------|
    | `Id`            | int (PK)              | INT AUTO_INCREMENT PK | Unique identifier                   |
    | `Title`         | string                | VARCHAR(255)          | Recipe name                         |
    | `Description`   | string                | TEXT                  | Overview and short instructions     |
    | `Difficulty`    | enum                  | ENUM                  | “Easy”, “Medium”, “Hard”            |
    | `TimeMinutes`   | int                   | INT                   | Preparation time in minutes         |
    | `Servings`      | int                   | INT                   | Number of servings                  |
    | `IsExpensive`   | bool                  | BOOLEAN               | true = expensive, false = cheap     |
    | `IsVegan`       | bool                  | BOOLEAN               | true = vegan, false = not vegan     |
    | `Type`          | enum                  | ENUM                  | “Appetizer”, “MainDish”, “Dessert”  |
    | `DishPicture`   | byte[]                | MEDUIMBLOB            | A chooseable dish img               |
    | `AuthorId`      | int (FK -> `User.Id`) | INT                   | References `User.Id`                |

- `Models/Poll.cs`

    | Field      | Type         | Database Type (MySQL) | Description                       |
    |------------|--------------|-----------------------|-----------------------------------|
    | `Id`       | integer (PK) | INT AUTO_INCREMENT PK | Unique identifier                 |
    | `Question` | string       | VARCHAR(255)          | Poll question                     |
    | `IsActive` | bool         | BOOLEAN               | true = active, false = not active |

- `Models/PollOption.cs`

    | Field        | Type                       | Database Type (MySQL) | Description                     |
    |--------------|----------------------------|-----------------------|---------------------------------|
    | `Id`         | integer (PK)               | INT AUTO_INCREMENT PK | Unique identifier               |
    | `PollId`     | integer (FK -> `Poll.Id`)  | -                     | References the poll             |
    | `OptionText` | string                     | VARCHAR(255)          | Option text                     |
    | `VoteCount`  | integer                    | INT                   | Number of votes for this option |

- `Models/Review.cs`

    | Field       | Type                         | Database Type (MySQL) | Description                                |
    |-------------|------------------------------|-----------------------|--------------------------------------------|
    | `Id`        | integer (PK)                 | INT AUTO_INCREMENT PK | Unique identifier for each review          |
    | `RecipeId`  | integer (FK -> `Recipes.Id`) | -                     | Reference to the recipe being reviewed     |
    | `UserId`    | integer (FK -> `Users.Id`)   | -                     | Reference to the user who wrote the review |
    | `Stars`     | integer                      | INT                   | Rating from 1 to 5                         |
    | `Comment`   | string                       | VARCHAR(1024)         | The review text                            |
    | `CreatedAt` | DateTime                     | TIMESTAMP Current     | When the review was created                |
    | `UpdatedAt` | DateTime Null                | TIMESTAMP Null        | When the review was last updated           |

- Define the one-to-many relationships between the classes.

### 3. **Version Control**

- Initialize Github repository.
- Commit the base project structure.
- Add `.gitignore` file:

    ```gitignore
        bin/
        obj/
        .vs/
        *.user
        *.db
    ```

## Testing

- Run the project locally and confirm the test endpoint returns a response.
- Verify the database schema exists in MYSQL.
- Confirm relationships work correctly between User, Recipe, Poll, PollOption and Review tables.

## Deliverables

- Working ASP .NET Core Web API project connected to MySQL.
- Initial database created with **User**, **Recipe**, **Poll**, **PollOption** and **Review** tables.

## Estimated Duration

~ Half a week

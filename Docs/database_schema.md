# Recepttár Database Schema

## Tables & Models

### User

| Field            | Type    | Database Type (MySQL) |  Description                                                         |
|------------------|---------|-----------------------|----------------------------------------------------------------------|
| `Id`             | integer | INT AUTO_INCREMENT PK | Unique identifier                                                    |
| `Name`           | string  | VARCHAR(255)          | Display name of the user                                             |
| `Email`          | string  | VARCHAR(255) UNIQUE   | Used for login (unique)                                              |
| `PasswordHash`   | string  | VARCHAR(128)          | Hashed password                                                      |
| `Bio`            | string  | TEXT                  | Short user description                                               |
| `ProfilePicture` | byte[]  | MEDIUMBLOB            | A chooseable profile img                                             |
| `Rank`           | enum    | ENUM                  | User level based on activity: 'HomeCook', 'ChefMaster', 'FoodLegend' |

### Recipe

| Field           | Type    | Database Type (MySQL) | Description                        |
|-----------------|---------|-----------------------|------------------------------------|
| `Id`            | integer | INT AUTO_INCREMENT PK | Unique identifier                  |
| `Title`         | string  | VARCHAR(255)          | Recipe name                        |
| `Description`   | string  | TEXT                  | Overview and short instructions    |
| `Difficulty`    | enum    | ENUM                  | “Easy”, “Medium”, “Hard”           |
| `TimeMinutes`   | integer | INT                   | Preparation time in minutes        |
| `Servings`      | integer | INT                   | Number of servings                 |
| `IsExpensive`   | bool    | BOOLEAN               | true = expensive, false = cheap    |
| `IsVegan`       | bool    | BOOLEAN               | true = vegan, false = not vegan    |
| `Type`          | enum    | ENUM                  | “Appetizer”, “MainDish”, “Dessert” |
| `DishPicture`   | byte[]  | MEDUIMBLOB            | A chooseable dish img              |
| `AuthorId`      | integer | INT (FK -> `User.Id`) | References `User.Id`               |

### RecipeStep

| Field             | Type    | Database Type (MySQL)   | Description             |
|-------------------|---------|-------------------------|-------------------------|
| `Id`              | integer | INT AUTO_INCREMENT PK   | Unique identifier       |
| `RecipeId`        | integer | INT (FK -> `Recipe.Id`) | References `Recipe.Id`  |
| `StepNumber`      | integer | INT                     | Order of the step       |
| `StepDescription` | string  | TEXT                    | Description of the step |

### Poll

| Field      | Type    | Database Type (MySQL) | Description          |
|------------|---------|-----------------------|----------------------|
| `Id`       | integer | INT AUTO_INCREMENT PK | Unique identifier    |
| `Question` | string  | VARCHAR(255)          | Poll question        |
| `AuthorId` | integer | INT (FK -> `User.Id`) | References `User.Id` |

### PollOption

| Field        | Type    | Database Type (MySQL) | Description          |
|--------------|---------|-----------------------|----------------------|
| `Id`         | integer | INT AUTO_INCREMENT PK | Unique identifier    |
| `PollId`     | integer | INT (FK -> `Poll.Id`) | References `Poll.Id` |
| `OptionText` | string  | VARCHAR(255)          | Option text          |

### Vote

| Field       | Type    | Database Type (MySQL)       | Description                |
|-------------|---------|-----------------------------|----------------------------|
| `Id`        | integer | INT AUTO_INCREMENT PK       | Unique identifier          |
| `UserId`    | integer | INT (FK -> `User.Id`)       | References `User.Id`       |
| `PollId`    | integer | INT (FK -> `Poll.Id`)       | References `Poll.Id`       |
| `OptionId`  | integer | INT (FK -> `PollOption.Id`) | References `PollOption.Id` |

### Review

| Field       | Type          | Database Type (MySQL)   | Description            |
|-------------|---------------|-------------------------|------------------------|
| `Id`        | integer       | INT AUTO_INCREMENT PK   | Unique identifier      |
| `RecipeId`  | integer       | INT (FK -> `Recipe.Id`) | References `Recipe.Id` |
| `UserId`    | integer       | INT (FK -> `User.Id`)   | References `User.Id`   |
| `Stars`     | integer       | INT                     | Rating from 1 to 5     |
| `Comment`   | string        | VARCHAR(1024)           | Review text            |
| `CreatedAt` | DateTime      | TIMESTAMP Current       | Review creation        |
| `UpdatedAt` | DateTime Null | TIMESTAMP Null          | Review updation        |

### Favorite

| Field       | Type    | Database Type (MySQL)   | Description            |
|-------------|---------|-------------------------|------------------------|
| `Id`        | integer | INT AUTO_INCREMENT PK   | Unique identifier      |
| `UserId`    | integer | INT (FK -> `User.Id`)   | References `User.Id`   |
| `RecipeId`  | integer | INT (FK -> `Recipe.Id`) | References `Recipe.Id` |

### Ingredient

| Field  | Type    | Database Type (MySQL) | Description       |
|--------|---------|-----------------------|-------------------|
| `Id`   | integer | INT AUTO_INCREMENT PK | Unique identifier |
| `Name` | string  | VARCHAR(255)          | Ingredient name   |

### Recipeingredient

| Field             | Type    | Database Type (MySQL)       | Description                 |
|-------------------|---------|-----------------------------|---------------------------- |
| `Id`              | integer | INT AUTO_INCREMENT PK       | Unique identifier           |
| `RecipeId`        | integer | INT (FK -> `Recipe.Id`)     | References `Recipe.Id`      |
| `IngredientId`    | integer | INT (FK -> `Ingredient.Id`) | References `Ingredient.Id`  |
| `Quantity`        | float   | FLOAT                       | Ingredient quantity         |
| `MeasurementUnit` | enum    | ENUM                        | Ingredient measurement unit |

import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

const savedLng = localStorage.getItem('i18nextLng') || 'en';

i18n.use(initReactI18next).init({
    resources: {
        en: {
            translation: {
                navbar: { 
                    leaderboard: "Leaderboard",
                    polls: "Polls",
                    home: "Home",
                    profile: "Profile",

                    myFavorites: "My Favorites",
                    myPolls: "My Polls",
                    myRecipes: "My Recipes",
                    createRecipe: "Create Recipe",
                    myProfile: "Profile",
                    logOut: "Log out",
                    logIn: "Log in",
                },
                userRank: {
                    rankLevel: "Rank Level",
                    HomeCook: "Home Cook",
                    ChefMaster: "Chef Master",
                    FoodLegend: "Food Legend",
                },
                measurementUnits: {
                    unit: "Unit",
                    piece: "Piece",
                    gram: "Gram",
                    decagram: "Decagram",
                    kilogram: "Kilogram",
                    milliliter: "Milliliter",
                    deciliter: "Deciliter",
                    liter: "Liter",
                    pinch: "Pinch",
                    clove: "Clove",
                    tablespoon: "Tablespoon",
                    teaspoon: "Teaspoon",
                    coffeespoon: "Coffeespoon",
                    glass: "Glass",
                    cup: "Cup",
                    handful: "Handful",
                    packet: "Packet",
                },
                recipeDifficulty: {
                    difficulty: "Difficulty",
                    easy: "Easy",
                    medium: "Medium",
                    hard: "Hard",
                },

                userPage: {
                    fullName: "Full name",
                    password: "Password",
                    email: "Email",
                    bio: "Bio",
                    language: "Language",
                    uploadImage: "Upload image",
                    save: "Save",
                    recipesBy: "'s recipes",
                },

                homePage: {
                    search: "Search",
                    tags: "Tags",
                    tagsList: {
                        expensive: "Expensive",
                        cheap: "Cheap",
                        vegan: "Vegan",

                        easy: "Easy",
                        medium: "Medium",
                        hard: "Hard",

                        appetizer: "Appetizer",
                        maindish: "Main dish",
                        dessert: "Dessert",
                    },
                    ingredients: "Ingredients",
                    recipesHeader: "Recipes",
                    showMore: "Show More",

                    reviewSingular: "review",
                    reviewPlural: "reviews",

                    polls: "Polls",
                    viewAll: "View all",

                    signInToVote: "Sign in to vote!",
                },

                //Login page translations
                loginPage: {
                    signIn: "Sign In",
                    logIn: "Log in",
                    email: "Email Address",
                    emailPattern: "Please use the following pattern: info@recepttar.hu",
                    continue: "Continue",
                    or: "or",
                    createAccount: "Create an Account",
                    discoverRecipes: "Discover recipes",

                    password: "Password",
                    passwordRequirements: "Password requirements: minimum 8 characters and must include a number",
                },

                registerPage: {
                    register: "Register",
                    fullName: "Full name",
                    fullNameRequirements: "Please enter at least 3 characters",
                },

                leaderboardPage: {
                    leaderboardHeader: "Leaderboard",
                    sortBy: {
                        FavoriteCount: "Favorite count",
                        AvgRating: "Average rating",
                        RecipeCount: "Recipe count",
                    },
                    recipes: "recipe",
                },

                myCollectionPage: {
                    myFavorites: "My favorites",
                    myPolls: "My polls",
                    myRecipes: "My recipes",
                },

                recipeViewPage: {
                    totalTime: "Total Time",
                    timeUnit: {
                        min: "min",
                        hour: "hour",
                    },
                    servings: "Servings",
                    author: "Author",
                    description: "Description",
                    instructions: "Instructions",
                    ingredientsListHeader: "Ingredients list",
                    portions: "Portions",
                    foodType: "Type",

                    reviews: "Reviews",
                    editReviewHeader: "Edit Your Review",
                    cancel: "Cancel",
                    deleteButton: "Delete",
                    saveChanges: "Save",
                    submitReview: "Submit Review",
                    WriteAReview: "Write a Review",
                    shareThoughts: "Share your thoughts about this recipe...",

                    deleteRecipeHeader: "Delete Recipe",
                    deleteRecipeMessage: "Are you sure you want to delete this recipe?",
                },

                createEditRecipePage: {
                    createNewRecipeHeader: "Create a new recipe",
                    editRecipeHeader: "Edit recipe",
                    title: "Title",
                    noImageSelected: "No image selected",
                    chooseFile: "Choose file",
                    noFileChosen: "No file chosen",
                    descriptionPlaceholder: "A short description about this recipe...",
                    createRecipe: "Create recipe",
                    updateRecipe: "Update recipe",

                    recipeDifficulty: "Recipe difficulty",
                    prepTime: "Preparation time (min)",
                    servings: "Servings",
                    price: "Price",

                    selectedIngredients: "Selected ingredients",

                    dishType: "Dish type",
                    type: "Type",
                    recipeStepTooltip: "Click and drag the left side of the steps to reorder them",
                    addStep: "Add step",
                },

                pollCard: {
                    editPoll: "Edit",
                    deletePoll: "Delete",
                    submitVote: "Submit",
                    alreadyVoted: "Already voted",

                    DeletePollConfirmHeader: "Delete Poll",
                    DeletePollConfirmMessage: "Are you sure you want to delete this poll?",
                },

                createPollCard: {
                    pollPageHeader: "Create your own!",
                    pollHeaderShort: "Create new!",
                },

                createCardForm: {
                    question: "Question",
                    questionPlaceholder: "Enter your question here...",
                    options: "Options",
                    optionsPlaceholder: "Option",
                    addOptions: "Add option",
                    submit: "Submit",
                    cancel: "Cancel",
                    update: "Update",
                }
            }
        },
        hu: {
            translation: {
                //Basic translations
                navbar: { 
                    leaderboard: "Ranglista",
                    polls: "Szavazás",
                    home: "Főoldal",
                    profile: "Profil",

                    myFavorites: "Kedvenceim",
                    myPolls: "Szavazásaim",
                    myRecipes: "Recepteim",
                    createRecipe: "Recept létrehozása",
                    myProfile: "Profilom",
                    logOut: "Kijelentkezés",
                    logIn: "Bejelentkezés",
                },
                userRank: {
                    rankLevel: "Rang",
                    HomeCook: "Kezdő szakács",
                    ChefMaster: "Haladó szakács",
                    FoodLegend: "Főszakács",
                },
                measurementUnits: {
                    unit: "Egység",
                    piece: "db",
                    gram: "g",
                    decagram: "dkg",
                    kilogram: "kg",
                    milliliter: "ml",
                    deciliter: "dl",
                    liter: "l",
                    pinch: "Csipet",
                    clove: "Gerezd",
                    tablespoon: "Evőkanál",
                    teaspoon: "Teáskanál",
                    coffeespoon: "Kávéskanál",
                    glass: "Pohár",
                    cup: "Bögre",
                    handful: "Marék",
                    packet: "Csomag",
                },
                recipeDifficulty: {
                    difficulty: "Nehézség",
                    easy: "Könnyű",
                    medium: "Átlagos",
                    hard: "Nehéz",
                },

                //User profile page translations
                userPage: {
                    fullName: "Teljes név",
                    password: "Jelszó",
                    email: "E-mail",
                    bio: "Rólam",
                    language: "Nyelv",
                    uploadImage: "Kép feltöltése",
                    save: "Mentés",
                    recipesBy: " Receptjei",
                },

                //Home page translations
                homePage: {
                    search: "Keresés",
                    tags: "Címkék",
                    tagsList: {
                        expensive: "Drága",
                        cheap: "Olcsó",
                        vegan: "Vegán",

                        easy: "Könnyű",
                        medium: "Átlagos",
                        hard: "Nehéz",

                        appetizer: "Előétel",
                        maindish: "Főétel",
                        dessert: "Desszert",
                    },
                    ingredients: "Hozzávalók",
                    recipesHeader: "Receptek",
                    showMore: "Mutass többet",

                    reviewSingular: "értékelés",
                    reviewPlural: "értékelés",

                    polls: "Szavazások",

                    viewAll: "Összes megtekintése",
                    signInToVote: "Lépj be a szavazáshoz!",
                },

                //Login page translations
                loginPage: {
                    signIn: "Bejelentkezés",
                    logIn: "Bejelentkezés",
                    email: "E-mail cím",
                    emailPattern: "Kérjük használja a következő formátumot:  info@recepttar.hu",
                    continue: "Tovább",
                    or: "vagy",
                    createAccount: "Fiók létrehozása",
                    discoverRecipes: "Receptek böngészése",

                    password: "Jelszó",
                    passwordRequirements: "Jelszókövetelmények: legalább 8 karakter, és tartalmaznia kell egy számot",
                },

                registerPage: {
                    register: "Regisztráció",
                    fullName: "Teljes név",
                    fullNameRequirements: "Legalább 3 karakter szükséges",
                },

                //Leaderboard page translations
                leaderboardPage: {
                    leaderboardHeader: "Ranglista",
                    sortBy: {
                        FavoriteCount: "Legnépszerűbb",
                        AvgRating: "Legjobbra értékelt",
                        RecipeCount: "Legaktívabb",
                    },
                    recipes: "recept",
                },

                //My-collection page translations
                myCollectionPage: {
                    myFavorites: "Kedvenceim",
                    myPolls: "Szavazásaim",
                    myRecipes: "Recepteim",
                },

                recipeViewPage: {
                    totalTime: "Teljes idő",
                    timeUnit: {
                        min: "perc",
                        hour: "óra",
                    },
                    servings: "Adag",
                    author: "Készítő",
                    description: "Leírás",
                    instructions: "Elkészítés",
                    ingredientsListHeader: "Hozzávalók",
                    portions: "Adag",
                    foodType: "Típus",

                    reviews: "Vélemények",
                    editReviewHeader: "Vélemény szerkesztése",
                    cancel: "Mégse",
                    saveChanges: "Mentés",
                    deleteButton: "Törlés",
                    submitReview: "Beküldés",
                    WriteAReview: "Írj véleményt",
                    shareThoughts: "Oszd meg véleményed erről a receptről...",

                    deleteRecipeHeader: "Recept törlése",
                    deleteRecipeMessage: "Biztosan törölni szeretnéd ezt a receptet?",
                },

                createEditRecipePage: {
                    createNewRecipeHeader: "Új recept létrehozása",
                    editRecipeHeader: "Recept szerkesztése",
                    title: "Cím",
                    noImageSelected: "Nincs kép kiválasztva",
                    chooseFile: "Fájl kiválasztása",
                    noFileChosen: "Nincs fájl kiválasztva",
                    descriptionPlaceholder: "Rövid leírás a receptről...",
                    createRecipe: "Recept létrehozása",
                    updateRecipe: "Recept mentése",

                    recipeDifficulty: "Nehézségi szint",
                    prepTime: "Elkészítési idő (perc)",
                    servings: "Adag",
                    price: "Ár",
                    dishType: "Étel típusa",

                    selectedIngredients: "Kiválasztott hozzávalók",

                    type: "Típus",
                    recipeStepTooltip: "Kattints és húzd a lépések bal oldalát az átrendezéshez",
                    addStep: "Lépés hozzáadása",
                },

                //Components translations
                pollCard: {
                    editPoll: "Szerkesztés",
                    deletePoll: "Törlés",
                    submitVote: "Szavazat leadása",
                    alreadyVoted: "Már szavaztál",

                    DeletePollConfirmHeader: "Szavazás törlése",
                    DeletePollConfirmMessage: "Biztosan törölni szeretnéd ezt a szavazást?",
                },

                createPollCard: {
                    pollPageHeader: "Indíts saját szavazást!",
                    pollHeaderShort: "Új létrehozása!",
                },

                createCardForm: {
                    question: "Kérdés",
                    questionPlaceholder: "Írd ide a kérdésed...",
                    options: "Válaszlehetőségek",
                    optionsPlaceholder: "Válasz",
                    addOptions: "Válasz hozzáadása",
                    submit: "Beküldés",
                    cancel: "Mégse",
                    update: "Frissítés",
                }
            }
        }
    },
    lng: savedLng,
    fallbackLng: 'en',
    interpolation: { escapeValue: false }
});

export default i18n;
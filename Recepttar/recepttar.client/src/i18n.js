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

                userPage: {
                    fullName: "Full name",
                    password: "Password",
                    email: "Email",
                    bio: "Bio",
                    language: "Language",
                    uploadImage: "Upload image",
                    save: "Save",
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

                pollCard: {
                    editPoll: "Edit",
                    deletePoll: "Delete",
                    submitVote: "Submit",
                    alreadyVoted: "Already voted",
                },

                createPollCard: {
                    pollPageHeader: "Create your own!",
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
                    rankLevel: "Rang szint",
                    HomeCook: "Kezdő szakács",
                    ChefMaster: "Haladó szakács",
                    FoodLegend: "Főszakács",
                },

                //User profile page translations
                userPage: {
                    fullName: "Teljes név",
                    password: "Jelszó",
                    email: "Email",
                    bio: "Bemutatkozás",
                    language: "Nyelv",
                    uploadImage: "Kép feltöltése",
                    save: "Mentés",
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

                //Components translations
                pollCard: {
                    editPoll: "Szerkesztés",
                    deletePoll: "Törlés",
                    submitVote: "Szavazat leadása",
                    alreadyVoted: "Már szavaztál",
                },

                createPollCard: {
                    pollPageHeader: "Indíts saját szavazást!",
                },

                createCardForm: {
                    question: "Kérdes",
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
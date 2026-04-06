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
                    bio: "Bemutatkozás",
                    language: "Language",
                    uploadImage: "Upload image",
                    save: "Save",
                },

                homePage: {
                    search: "Search",
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
                    myRecipes: "Receptjeim",
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
                }
            }
        }
    },
    lng: savedLng,
    fallbackLng: 'en',
    interpolation: { escapeValue: false }
});

export default i18n;
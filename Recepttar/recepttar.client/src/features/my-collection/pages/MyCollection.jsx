import { useAuth } from '../../../shared/hooks/useAuthContext';
import useGetUserCollections from "../hooks/useGetUserCollections";
import RecipeCard from "../../main/components/recipe-card/Card";
import PollCard from "../../main/components/poll-section/PollCard";
import ContainerLayout from "../components/containerLayout";

export default function MyCollection() {
    const { isLoggedIn, profileData } = useAuth();
    const { favoriteRecipes, userRecipes, userPolls } = useGetUserCollections(profileData?.id);

    return (
        <div className="w-100">
            <div className="m-4 p-2 main-green rounded-2" id="favorites">
                <h2 className="text-white fw-bold">My favorites</h2>
            </div>
            <ContainerLayout>
                {
                    favoriteRecipes.map(recipe => (
                        <RecipeCard key={recipe.recipeId} data={recipe} allowFavorites={true} />
                    ))
                }
            </ContainerLayout>

            <div className="w-100">
                <div className="m-4 p-2 main-green rounded-2" id="polls">
                    <h2 className="text-white fw-bold">My polls</h2>
                </div>
                <ContainerLayout>
                    {
                        userPolls.map(poll => (
                            <PollCard key={poll.id} data={poll} profileID={profileData} />
                        ))
                    }
                </ContainerLayout>
            </div>
            
            <div className="m-4 p-2 main-green rounded-2" id="recipes">
                <h2 className="text-white fw-bold">My recipes</h2>
            </div>
            <ContainerLayout>
                {
                    userRecipes.map(recipe => (
                        <RecipeCard key={recipe.recipeId} data={recipe} allowFavorites={false} />
                    ))
                }
            </ContainerLayout>
        </div>
    );
}
import { useState, useEffect } from 'react';
import { useAuth } from '../../../shared/hooks/useAuthContext';
import useGetUserCollections from "../hooks/useGetUserCollections";
import RecipeCard from "../../main/components/recipe-card/Card";
import PollCard from "../../main/components/poll-section/PollCard";
import ContainerLayout from "../components/containerLayout";
import CreatePollCard from "../../poll/components/create-card/CreatePollCard";
import CreatePollForm from "../../poll/components/create-poll-form/CreatePollFrom";
import { createPoll } from '../../poll/hooks/useCreatePoll';
import { usePolls } from '../../main/hooks/usePoll';
import ErrorBox from '../../../shared/components/error-box/ErrorBox';

export default function MyCollection() {
    const { isLoggedIn, profileData } = useAuth();
    const { polls, deletePoll } = usePolls();
    const { isFormOpen, openForm, pollValues } = createPoll();
    const { favoriteRecipes, userRecipes, userPolls } = useGetUserCollections(profileData?.id);
    
    const [pollCards, updatePolls] = useState([]);
    const [error, setError] = useState('');
    const [errorVisible, setErrorVisible] = useState(false);

    useEffect(() => {
        if(error !== ''){
            setErrorVisible(true);
        }
    }, [error]);

    useEffect(() => {
        updatePolls(userPolls);
    }, [userPolls]);

    function deletePollCard(pollId) {
        deletePoll(pollId);
        updatePolls(prevPolls => prevPolls.filter(poll => poll.id !== pollId));
    }

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
                    <CreatePollCard openFormTrigger={openForm} caption={"Create new!"} />
                    {
                        pollCards.map(poll => (
                            <PollCard key={poll.id} data={poll} profileID={profileData} deletePollMethod={deletePollCard} />
                        ))
                    }

                    {
                        (isFormOpen) ? 
                        (
                            <CreatePollForm isFormOpen={isFormOpen} openForm={openForm} preData={pollValues} errorMessage={setError} >
                                <ErrorBox visible={errorVisible} errorMessage={error} clearError={setError} closeError={setErrorVisible}/>
                            </CreatePollForm>
                        ) : ( null )
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
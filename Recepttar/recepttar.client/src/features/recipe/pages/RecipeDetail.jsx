import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import NotFound from '../../../shared/pages/NotFound';
import useRecipe from '../hooks/useRecipe';
import RecipeInfo from '../components/RecipeInfo/RecipeInfo';
import IngredientsCard from '../components/RecipeInfo/IngredientsCard';
import Reviews from '../components/Review/Review';
import './print.css';

const RecipeDetail = () => {
    const { recipeId } = useParams();
    const { recipe, ingredients, reviews, author, error, refetchReviews } = useRecipe(recipeId);
    
    const { t } = useTranslation();

    if (error) {
        return <NotFound message="This recipe doesn't exist or has been removed." />;
    }

    if (!recipe) {
        return (
            <div className="d-flex justify-content-center py-5">
                <div className="spinner-border text-danger" role="status" style={{ width: '3rem', height: '3rem' }} />
            </div>
        );
    }

    return (
        <div className="container py-4 mt-4">
            <div className="row g-4 rounded-2 container-bg-beige">
                <div className="col-lg-8">
                    <RecipeInfo recipe={recipe} author={author} reviews={reviews} t={t} />
                </div>
                <div className="col-lg-4">
                    <div className="row mx-auto">
                        <IngredientsCard ingredients={ingredients} t={t} />
                    </div>
                    <div className="row reviews mx-auto">
                        <Reviews reviews={reviews} recipeId={recipeId} onReviewAdded={refetchReviews} t={t} />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RecipeDetail;

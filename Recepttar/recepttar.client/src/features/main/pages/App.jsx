import Card from '../Components/RecipeCard/Card';

function Reicpes({ recipes }) {
    return (
        <>
            <h1 className="m-2 mt-4 ms-md-4">Recipes</h1>
            <div className="d-flex flex-wrap justify-content-center justify-content-md-start gap-3 m-2 ms-md-4 pb-3">
                {recipes.map(item => (
                    <Card key={item.id} data={item} />
                ))}
            </div>
        </>
    );
}

export default Reicpes;

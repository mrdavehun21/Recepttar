import axios from 'axios';

export async function getRecipe(recipeId){
    let repsonse = null;
    try {
        repsonse = await axios.get(`${import.meta.env.VITE_API_URL}/api/recipe/${recipeId}`);
        return repsonse.data;
    } catch (error) {
        console.error(error);
        return null;
    }
}

export async function updateRecipe(recipeId, recipeForm, setErrorMessage){
    let response = null;
    try{
        response = await axios.patch(`${import.meta.env.VITE_API_URL}/api/recipe/${recipeId}`, recipeForm, {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
        });
        setErrorMessage(null);
    }
    catch(error){
        const message = error?.response?.data?.errors;

        if(message){
            Object.keys(message).forEach(key => {
                if(key.includes("Ingredients") && key.includes("MeasurementUnit")){
                    message[key] = ["Invalid measurement unit"];
                }
                else if(key.includes("Ingredients") && key.includes("Quantity")){
                    message[key] = ["Invalid quantity"];
                }
            });
        }

        setErrorMessage(message);
    }
}
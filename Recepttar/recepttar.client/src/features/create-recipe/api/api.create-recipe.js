import axios from 'axios';

const API_BASE = import.meta.env.VITE_API_URL;

export async function getMeasurementUnit(setMeasurementUnits){
    axios.get(`${API_BASE}/api/ingredient/units`)
    .then((response) => {
        setMeasurementUnits(response.data);
    })
}

export async function uploadRecipe(recipeForm, setErrorMessage, userId){
    let response = null;
    try{
        response = await axios.post(`${API_BASE}/api/recipe/create`, recipeForm, {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
        });
        setErrorMessage(null);
        let newReciepeId = await axios.get(`${API_BASE}/api/recipe/${userId}/recipes`);
        return newReciepeId.data[newReciepeId.data.length - 1].recipeId;
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
import axios from 'axios';

const API_BASE = import.meta.env.VITE_API_URL;

export async function getMeasurementUnit(setMeasurementUnits){
    axios.get(`${API_BASE}/api/ingredient/units`)
    .then((response) => {
        setMeasurementUnits(response.data);
    })
}

export async function uploadRecipe(recipeForm, setErrorMessage){
    let response = null;
    try{
        response = await axios.post(`${API_BASE}/api/recipe/create`, recipeForm, {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
        });
    }
    catch(error){
        const message = error?.response?.data?.errors;

        // Replace every Ingredients[n].MeasurementUnit[] text with "Invalid measurement unit"
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
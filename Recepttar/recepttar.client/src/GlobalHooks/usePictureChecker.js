import axios from "axios";

export async function ImageAvailable(url = '') {
    if (url.trim() == '') {
        return false;
    }
    try {
        const response = await axios.get(url);
        if (response.data == '') return false;
        return true;
    }
    catch (error) {
        return false;
    }
}
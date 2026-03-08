import axios from "axios";

export async function patchPollAPI(form, pollId) {
    try {
        const response = await axios.patch(`https://localhost:7035/api/poll/${pollId}`, form);

        if (response.status !== 200) {
            throw new Error("Failed to update poll");
        }
    } catch (error) {
        console.error("Error updating poll:", error);
        throw error;
    }
}

export async function createPollAPI(form) {
    try {
        const response = await axios.post(`https://localhost:7035/api/poll/create`, form);

        if (response.status !== 201) {
            throw new Error("Failed to create poll");
        }
    } catch (error) {
        console.error("Error creating poll:", error);
        throw error;
    }
}
import { useState } from "react";
import { validateEmailInput, validatePasswordInput } from "../../auth/hooks/authHelper";
import { UpdateUserAPI } from "../api/profile.api";

export function useUpdateUser(form) {

    const UpdateUser = async (form, setError) => {
        const data = Object.fromEntries(form.entries());

        const email = data.Email;
        if(email != ''){
            const isValidEmail = validateEmailInput(email);
            if(!isValidEmail) { setError({ message: "Please enter a valid email", isValidatingIssue: true }); return; }
        }

        const password = data.Password;
        if(password != ''){
            const isValidPassword = validatePasswordInput(password);
            if(!isValidPassword) { setError({ message: 'Please enter a valid password', isValidatingIssue: true }); return; }
        }

        return await UpdateUserAPI(form, setError);
    }

    return {
        UpdateUser
    };
}
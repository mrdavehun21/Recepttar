import { useState } from "react";
import { patchPollAPI, createPollAPI } from "../api/poll.api";

export default function usePollForm(preData = [null, null]) {
    const [options, setOptions] = useState(preData.options == undefined ? [null, null] : preData.options);

    async function handleSubmit(e) {
        e.preventDefault();

        const form = e.target;

        const FilteredForm = document.createElement("form");

        const Question = document.createElement("input");
        Question.name = "Question";
        Question.value = form.Question.value;
        FilteredForm.appendChild(Question);

        options.forEach((option, index) => {
            const optionValue = form[`options[${index}].OptionText`].value;

            if (optionValue.trim() !== "") {
                const input = document.createElement("input");
                input.name = `options[${index}].OptionText`;
                input.value = optionValue;
                FilteredForm.appendChild(input);
            }
        });

        try {
            if (preData?.id !== undefined) {
                await patchPollAPI(FilteredForm, preData.id);
            } else {
                await createPollAPI(FilteredForm);
            }
        } catch (error) {
            console.error("Error creating poll:", error);
        }

        window.location.reload();
    }

    function handleAddOption(e) {
        e.preventDefault();
        setOptions((prev) => [...prev, null]);
    }

    return {
        options,
        handleSubmit,
        handleAddOption
    };
}
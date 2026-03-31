import { useState } from "react";
import { patchPollAPI, createPollAPI } from "../api/poll.api";

export default function usePollForm(preData = [null, null], errorMessage) {
    const [options, setOptions] = useState(
        preData?.options?.length
          ? preData.options.map((o, i) => ({
              id: i + 1,
              text: o.optionText || ""
            }))
          : [
              { id: 1, text: "" },
              { id: 2, text: "" }
            ]
      );

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
            // error.response.data
            if(error.response.data.title == undefined){
                errorMessage(error.response.data);
            }
            else{
                errorMessage(error.response.data.title);
            }
            return;
        }

        window.location.reload();
    }

    function handleAddOption(e) {
        e.preventDefault();
        setOptions((prev) => [
          ...prev,
          {
            id: Math.max(...prev.map(o => o.id), 0) + 1,
            text: ""
          }
        ]);
      }

      function handleRemoveOption(id) {
        setOptions((prev) => prev.filter(o => o.id !== id));
      }

    return {
        options,
        handleSubmit,
        handleAddOption,
        handleRemoveOption,
        setOptions
    };
}
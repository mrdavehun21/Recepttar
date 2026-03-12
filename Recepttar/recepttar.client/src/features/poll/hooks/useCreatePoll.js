import { useState } from 'react';

export function createPoll() {
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [pollValues, setPollValues] = useState([null, null]);

    const openForm = () => {
        if(isFormOpen) {
            setIsFormOpen(false);
            setPollValues([null, null]);
        } else {
            setIsFormOpen(true);
        }
    };

    const setPollValue = (input) => {
        setPollValues(input);
    };

    return {
        isFormOpen,
        openForm,
        pollValues,
        setPollValue
    };
}
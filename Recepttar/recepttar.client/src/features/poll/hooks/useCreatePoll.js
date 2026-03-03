import { useState } from 'react';

export function createPoll() {
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [pollValues, setPollValues] = useState([]);

    const openForm = () => {
        if(isFormOpen) {
            setIsFormOpen(false);
            setPollValues([]);
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
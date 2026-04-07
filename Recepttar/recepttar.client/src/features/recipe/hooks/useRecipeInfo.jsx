const useRecipeInfo = () => {

    const formatTime = (minutes, t) => {
        if (minutes < 60) return `${minutes} ${t("recipeViewPage.timeUnit.min")}`;
        const hours = Math.floor(minutes / 60);
        const mins = minutes % 60;
        return mins === 0 ? `${hours} ${t("recipeViewPage.timeUnit.hour")}${hours > 1 ? 's' : ''}` : `${hours} ${t("recipeViewPage.timeUnit.hour")}${hours > 1 ? 's' : ''} ${mins} ${t("recipeViewPage.timeUnit.min")}`;
    };

    const getDifficultyColor = (difficulty) => {
        switch (difficulty?.toLowerCase()) {
            case 'easy': return 'success';
            case 'medium': return 'warning';
            case 'hard': return 'danger';
            default: return 'secondary';
        }
    };

    const getDifficultyIcon = (difficulty) => {
        switch (difficulty?.toLowerCase()) {
            case 'easy': return 'bi bi-check-circle';
            case 'medium': return 'bi bi-dash-circle';
            case 'hard': return 'bi bi-exclamation-circle';
            default: return 'bi bi-circle';
        }
    };

    const getDishTypeIcon = (type) => {
        switch (type) {
            case 'Appetizer': return 'bi bi-shop';
            case 'MainDish': return 'bi bi-egg-fried';
            case 'Dessert': return 'bi bi-cake2';
            case 'Soup': return 'bi bi-cup-hot';
            case 'SideDish': return 'bi bi-grid';
            default: return 'bi bi-egg-fried';
        }
    };

    const renderStars = (avg) => {
        const rounded = Math.round(parseFloat(avg) * 2) / 2;
        return [1, 2, 3, 4, 5].map(s => {
            const full = s <= Math.floor(rounded);
            const half = !full && s - 0.5 === rounded;
            return (
                <i key={s}
                    className={`bi ${full ? 'bi-star-fill star-additional-9 me-2' : half ? 'bi-star-half star-additional-9 me-2' : 'bi-star star-additional-10 me-2'}`} />
            );
        });
    };

    return { formatTime, getDifficultyColor, getDifficultyIcon, getDishTypeIcon, renderStars };
};

export default useRecipeInfo;
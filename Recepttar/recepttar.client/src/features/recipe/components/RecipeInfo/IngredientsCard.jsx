import { useState } from 'react';

const IngredientsCard = ({ ingredients, t }) => {
    const [portions, setPortions] = useState(1);
    const [dropdownOpen, setDropdownOpen] = useState(false);

    return (
        <div className="card ingredients">
            <div className="card-body">
                <div className="d-flex align-items-center gap-3 mb-2">
                    <h5 className="card-title">{t("recipeViewPage.ingredientsListHeader")}</h5>
                    <div style={{ position: 'relative', display: 'inline-block' }}>
                        <button
                            onClick={() => setDropdownOpen(!dropdownOpen)}
                            style={{
                                background: 'white', border: '1px solid #ccc', borderRadius: '8px', padding: '6px 12px',
                                cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '8px'}}>
                            {portions}x {t("recipeViewPage.portions")}<i className="bi bi-chevron-down" />
                        </button>
                        {dropdownOpen && (
                            <div style={{
                                position: 'absolute', top: '100%', left: 0, background: 'white', border: '1px solid #ccc',
                                borderRadius: '8px', zIndex: 1000, minWidth: '120px', boxShadow: '0 2px 8px rgba(0,0,0,0.15)'}}>
                                {[1, 2, 3].map(p => (
                                    <div
                                        key={p} onClick={() => { setPortions(p); setDropdownOpen(false); }}
                                        style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '8px' }}
                                        onMouseEnter={e => e.currentTarget.style.background = '#f0f0f0'}
                                        onMouseLeave={e => e.currentTarget.style.background = 'white'}>
                                        {p}x {t("recipeViewPage.portions")}
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>

                {ingredients.map((ingredient, index) => (
                    <div key={index}>
                        <input type="checkbox" className="form-check-input" id={`ingredient${index}`} />
                        <label className="label-style" htmlFor={`ingredient${index}`}>
                            {ingredient.quantity * portions} {ingredient.measurementUnit} {ingredient.ingredientName}
                        </label>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default IngredientsCard;
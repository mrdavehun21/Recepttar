import { useState, useEffect, use } from "react";

function RecipeBasic({ errors, recipeData, t }) {
  const [image, setImage] = useState(null);
  const [title, setTitle] = useState(recipeData?.title || "");
  const [description, setDescription] = useState("");

  const MAX_CHARS = 25;
  const remaining = MAX_CHARS - title.length;
  
  const MAX_DESC_CHARS = 150;
  const remainingDesc = MAX_DESC_CHARS - description.length;

  const handleChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(URL.createObjectURL(file));
    }
  };

  const UpdateTitle = (e) => {
    const value = e.target.value.slice(0, MAX_CHARS);
    setTitle(value);
  };

  const UpdateDescription = (e) => {
    if(e.target.value.length > MAX_DESC_CHARS){
      e.target.value = e.target.value.slice(0, MAX_DESC_CHARS);
      return;
    }
    setDescription(e.target.value);
  }

  useEffect(() => {
    if(recipeData != null){
      setTitle(recipeData.title);
      setDescription(recipeData.description);
      setImage(import.meta.env.VITE_API_URL + "/" + recipeData.dishPicture);
    }
  }, [recipeData]);

  return (
    <div className="container p-0 d-flex shadow">
      <div className="card p-4 text-center w-100">
        <div className={"p-2 bg-danger text-white text-start " + (errors?.Title == null ? "d-none" : "")}>{errors?.Title}&nbsp;</div>
        <input type="text" id="recipeTitle" name="Title" placeholder={t("createEditRecipePage.title")} className="form-control mb-3 border-0 border-bottom border-black" onChange={UpdateTitle} value={title} /> 

        <div className="d-flex justify-content-between align-items-center mb-2">
            <div className={`text - end small mb-2 ${remaining <= 5 ? 'text-danger' : 'text-muted'}`}>
                {remaining} / {MAX_CHARS}
            </div>
        </div>
       
        {image ? (
          <img src={image} alt="Preview" className="img-fluid mb-3 rounded" style={{ maxHeight: "260px", objectFit: "cover" }}/>
        ) : (
          <div>
            <div className={"p-2 bg-danger text-white text-start " + (errors?.DishPicture == null ? "d-none" : "")}>{errors?.DishPicture}&nbsp;</div>
            <div className="mb-3 d-flex align-items-center justify-content-center border rounded" style={{height: "200px", borderStyle: "dashed", color: "#888"}}>
              {t("createEditRecipePage.noImageSelected")}
            </div>
          </div>
        )}

        <input type="file" id="imgUpload" name="DishPicture" accept="image/*" className="form-control" onChange={handleChange} />

        <div className={"p-2 bg-danger text-white text-start mt-3 " + (errors?.Description == null ? "d-none" : "")}>{errors?.Description}&nbsp;</div>
        <textarea className="form-control review-bg mb-2 mt-3" name="Description" placeholder={t("createEditRecipePage.descriptionPlaceholder")} value={description} onChange={e => UpdateDescription(e)} style={{ resize: 'none' }} rows={5} />
        <div className="d-flex justify-content-between align-items-center mb-2">
            <div className={`text - end small mb-2 ${remainingDesc <= 20 ? 'text-danger' : 'text-muted'}`}>
                {remainingDesc} / {MAX_DESC_CHARS}
            </div>
        </div>

        <input type="submit" className="btn btn-primary mt-3" value={recipeData? t("createEditRecipePage.updateRecipe") : t("createEditRecipePage.createRecipe")} />
      
      </div>
    </div>
  );
}

export default RecipeBasic;
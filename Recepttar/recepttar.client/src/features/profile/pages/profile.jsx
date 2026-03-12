import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from '../../../shared/hooks/useAuthContext';
import { useUpdateUser } from '../hooks/useProfileUpdate';
import { useProfile } from '../hooks/useProfile';
import ErrorBox from '../../../shared/components/error-box/ErrorBox';
import Card from '../../main/components/recipe-card/Card';
import Logo from '../../../assets/Logo.png';
import './index.css';

function profile() {
  const API_BASE = import.meta.env.VITE_API_URL;

  const { profileId } = useParams();
  const { isLoggedIn, profileData } = useAuth();
  const { UpdateUser } = useUpdateUser();
  
  const [error, setError] = useState('');
  const [errorVisible, setErrorVisible] = useState(false);
  
  const { data, imageExists } = useProfile(profileId, profileData, isLoggedIn, setError);

  useEffect(() => {
    if(error !== ''){
      setErrorVisible(true);
    }
  }, [error]);

  return (
      <div className="d-flex align-items-center h-100 w-100 justify-content-center">
        <ErrorBox visible={errorVisible} errorMessage={error} clearError={setError} closeError={setErrorVisible}/>
          <form className="p-4 ms-auto me-auto mt-4 mb-4 container-bg-beige rounded-2 shadow w-75 w-min-320" onSubmit={async (e) => {
              e.preventDefault();

              const formData = new FormData(e.target);

              const success = await UpdateUser(formData, setError);

              if(success){
                window.location.reload();
              }
          }}>
          <div className="w-100 d-flex gap-3 gap-md-5 flex-md-row flex-column justify-content-around">
            <div className="d-flex flex-column align-items-center">
              <div className="m-3 bg-light rounded-circle shadow w-fit h-fit">
                {
                  (imageExists) ? (
                    <img className="rounded-circle m-2" src={`${API_BASE}/${data?.profilePicture}`} style={{width: "150px", height: "150px"}}/>
                  ) : (
                    <img className="rounded-circle m-2" src={Logo} style={{width: "150px", height: "150px"}}/>
                  )
                }
              </div>
                <p>Rank level - {data?.rank}</p>
                {
                (profileId !== undefined) ? (
                  null
                ) : (
                  <label className="upload-btn polls-bg-additional-7 text-white w-100">
                    <span className="d-block ms-auto me-auto w-fit">Upload image</span>
                    <input type="file" name="ProfilePicture" accept="image/*" hidden />
                  </label>
                )
              }
            </div>
            <div className="w-100 w-max-510">
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 w-fit">Full name</span>
                <input name="Name" className="d-block w-100 p-2 rounded-3 border border-black" type="text" defaultValue={data?.fullName} disabled={profileId != undefined}/>
              </div>
              <div className={"w-100 w-md-75 mx-md-auto " + (profileId != undefined ? "d-none" : "")}>
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">Password</span>
                <input name="Password" className="p-2 w-100 rounded-3 border border-black" type="password" disabled={profileId != undefined}/>
              </div>
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">Email</span>
                <input name="Email" className="p-2 w-100 rounded-3 border border-black" type="text" disabled={profileId != undefined} defaultValue={data?.email}/>
              </div>
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">Bio</span>
                <textarea name="Bio" className="p-2 w-100 rounded-3 border border-black" type="text" defaultValue={data?.bio} style={{height: "180px"}} disabled={profileId != undefined} />
              </div>

            </div>
          </div>
            {
              (profileId !== undefined) ? (
                <div className="w-100 rounded-3 pb-2 mt-5" style={{backgroundColor: "lightblue"}}>
                    <p className="ms-auto me-auto mt-2 text-decoration-underline fs-4 w-fit">Recipes by {data?.fullName}</p>
                    <div className="d-flex flex-wrap gap-3 justify-content-center w-100 p-2 pt-4">
                      {
                        data?.recipes.map((recipe) => (
                          <Card key={recipe.recipeId} data={recipe} allowFavorites={false} />
                        ))
                      }
                    </div>
                </div>
              ) : (
                <input className="d-block ms-auto mt-3 polls-bg-additional-8 text-white border border-black p-2 rounded-3" type="submit" value="Save" style={{ width: "100px" }} />
              )
            }
        </form>
    </div>
  );
}

export default profile;
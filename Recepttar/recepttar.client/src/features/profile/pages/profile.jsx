import { useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from '../../../shared/hooks/useAuthContext';
import { useUpdateUser } from '../hooks/useProfileUpdate';
import { useProfile } from '../hooks/useProfile';
import { useTranslation } from 'react-i18next';
import { Modal } from 'bootstrap';
import Card from '../../main/components/recipe-card/Card';
import NotFound from '../../../shared/pages/NotFound';
import Logo from '../../../assets/Logo.png';
import './index.css';

function Profile() {
  const API_BASE = import.meta.env.VITE_API_URL;
  
  const { profileId } = useParams();
  const { isLoggedIn, profileData } = useAuth();
  const { UpdateUser } = useUpdateUser();
  
  const [error, setError] = useState('');
  const [errorVisible, setErrorVisible] = useState(false);

  const modalRef = useRef(null);
  const bsModalRef = useRef(null);

  useEffect(() => {
    if (modalRef.current) {
        bsModalRef.current = new Modal(modalRef.current);
    }
  }, []);

  const handleDeleteClick = () => {
    setDeleteError(null);
    bsModalRef.current?.show();
  };
  
  const { data, imageExists } = useProfile(profileId, profileData, isLoggedIn, setError);

  const { t, i18n } = useTranslation();

  const Languages = [
    { code: 'en', name: 'English', flag: 'GB' },
    { code: 'hu', name: 'Hungarian', flag: 'HU' },
  ]

  useEffect(() => {
    if(error !== ''){
      setErrorVisible(true);
    }
  }, [error]);

  if(errorVisible && !error?.isValidatingIssue){
    return (<NotFound message="The user you are looking for does not exist." />);
  }

  function handleCloseError(){
    setError('');
    setErrorVisible(false);
  }
  
  return (
      <div className="d-flex align-items-center h-100 w-100 justify-content-center">
        <div className="modal fade" ref={modalRef} tabIndex="-1">
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">An error occoured</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" onClick={handleCloseError} />
                    </div>
                    <div className="modal-body">
                        <p>{error?.message || error}</p>
                    </div>
                    <div className="modal-footer">
                        <button className="btn btn-secondary" data-bs-dismiss="modal" onClick={handleCloseError}>Cancel</button>
                    </div>
                </div>
            </div>
        </div>

        <form className="p-4 ms-auto me-auto mt-4 mb-4 container-bg-beige rounded-2 shadow w-75 w-min-320" onSubmit={async (e) => {
              e.preventDefault();

              const formData = new FormData(e.target);

              const success = await UpdateUser(formData, setError);

              if(success){
                window.location.reload();
              }
              else{
                if(!error?.isValidatingIssue){
                  bsModalRef.current?.show();
                }
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
                <p>{t("userRank.rankLevel")} - {t(`userRank.${data?.rank}`)}</p>
                {
                (profileId !== undefined) ? (
                  null
                ) : (
                  <label className="upload-btn polls-bg-additional-7 text-white w-100">
                    <span className="d-block ms-auto me-auto w-fit">{t("userPage.uploadImage")}</span>
                    <input type="file" name="ProfilePicture" accept="image/*" hidden />
                  </label>
                )
              }
            </div>
            <div className="w-100 w-max-510">
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 w-fit">{t("userPage.fullName")}</span>
                <input name="Name" className="d-block w-100 p-2 rounded-3 border border-black" type="text" defaultValue={data?.fullName} disabled={profileId != undefined}/>
              </div>
              <div className={"w-100 w-md-75 mx-md-auto " + (profileId != undefined ? "d-none" : "")}>
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">{t("userPage.password")}</span>
                <input name="Password" className="p-2 w-100 rounded-3 border border-black" type="password" disabled={profileId != undefined}/>
              </div>
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">{t("userPage.email")}</span>
                <input name="Email" className="p-2 w-100 rounded-3 border border-black" type="text" disabled={profileId != undefined} defaultValue={data?.email}/>
              </div>
              <div className="w-100 w-md-75 mx-md-auto">
                <span className="d-block fs-4 fw-normal m-0 mt-3 w-fit">{t("userPage.bio")}</span>
                <textarea name="Bio" className="p-2 w-100 rounded-3 border border-black" type="text" defaultValue={data?.bio} style={{height: "180px"}} disabled={profileId != undefined} />
              </div>

              <div className={profileId === undefined ? "" : "d-none"}>
                <span className="d-block fs-4 fw-normal m-0 mt-2 w-fit">{t("userPage.language")}</span>
                <select className="form-select mb-3" name="" id="" value={i18n.language} onChange={(e) => { i18n.changeLanguage(e.target.value); localStorage.setItem('i18nextLng', e.target.value);}}>
                  {
                    Languages.map((language) => (
                      <option key={language.code} value={language.code}>{language.flag} {language.name}</option>
                    ))
                  }
                </select>
              </div>
            </div>
          </div>
            {
              (profileId !== undefined) ? (
                <div className="w-100 rounded-3 pb-2 mt-5" style={{backgroundColor: "lightblue"}}>
                    <p className="ms-auto me-auto mt-2 text-decoration-underline fs-4 w-fit">{data?.fullName + t("userPage.recipesBy")}</p>
                    <div className="d-flex flex-wrap gap-3 justify-content-center w-100 p-2 pt-4">
                      {
                        data?.recipes.map((recipe) => (
                          <Card key={recipe.recipeId} data={recipe} allowFavorites={false} t={t} />
                        ))
                      }
                    </div>
                </div>
              ) : (
                <input className="d-block ms-auto mt-3 polls-bg-additional-8 text-white border border-black p-2 rounded-3" type="submit" value={t("userPage.save")} style={{ width: "100px" }} />
              )
            }
        </form>
    </div>
  );
}

export default Profile;
import { useEffect, useState } from 'react';
import { usePolls } from '../../main/hooks/usePoll';
import { createPoll } from '../hooks/useCreatePoll';
import { useTranslation } from 'react-i18next';

import ErrorBox from '../../../shared/components/error-box/ErrorBox';
import CreatePollCard from '../components/create-card/CreatePollCard';
import CreatePollForm from '../components/create-poll-form/CreatePollForm';
import PollCard from '../../main/components/poll-section/PollCard';

function poll({ loginStatus, profileID }) {
    const { t } = useTranslation();

    const { polls, deletePoll } = usePolls();

    const { isFormOpen, openForm, pollValues, setPollValue } = createPoll();

    const [error, setError] = useState('');
    const [errorVisible, setErrorVisible] = useState(false);

    useEffect(() => {
      if(error !== ''){
        setErrorVisible(true);
      }
    }, [error]);

    return (
        <div className="h-100">
          <div className='d-flex flex-wrap justify-content-center ms-auto me-auto mt-5'>
            <CreatePollCard openFormTrigger={openForm} caption={t("createPollCard.pollPageHeader")} />
              {
                polls.map((item, index) => (
                  <PollCard key={index} data={item} loginStatus={loginStatus} profileID={profileID} deletePollMethod={deletePoll} openFormTrigger={openForm} returnPollValues={setPollValue} t={t} />
              ))
            }
          </div>

          {
            (isFormOpen) ? (
              <CreatePollForm isFormOpen={isFormOpen} openForm={openForm} preData={pollValues} errorMessage={setError} hhhh={t} >
                <ErrorBox visible={errorVisible} errorMessage={error} clearError={setError} closeError={setErrorVisible}/>
              </CreatePollForm>
            ) : ( null )
          }
      </div>
  );
}

export default poll;
import CreatePollCard from '../Components/CreateCard/CreatePollCard'
import CreatePollForm from '../Components/CreatePollForm/CreatePollFrom';

import PollCard from '../../main/Components/PollSection/PollCard';
import { usePolls } from '../../main/hooks/usePoll';

import { createPoll } from '../hooks/useCreatePoll';
import { useEffect } from 'react';

function poll({ loginStatus, profileID }) {
  const { polls, deletePoll } = usePolls();

    const { isFormOpen, openForm, pollValues, setPollValue } = createPoll();

    return (
        <div className="h-100">

          <div className='d-flex flex-wrap justify-content-center ms-auto me-auto mt-5'>
            <CreatePollCard openFormTrigger={openForm} />
              {
                polls.map((item, index) => (
                    <PollCard key={index} data={item} loginStatus={loginStatus} profileID={profileID} deletePollMethod={deletePoll} openFormTrigger={openForm} returnPollValues={setPollValue} />
              ))
            }
          </div>

          {
            (isFormOpen) ? 
              (<CreatePollForm isFormOpen={isFormOpen} openForm={openForm} preData={pollValues} />) : ( null )
          }
      </div>
  );
}

export default poll;
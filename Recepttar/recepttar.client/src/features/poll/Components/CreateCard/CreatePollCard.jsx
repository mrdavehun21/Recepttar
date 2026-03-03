import '../../../main/Components/PollSection/PollCard.css';

function CreatePollCord({ openFormTrigger }) {
  return (
    <div className="card overflow-hidden shadow m-3 rounded-3 vote-card d-flex justify-content-evenly" style={{height:"-webkit-fill-available"}}>
      <h3 className="ms-auto me-auto mt-3 mb-4">Create your own!</h3>
      <div className="bg-light border shadow ms-auto me-auto mb-4 d-flex justify-content-center align-items-center" style={{ borderRadius: "50%", width: "80px", height: "80px" }} onClick={openFormTrigger}>
        <i className="bi bi-plus fs-1"></i>
      </div>
    </div>
  );
}

export default CreatePollCord;
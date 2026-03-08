import '../../../main/Components/PollSection/PollCard.css';

function CreatePollCard({ openFormTrigger }) {
  return (
    <div className="card overflow-hidden shadow m-3 rounded-3 vote-card d-flex justify-content-evenly h-fill-available">
      <h3 className="ms-auto me-auto mt-3 mb-4">Create your own!</h3>
      <div className="bg-light border shadow ms-auto me-auto mb-4 d-flex justify-content-center align-items-center rounded-circle" style={{width: "80px", height: "80px" }} onClick={openFormTrigger}>
        <i className="bi bi-plus fs-1"></i>
      </div>
    </div>
  );
}

export default CreatePollCard;
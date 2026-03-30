import { useNavigate } from 'react-router-dom';
import SadEmoji from '../../assets/notfound.jpg'

const NotFound = ({message = "This page is still cooking… It's not ready yet, or it never existed."}) => {
    const navigate = useNavigate();

    return (
        <div className="container py-5 mt-5 text-center">
            <div className="mb-4">
                <img className="img-fluid" src={SadEmoji} style={{ maxHeight: '150px' }} />
                <h1 className="fw-bold mt-3">404</h1>
                <p className="text-muted">{message}</p>
            </div>

            <button
                className="btn polls-bg-additional-8 text-light mb-5"
                onClick={() => navigate('/home')}
            >
                <i className="bi bi-arrow-left-circle me-2"></i>
                Take me back to something edible
            </button>
        </div>
    );
};

export default NotFound;
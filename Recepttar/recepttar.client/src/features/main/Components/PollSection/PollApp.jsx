import PollCard from './PollCard';
import { usePolls } from '../../hooks/usePoll';
import './PollApp.css';

import Nav from 'react-bootstrap/Nav';

function PollApp({ loginStatus }) {
    const { polls } = usePolls();

    return (
        <div className="position-relative mt-4 bg-poll rounded-3 ms-auto me-auto mb-3 h-100">
            {
                (!loginStatus) ? (
                    <div className="position-absolute w-100 h-100 backdrop-blur d-flex align-items-center justify-content-center">
                        <h3 className="color-red">Sign in to vote!</h3>
                    </div>
                ) : (
                    <div className="d-none"></div>
                )
            }
            <h2 className="m-2 mt-2 mb-4 ms-2 text-decoration-underline color-neutral-100 fs-3">Polls</h2>
            <div
                className="d-block d-md-flex m-1 flex-column align-items-center overflow-auto"
                style={{ scrollBehavior: 'smooth' }}
            >
                {
                    (!loginStatus) ? (
                        polls.slice(0, 1).map((item, index) => (
                            <PollCard key={index} data={item} loginStatus={loginStatus} />
                        ))
                    ) : (
                        <div>
                            {
                                polls.slice(0, 4).map((item, index) => (
                                    <PollCard key={index} data={item} loginStatus={loginStatus} />
                                ))
                                }
                                <Nav.Link href="/polls" className="polls-bg-additional-8 p-2 rounded-2 text-light ms-auto me-auto mb-3" style={{width: "fit-content"} }>View all</Nav.Link>
                        </div>
                    )
                }
            </div>
        </div>
    );
}

export default PollApp;

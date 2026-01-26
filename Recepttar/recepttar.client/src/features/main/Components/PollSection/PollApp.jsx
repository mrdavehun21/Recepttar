import PollCard from './PollCard';
import { usePolls } from '../../hooks/usePoll';

function PollApp() {
    const { polls } = usePolls();

    //if (loading) return <p>Loading...</p>;
    //if (error) return <p>Error: {error}</p>;

    return (
        <div className="mt-5">
            <h1 className="m-2 ms-md-4">Polls</h1>
            <div
                className="d-flex m-2 ms-md-4 flex-column flex-sm-row align-items-center overflow-auto"
                style={{ scrollBehavior: 'smooth' }}
            >
                {polls.map((item, index) => (
                    <PollCard key={index} data={item} />
                ))}
            </div>
        </div>
    );
}

export default PollApp;

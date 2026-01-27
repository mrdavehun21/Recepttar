export default function NameSet({
    name,
    setName
}) {
    return (
        <>
            <label className="form-label fw-semibold">Display Name
                <i
                    className="bi bi-question-circle ms-2"
                    data-bs-toggle="tooltip"
                    data-bs-placement="right"
                    title="Please enter at least 3 characters"
                />
            </label>
            <input
                type="text"
                className="form-control form-control-lg login-input"
                value={name}
                onChange={(e) => setName(e.target.value)}
                autoFocus
            />
        </>
    )
}

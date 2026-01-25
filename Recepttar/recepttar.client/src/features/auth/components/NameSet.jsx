export default function NameSet({ name, setName }) {
    return (
        <div className="mb-3">
            <label className="form-label fw-semibold">Display Name</label>
            <input
                type="text"
                className="form-control form-control-lg login-input"
                value={name}
                onChange={(e) => setName(e.target.value)}
                autoFocus
            />
        </div>
    )
}

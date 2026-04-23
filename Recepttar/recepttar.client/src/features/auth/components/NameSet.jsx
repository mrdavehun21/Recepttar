export default function NameSet({
    name,
    setName,
    t
}) {
    return (
        <>
            <label className="form-label fw-semibold">{t("registerPage.fullName")}
                <i
                    className="bi bi-question-circle ms-2"
                    data-bs-toggle="tooltip"
                    data-bs-placement="right"
                    title={t("registerPage.fullNameRequirements")}
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

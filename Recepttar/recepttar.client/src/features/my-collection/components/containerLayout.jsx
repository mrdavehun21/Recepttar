export default function ContainerLayout({ children }) {
    return (
        <div className="container m-4 p-4 w-min-95 container-bg-beige rounded-2 shadow ms-auto me-auto">
            <div className="d-flex flex-wrap justify-content-center gap-3">
                {children}
            </div>
        </div>
    );
}
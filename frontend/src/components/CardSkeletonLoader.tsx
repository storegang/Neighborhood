export const CardSkeletonLoader: React.FC = () => {
    return (
        <div className="card flex w-full flex-col gap-4 shadow-sm">
            <div className="card-body">
                <div className="skeleton h-32 w-full"></div>
                <div className="skeleton h-4 w-28"></div>
                <div className="skeleton h-4 w-full"></div>
                <div className="skeleton h-4 w-full"></div>
            </div>
        </div>
    )
}

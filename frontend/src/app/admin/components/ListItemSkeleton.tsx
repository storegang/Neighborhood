export const ListItemSkeleton: React.FC = () => {
    return (
        <li className="list-row items-center" key="skeleton">
            <div className="skeleton h-8 w-8"></div>
            <div className="skeleton h-6 w-32"></div>
        </li>
    )
}

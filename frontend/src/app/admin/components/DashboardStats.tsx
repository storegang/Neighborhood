type DashboardStatsProps = {
    totalUsers: number
    postsLastMonth: number
    newUsersThisMonth: number
    totalProperites: number
}

export const DashboardStats: React.FC<DashboardStatsProps> = ({
    totalUsers,
    postsLastMonth,
    newUsersThisMonth,
    totalProperites,
}) => {
    const averagePostsLastMonth = postsLastMonth / totalUsers

    return (
        <div className="stats stats-vertical sm:stats-horizontal shadow">
            <div className="stat w-auto lg:w-full">
                <div className="stat-figure text-primary">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth="1.5"
                        stroke="currentColor"
                        className="size-6 sm:size-8"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z"
                        />
                    </svg>
                </div>
                <div className="stat-title text-sm sm:text-base">
                    Total Users
                </div>
                <div className="stat-value text-primary">
                    {totalUsers ? totalUsers : "50"}
                </div>
                <p className="stat-desc text-xs sm:text-sm">
                    {newUsersThisMonth ? newUsersThisMonth : 2} new users this
                    month
                </p>
            </div>

            <div className="stat">
                <div className="stat-figure text-secondary">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        className="inline-block h-8 w-8 stroke-current"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M13 10V3L4 14h7v7l9-11h-7z"
                        ></path>
                    </svg>
                </div>
                <div className="stat-title text-sm sm:text-base">
                    Posts the last 30 days
                </div>
                <div className="stat-value text-secondary">
                    {postsLastMonth ? postsLastMonth : "25"}
                </div>
                <p className="stat-desc text-xs sm:text-sm">
                    That's {averagePostsLastMonth} every day, on average
                </p>
            </div>

            <div className="stat">
                <div className="stat-figure text-secondary">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="size-6 sm:size-8"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M8.25 21v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21m0 0h4.5V3.545M12.75 21h7.5V10.75M2.25 21h1.5m18 0h-18M2.25 9l4.5-1.636M18.75 3l-1.5.545m0 6.205 3 1m1.5.5-1.5-.5M6.75 7.364V3h-3v18m3-13.636 10.5-3.819"
                        />
                    </svg>
                </div>
                <div className="stat-title text-sm sm:text-base">
                    Number of properties
                </div>
                <div className="stat-value text-accent">
                    {totalProperites ? totalProperites : "50"}
                </div>
                <p className="stat-desc text-secondary text-xs break-words sm:text-sm">
                    X houses, Y apartments
                </p>
            </div>
        </div>
    )
}

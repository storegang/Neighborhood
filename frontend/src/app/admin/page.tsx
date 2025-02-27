import { BoardMembersList, DashboardStats } from "./components"

export default function Page() {
    return (
        <main className="container mx-auto mt-6 max-w-screen-lg px-4">
            <section className="flex w-full flex-col gap-6">
                <DashboardStats
                    totalUsers={50}
                    postsLastMonth={20}
                    totalProperites={100}
                    newUsersThisMonth={2}
                />
                <BoardMembersList />
            </section>
        </main>
    )
}

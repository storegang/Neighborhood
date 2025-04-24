"use client"

import { UserListItem } from "./UserListItem"
import { useGetUsers } from "../queries"
import { useUser } from "@/lib/getUser"
import { ListItemSkeleton } from "./ListItemSkeleton"
import { TabPanel, Tabs } from "@/components/Tabs"

export const UsersList: React.FC = () => {
    const user = useUser()

    const { data: users, isLoading, isError } = useGetUsers(user ?? null)

    const boardMembers = users?.filter((user) =>
        user.roles.includes("BoardMember")
    )

    const tenants = users?.filter((user) => user.roles.includes("Tenant"))

    if (isLoading)
        return (
            <div className="card card-border border-base-300">
                <div className="card-body">
                    <h2 className="card-title">Members</h2>
                    <ul className="list">
                        {Array.from({ length: 5 }, (_, index) => (
                            <ListItemSkeleton key={index} />
                        ))}
                    </ul>
                </div>
            </div>
        )

    if (isError)
        return (
            <div className="card card-border border-base-300">
                <div className="card-body">
                    <h2 className="card-title">Members</h2>
                    <div role="alert" className="alert alert-error w-fit">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-6 w-6 shrink-0 stroke-current"
                            fill="none"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth="2"
                                d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
                            />
                        </svg>
                        <span>There was an error getting users.</span>
                    </div>
                </div>
            </div>
        )

    return (
        <div className="card card-border border-base-300">
            <div className="card-body">
                <h2 className="card-title">Members</h2>
                <Tabs name="users">
                    <TabPanel label="Board" defaultChecked>
                        <ul className="list">
                            {boardMembers?.map((user) => (
                                <UserListItem
                                    key={user.id}
                                    name={user.name}
                                    role={user.roles}
                                    phoneNumber={user.phoneNumber || "98765432"}
                                    avatar={user.avatar || ""}
                                    isBoardMember
                                />
                            ))}
                        </ul>
                    </TabPanel>
                    <TabPanel label="Tenants">
                        {isLoading && (
                            <ul className="list">
                                {Array.from({ length: 5 }, (_, index) => (
                                    <ListItemSkeleton key={index} />
                                ))}
                            </ul>
                        )}
                        <ul className="list">
                            {tenants?.map((user) => (
                                /* ts-expect-error Complains about not having a key, but it does */
                                <UserListItem
                                    key={user.id}
                                    name={user.name}
                                    avatar={user.avatar || ""}
                                />
                            ))}
                        </ul>
                    </TabPanel>
                </Tabs>
            </div>
        </div>
    )
}

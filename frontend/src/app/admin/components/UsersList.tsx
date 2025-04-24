"use client"

import { UserListItem } from "./UserListItem"
import { useGetUsers } from "../queries"
import { useUser } from "@/lib/getUser"
import { UserListItemSkeleton } from "./UserListItemSkeleton"
import { TabPanel, Tabs } from "@/components/Tabs"

export const UsersList: React.FC = () => {
    const user = useUser()

    const { data: users, isLoading, isError } = useGetUsers(user ?? null)

    const boardMembers = users?.filter((user) =>
        user.roles.includes("BoardMember")
    )

    const tenants = users?.filter((user) => user.roles.includes("Tenant"))

    return (
        <Tabs name="users">
            <TabPanel label="Board" defaultChecked>
                {isLoading && (
                    <ul className="list">
                        {Array.from({ length: 5 }, (_, index) => (
                            <UserListItemSkeleton key={index} />
                        ))}
                    </ul>
                )}
                <ul className="list">
                    {boardMembers?.map((user) => (
                        <UserListItem
                            key={user.uid}
                            name={user.name}
                            role="BoardMember"
                            phoneNumber="93629473"
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
                            <UserListItemSkeleton key={index} />
                        ))}
                    </ul>
                )}
                <ul className="list">
                    {tenants?.map((user) => (
                        <UserListItem
                            key={user.uid}
                            name={user.name}
                            phoneNumber={user.phoneNumber || ""}
                            avatar={user.avatar || ""}
                        />
                    ))}
                </ul>
            </TabPanel>
        </Tabs>
    )
}

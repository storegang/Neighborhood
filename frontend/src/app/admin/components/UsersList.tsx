"use client"

import { UserListItem } from "./UserListItem"
import { useGetUsers } from "../queries"
import { useUser } from "@/lib/getUser"
import { ListItemSkeleton } from "./ListItemSkeleton"
import { TabPanel, Tabs } from "@/components/Tabs"

export const UsersList: React.FC = () => {
    const user = useUser()

    const { data: users, isLoading } = useGetUsers(user ?? null)

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
                            <ListItemSkeleton key={index} />
                        ))}
                    </ul>
                )}
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
    )
}

"use client"

import { UserListItem } from "./UserListItem"
import { useGetUsers } from "../queries"
import { useUser } from "@/lib/getUser"
import { UserListItemSkeleton } from "./UserListItemSkeleton"
import { Alert } from "@/components"

export const UsersList: React.FC = () => {
    const user = useUser()

    const { data: users, isLoading, isError } = useGetUsers(user)

    return (
        <div className="tabs tabs-lift h-80 w-1/2">
            <input
                type="radio"
                name="my_tabs_3"
                className="tab"
                aria-label="Board"
                defaultChecked
            />
            <div className="tab-content bg-base-100 border-base-300 rounded-box overflow-y-scroll p-6 shadow-md">
                <ul className="list">
                    {isLoading ? (
                        <UserListItemSkeleton />
                    ) : isError ? (
                        <Alert
                            type="error"
                            message="Something went wrong, try again!"
                        />
                    ) : users?.length === 0 ? (
                        <Alert type="info" message="No users found!" />
                    ) : users && users.length > 0 ? (
                        users.map((user) => (
                            <UserListItem
                                key={user.uid}
                                name={user.name}
                                role="Board member"
                                phoneNumber="93629473"
                                avatar={user.avatar || ""}
                                isBoardMember
                            />
                        ))
                    ) : null}
                </ul>
            </div>

            <input
                type="radio"
                name="my_tabs_3"
                className="tab"
                aria-label="Users"
            />
            <div className="tab-content bg-base-100 border-base-300 rounded-box overflow-y-scroll p-6 shadow-md">
                <ul className="list">
                    {users?.map((user) => (
                        <UserListItem
                            key={user.uid}
                            name={user.name}
                            phoneNumber="93629473"
                            avatar={user.avatar || ""}
                        />
                    ))}
                </ul>
            </div>
        </div>
    )
}

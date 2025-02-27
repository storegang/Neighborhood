"use client"

import { UserListItem } from "./UserListItem"
import { useGetUsers } from "../queries"
import { useUser } from "@/lib/getUser"

export const UsersList: React.FC = () => {
    const user = useUser()

    const {
        data: users,
        isLoading: postsLoading,
        isError: postsError,
    } = useGetUsers(user)

    console.log(users)

    return (
        <div className="tabs tabs-lift h-80 w-1/2">
            <input
                type="radio"
                name="my_tabs_3"
                className="tab"
                aria-label="Board"
            />
            <div className="tab-content bg-base-100 border-base-300 rounded-box overflow-y-scroll p-6 shadow-md">
                <ul className="list">
                    {users?.map((user) => (
                        <UserListItem
                            key={user.uid}
                            name={user.name}
                            role="Board member"
                            phoneNumber="93629473"
                            avatar={user.avatar || ""}
                            isBoardMember
                        />
                    ))}
                </ul>
            </div>

            <input
                type="radio"
                name="my_tabs_3"
                className="tab"
                aria-label="Users"
                defaultChecked
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

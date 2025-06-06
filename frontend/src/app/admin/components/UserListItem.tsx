import { formatPhoneNumber } from "@/lib/formatters/formatPhoneNumber"
import { User } from "@/Models"
import Link from "next/link"

type UserListItemProps = {
    name: User["name"]
    role?: User["roles"]
    avatar: User["avatar"]
    isBoardMember?: boolean
    phoneNumber?: string
    countryCode?: string
    partial?: boolean
    email?: string
}

export const UserListItem: React.FC<UserListItemProps> = ({
    name,
    role,
    phoneNumber,
    avatar,
    countryCode,
    partial,
    email,
    isBoardMember,
}) => {
    const formattedRole = role?.includes("BoardMember") ? "Board Member" : role

    return (
        <li
            className={`list-row ${!isBoardMember ? "items-center" : ""}`}
            key={name}
        >
            <div>
                <img
                    alt={name}
                    className="rounded-box size-10 object-cover"
                    src={avatar}
                />
            </div>
            <div>
                <div>{name}</div>
                {isBoardMember ? (
                    <>
                        {role && (
                            <div className="text-xs font-semibold uppercase opacity-60">
                                {formattedRole}
                            </div>
                        )}
                        <div className="list-col-wrap flex flex-col gap-2 text-xs">
                            {phoneNumber && (
                                <p className="flex items-center gap-2">
                                    <svg
                                        className="size-[1.2em]"
                                        xmlns="http://www.w3.org/2000/svg"
                                        fill="none"
                                        viewBox="0 0 24 24"
                                        strokeWidth={1.5}
                                        stroke="currentColor"
                                    >
                                        <path
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            d="M2.25 6.75c0 8.284 6.716 15 15 15h2.25a2.25 2.25 0 0 0 2.25-2.25v-1.372c0-.516-.351-.966-.852-1.091l-4.423-1.106c-.44-.11-.902.055-1.173.417l-.97 1.293c-.282.376-.769.542-1.21.38a12.035 12.035 0 0 1-7.143-7.143c-.162-.441.004-.928.38-1.21l1.293-.97c.363-.271.527-.734.417-1.173L6.963 3.102a1.125 1.125 0 0 0-1.091-.852H4.5A2.25 2.25 0 0 0 2.25 4.5v2.25Z"
                                        />
                                    </svg>
                                    <Link
                                        className="link link-hover link-primary"
                                        href={`tel:${phoneNumber}`}
                                    >
                                        {formatPhoneNumber(phoneNumber, {
                                            countryCode,
                                            partial,
                                        })}
                                    </Link>
                                </p>
                            )}
                            {email && (
                                <p className="flex items-center gap-2">
                                    <svg
                                        className="size-[1.2em]"
                                        xmlns="http://www.w3.org/2000/svg"
                                        fill="none"
                                        viewBox="0 0 24 24"
                                        strokeWidth={1.5}
                                        stroke="currentColor"
                                    >
                                        <path
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            d="M16.5 12a4.5 4.5 0 1 1-9 0 4.5 4.5 0 0 1 9 0Zm0 0c0 1.657 1.007 3 2.25 3S21 13.657 21 12a9 9 0 1 0-2.636 6.364M16.5 12V8.25"
                                        />
                                    </svg>
                                    <Link
                                        className="link link-hover link-primary"
                                        href={`mailto:${email}`}
                                    >
                                        {email}
                                    </Link>
                                </p>
                            )}
                        </div>
                    </>
                ) : null}
            </div>
        </li>
    )
}

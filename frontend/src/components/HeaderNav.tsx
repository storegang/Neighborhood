import { useUser } from "@auth0/nextjs-auth0"
import React from "react"

type HeaderNavProps = {
    image?: string
}

export const HeaderNav: React.FC<HeaderNavProps> = () => {
    const links = [
        { name: "Home", href: "/" },
        { name: "Lend", href: "/lend" },
    ]

    const { user, error, isLoading } = useUser()

    if (isLoading) return
    if (error) return <div>{error.message}</div>

    return (
        <div className="navbar bg-base-100 shadow-sm">
            <div className="navbar-start"></div>
            <div className="navbar-center hidden lg:flex">
                <ul className="menu menu-horizontal px-1">
                    {links.map((link) => (
                        <li key={link.href}>
                            <a href={link.href}>{link.name}</a>
                        </li>
                    ))}
                </ul>
            </div>
            {!user && (
                <div className="navbar-end">
                    <a role="button" className="btn" href="/api/auth/login">
                        Login
                    </a>
                </div>
            )}
            {user && (
                <div className="navbar-end">
                    <div className="dropdown dropdown-end">
                        <div
                            tabIndex={0}
                            role="button"
                            className="btn btn-ghost btn-circle avatar"
                        >
                            <div className="w-10 rounded-full">
                                <img alt="Profile image" src={user.picture} />
                            </div>
                        </div>
                        <ul
                            tabIndex={0}
                            className="menu menu-sm dropdown-content bg-base-100 rounded-box z-1 mt-3 w-52 p-2 shadow"
                        >
                            <li>
                                <a className="justify-between">
                                    Profile
                                    <span className="badge">New</span>
                                </a>
                            </li>
                            <li>
                                <a>Settings</a>
                            </li>
                            <li>
                                <a href="/api/auth/logout">Logout</a>
                            </li>
                        </ul>
                    </div>
                </div>
            )}
        </div>
    )
}

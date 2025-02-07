"use client"

import React from "react"
import Link from "next/link"
import { signInWithGoogle, signOut } from "@/lib/firebase/auth"
import { useUser } from "@/lib/getUser"

type HeaderNavProps = {
    image?: string
    initialUser?: any
}

export const HeaderNav: React.FC<HeaderNavProps> = () => {
    const user = useUser()
    const handleSignOut = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault()
        signOut()
    }

    const handleSignIn = async (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault()
        signInWithGoogle()
    }
    const links = [
        { name: "Home", href: "/" },
        { name: "Lend", href: "/lend" },
    ]

    return (
        <div className="navbar bg-base-100 shadow-sm">
            <div className="navbar-start"></div>
            <div className="navbar-center hidden lg:flex">
                <ul className="menu menu-horizontal px-1">
                    {links.map((link) => (
                        <li key={link.href}>
                            <Link href={link.href}>{link.name}</Link>
                        </li>
                    ))}
                </ul>
            </div>
            <div className="navbar-end">
                {user ? (
                    <div className="dropdown dropdown-end">
                        <div
                            tabIndex={0}
                            role="button"
                            className="btn btn-ghost btn-circle avatar"
                        >
                            <div className="w-10 rounded-full">
                                <img
                                    alt="User Avatar"
                                    src={user.photoURL || "/default-avatar.png"}
                                />
                            </div>
                        </div>
                        <ul
                            tabIndex={0}
                            className="menu menu-sm dropdown-content bg-base-100 rounded-box z-1 mt-3 w-52 p-2 shadow"
                        >
                            <li>
                                <Link
                                    href="/profile"
                                    className="justify-between"
                                >
                                    Profile
                                </Link>
                            </li>
                            <li>
                                <Link href="/settings">Settings</Link>
                            </li>
                            <li>
                                <button onClick={handleSignOut}>Logout</button>
                            </li>
                        </ul>
                    </div>
                ) : (
                    <button onClick={handleSignIn} className="btn btn-ghost">
                        Sign In
                    </button>
                )}
            </div>
        </div>
    )
}

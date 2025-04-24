"use client"

import { HomeIcon } from "@heroicons/react/24/outline"
import { WrenchIcon } from "@heroicons/react/24/outline"
import { usePathname } from "next/navigation"

export const Dock: React.FC = () => {
    const links = [
        { name: "Home", href: "/" },
        { name: "Admin", href: "/admin" },
    ]

    const currentPath = usePathname()

    return (
        <div className="dock z-50 lg:hidden">
            {links.map((link) => (
                <div
                    className={`${currentPath === link.href ? "dock-active" : ""} pb-1`}
                    key={link.name}
                >
                    <a href={link.href} className={`dock-label`}>
                        {link.name === "Home" && (
                            <HomeIcon className="dock-icon" />
                        )}
                        {link.name === "Admin" && (
                            <WrenchIcon className="dock-icon" />
                        )}
                        {link.name}
                    </a>
                </div>
            ))}
        </div>
    )
}

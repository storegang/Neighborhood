"use client"

import Image from "next/image"
import { HomeIcon } from "@heroicons/react/24/outline"
import { WrenchIcon } from "@heroicons/react/24/outline"
import { usePathname } from "next/navigation"

export const Dock: React.FC = () => {
    const links = [
        { name: "Home", href: "/" },
        { name: "Lend", href: "/lend" },
    ]

    const currentPath = usePathname()

    return (
        <div className="dock mt-8 lg:hidden">
            {links.map((link) => (
                <button
                    className={`${currentPath === link.href ? "dock-active" : ""}`}
                    key={link.name}
                >
                    <a href={link.href} className="dock-label">
                        {link.name === "Home" && (
                            <HomeIcon className="dock-icon" />
                        )}
                        {link.name === "Lend" && (
                            <WrenchIcon className="dock-icon" />
                        )}
                        {link.name}
                    </a>
                </button>
            ))}
        </div>
    )
}

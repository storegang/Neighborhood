import Image from "next/image"
import { HomeIcon } from "@heroicons/react/24/outline"
import { WrenchIcon } from "@heroicons/react/24/outline"

export const Dock: React.FC = () => {
    const links = [
        { name: "Home", href: "/" },
        { name: "Lend", href: "/lend" },
    ]

    return (
        <div className="dock lg:hidden">
            {links.map((link) => (
                <button key={link.name}>
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

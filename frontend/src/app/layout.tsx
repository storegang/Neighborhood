import { Geist, Geist_Mono } from "next/font/google"
import "@/app/globals.css"
import { ReactNode } from "react"
import { Dock, Footer, HeaderNav } from "@/components"
import { getAuthenticatedAppForUser } from "@/lib/firebase/serverApp"
import AuthGuard from "@/components/AuthGuard"
import { QueryProvider } from "@/providers/QueryProvider"

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
})

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
})

export default async function RootLayout({
    children,
}: Readonly<{
    children: ReactNode
}>) {
    const { currentUser } = await getAuthenticatedAppForUser()

    return (
        <html suppressHydrationWarning>
            <body
                className={`${geistSans.variable} ${geistMono.variable} mb-16 flex min-h-screen flex-col antialiased sm:mb-0`}
            >
                <QueryProvider>
                    <header>
                        <HeaderNav initialUser={currentUser?.toJSON()} />
                    </header>
                    <AuthGuard>{children}</AuthGuard>
                    <footer className="mt-auto">
                        <Dock />
                        <Footer />
                    </footer>
                </QueryProvider>
            </body>
        </html>
    )
}

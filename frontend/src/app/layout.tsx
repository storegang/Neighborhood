"use client"

import { Geist, Geist_Mono } from "next/font/google"
import "./globals.css"
import { ReactNode } from "react"
import { HeaderNav, Footer, Dock } from "@/app/components"

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
})

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
})

export default function Layout({
    children,
}: Readonly<{
    children: ReactNode
}>) {
    return (
        <html suppressHydrationWarning>
            <body
                className={`${geistSans.variable} ${geistMono.variable} antialiased flex flex-col min-h-screen`}
            >
                <header>
                    <HeaderNav />
                </header>
                {children}
                <footer className="mt-auto">
                    <Dock />
                    <Footer />
                </footer>
            </body>
        </html>
    )
}

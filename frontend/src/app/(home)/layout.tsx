"use client"

import { Geist, Geist_Mono } from "next/font/google"
import "@/app/globals.css"
import { ReactNode } from "react"
import { Dock, Footer, HeaderNav } from "@/components"
import { UserProvider } from "@auth0/nextjs-auth0"
import React from "react"

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
            <UserProvider>
                <body
                    className={`${geistSans.variable} ${geistMono.variable} flex min-h-screen flex-col antialiased`}
                >
                    <header>
                        <HeaderNav />
                    </header>
                    {children}
                    <footer>
                        <Dock />
                        <Footer />
                    </footer>
                </body>
            </UserProvider>
        </html>
    )
}

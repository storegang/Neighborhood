import { Geist, Geist_Mono } from "next/font/google"
import "./globals.css"
import { ReactNode } from "react"

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
        <html>
            <body
                className={`${geistSans.variable} ${geistMono.variable} antialiased`}
            >
                {children}
            </body>
        </html>
    )
}
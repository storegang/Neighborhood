"use client"
import { ReactNode, useEffect, useState } from "react"
import { onAuthStateChanged, signInWithGoogle } from "@/lib/firebase/auth"

export default function AuthGuard({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<any>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const unsubscribe = onAuthStateChanged((authUser: any) => {
            setUser(authUser)
            setLoading(false)
        })

        return () => unsubscribe()
    }, [])

    if (loading) {
        return (
            <main className="flex flex-1 items-center justify-center">
                <span className="loading loading-spinner loading-sm">
                    Loading...
                </span>
            </main>
        )
    }

    if (!user) {
        return (
            <main className="flex flex-1 items-center justify-center p-4 text-center">
                <div className="max-w-md space-y-4">
                    <h1 className="text-2xl font-bold">
                        Welcome to the Neighborhood app!
                    </h1>
                    <p className="text-gray-600">
                        Please log in to access your neighborhood.
                    </p>
                    <button
                        className="btn btn-primary"
                        onClick={signInWithGoogle}
                    >
                        Log in
                    </button>
                </div>
            </main>
        )
    }

    return <>{children}</>
}

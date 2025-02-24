"use client"

import { onAuthStateChanged } from "firebase/auth"
import { useEffect, useState } from "react"

import { auth } from "@/lib/firebase/clientApp.js"
import { useRouter } from "next/navigation"
import { User as FirebaseUser } from "firebase/auth"

type NeighborhoodUser = FirebaseUser & {
    accessToken: string
}

export const useUser = () => {
    const [user, setUser] = useState<NeighborhoodUser | null>(null)

    const router = useRouter()

    useEffect(() => {
        const unsubscribe = onAuthStateChanged(auth, (authUser) => {
            setUser(authUser as NeighborhoodUser)
        })

        return () => unsubscribe()
    }, [])

    /* useEffect(() => {
        if (!user?.accessToken) {
            router.push("/")
        }
    }, [user]) */

    return user
}

"use client"

import { onAuthStateChanged } from "firebase/auth"
import { useEffect, useState } from "react"

import { auth } from "@/lib/firebase/clientApp.js"
import { User } from "@/Models/User"
import { useGetUser } from "@/app/(home)/queries"

/**
 * Custom hook to get the user from firebase auth
 *
 * @returns The user object
 */
export const useUser = (): User | undefined => {
    const [firebaseUser, setFirebaseUser] = useState<User | null>(null)

    useEffect(() => {
        const unsubscribe = onAuthStateChanged(auth, (authUser) => {
            setFirebaseUser(authUser as User)
        })

        return () => unsubscribe()
    }, [])

    const { data: serverUser } = useGetUser(firebaseUser)

    if (!serverUser || !firebaseUser) return

    return {
        ...firebaseUser,
        roles: serverUser?.roles ?? [],
        neighborhoodId: serverUser?.neighborhoodId,
    }
}

"use client"

import { onAuthStateChanged } from "firebase/auth"
import { useEffect, useState } from "react"

import { auth } from "@/lib/firebase/clientApp.js"

export const useUser = () => {
    const [user, setUser] = useState<any>(null)

    useEffect(() => {
        const unsubscribe = onAuthStateChanged(auth, (authUser) => {
            setUser(authUser)
        })

        return () => unsubscribe()
    }, [])

    return user
}

"use server"

import "server-only"

import { headers } from "next/headers"
import { initializeServerApp } from "firebase/app"

import { firebaseConfig } from "./config"
import { getAuth } from "firebase/auth"

export async function getAuthenticatedAppForUser() {
    const requestHeaders = await headers()
    const idToken = requestHeaders.get("Authorization")?.split("Bearer ")[1]
    console.log("firebaseConfig", JSON.stringify(firebaseConfig))
    const firebaseServerApp = initializeServerApp(
        firebaseConfig,
        idToken
            ? {
                  authIdToken: idToken,
              }
            : {}
    )

    const auth = getAuth(firebaseServerApp)
    await auth.authStateReady()

    console.log("serverApp", auth.currentUser)

    return { firebaseServerApp, currentUser: auth.currentUser }
}

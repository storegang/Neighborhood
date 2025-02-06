import { Feed } from "./components"
import { useEffect, useState } from "react"
import { fetchPosts } from "./components/fetchTest"
import { auth0 } from "@/lib/auth0"
import { useUser } from "@auth0/nextjs-auth0"

export default async function Home() {
    const session = await auth0.getSession()
    /*     useEffect(() => {
        fetchPosts()
    }, []) */

    /*     if (isLoading)
        return (
            <main className="container m-auto">
                <div className="flex justify-center">
                    <span className="loading loading-dots loading-lg"></span>
                </div>
            </main>
        )

    if (error) return <div>{error.message}</div>
 */
    return (
        <main className="container m-auto">
            {!session && (
                <div className="flex flex-col items-center justify-center">
                    <b>Welcome to the Neighborhood app!</b>
                    <br />
                    <div>
                        Please{" "}
                        <a
                            className="link"
                            href="api/auth/login?audience=urn:neighborhood.storegang.com"
                        >
                            log in
                        </a>{" "}
                        to access your neighborhood.
                    </div>
                </div>
            )}
            {session && (
                <div>
                    <Feed />
                </div>
            )}
        </main>
    )
}

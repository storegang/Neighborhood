"use server"

import { auth0 } from "@/lib/auth0"

export const fetchPosts = async () => {
    const baseUrl = "http://localhost:7059/api"

    try {
        const accessToken = await auth0.getAccessToken()
        console.log("accessToken", accessToken)

        const response = await fetch(
            `${baseUrl}/Category/5a2b5a60-4712-486b-ab38-5fc3988e4e59`,
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
            }
        )

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`)
        }

        const data = await response.json()
        return data
    } catch (error) {
        console.error("Error fetching posts:", error)
        return null // Eller en tom liste [] hvis du forventer et array
    }
}

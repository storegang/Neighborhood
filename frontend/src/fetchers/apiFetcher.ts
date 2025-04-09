"use server"

const url = process.env.API_URL

if (!url) {
    throw new Error("API_URL is not set")
}
/**
 * Fetches data from the server based on the input
 *
 * @param path The path to be used for the request
 * @param method The method to be used for the request
 * @param accessToken The accesToken to be used for the request
 * @param body The body to be used for the request
 * @returns The response from the server
 */
export const apiFetcher = async <T>({
    path,
    method,
    accessToken,
    body,
}: {
    path: string
    method?: string
    accessToken?: string
    body?: Record<string, unknown>
}): Promise<T> => {
    try {
        const response = await fetch(`${url}${path}`, {
            method,
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
            body: JSON.stringify(body),
        })
        if (!response.ok) {
            throw new Error(`Error fetching data: ${response.statusText}`)
        }
        if (response.status === 204) {
            return {} as T // Return an empty object for 204 No Content
        }
        return await response.json()
    } catch (error) {
        console.error("Error fetching data:", error)
        throw error
    }
}

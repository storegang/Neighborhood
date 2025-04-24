"use server"

const url = process.env.API_URL || "http://localhost:5233/api"

if (!process.env.API_URL) {
    console.warn(
        "⚠️ Warning: API_URL is not set, using fallback http://localhost:5233/api"
    )
}

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
    query,
}: {
    path: string
    method?: string
    accessToken?: string
    body?: Record<string, unknown>
    query?: Record<string, string>
}): Promise<T> => {
    const queryPath = query
        ? "/" +
          Object.entries(query)
              .map(([key, value]) => `${key}=${value}`)
              .join("/")
        : ""

    const fullUrl = `${url}${path}${queryPath}`

    console.log(`Fetching: ${fullUrl}`)
    try {
        const response = await fetch(fullUrl, {
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

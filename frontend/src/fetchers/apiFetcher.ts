"use server"

const url = process.env.API_URL

if (!url) {
    throw new Error("API_URL is not set")
}

export const apiFetcher = async <T>({
    path,
    params,
    method = "GET",
    accessToken,
}: {
    path: string
    method?: string
    params?: Record<string, unknown>
    accessToken?: string
}): Promise<T> => {
    try {
        const response = await fetch(`${url}${path}`, {
            method,
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
            body: JSON.stringify(params),
        })
        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`)
        }
        return await response.json()
    } catch (error) {
        console.error("Error fetching data:", error)
        throw error
    }
}

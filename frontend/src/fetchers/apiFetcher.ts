"use server"

const url = process.env.API_URL

if (!url) {
    throw new Error("API_URL is not set")
}

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
        return await response.json()
    } catch (error) {
        console.error("Error fetching data:", error)
        throw error
    }
}

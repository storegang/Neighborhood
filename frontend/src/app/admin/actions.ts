import { apiFetcher } from "@/fetchers/apiFetcher"
import { User } from "@/Models"

export const gettUsers = async (accessToken: string): Promise<User[]> => {
    const response = await apiFetcher<{ users: User[] }>({
        path: "/user",
        accessToken: accessToken,
    })
    return response.users
}

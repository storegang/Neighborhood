import { apiFetcher } from "@/fetchers/apiFetcher"
import { User } from "@/Models"

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @returns All users from the server
 */
export const gettUsers = async (accessToken: string): Promise<User[]> => {
    const response = await apiFetcher<{ users: User[] }>({
        path: "/user",
        accessToken: accessToken,
    })
    return response.users
}

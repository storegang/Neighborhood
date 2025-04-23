import { User } from "@/Models"
import { useQuery } from "@tanstack/react-query"
import { getMeetings, getUsers } from "./actions"

/**
 *
 * @param user The user to be used for the request
 * @returns Calls the gettUsers function
 */
export const useGetUsers = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["users", accessToken],
        queryFn: () => getUsers(accessToken!),
        enabled: !!accessToken,
    })
}

/**
 *
 * @param user The user to be used for the request
 * @return Query for getting meetings
 */
export const useGetMeetings = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["meetings", accessToken],
        queryFn: () => getMeetings(accessToken!),
        enabled: !!accessToken,
    })
}

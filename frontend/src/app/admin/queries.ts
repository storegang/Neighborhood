import { User } from "@/Models"
import { useQuery } from "@tanstack/react-query"
import { gettUsers } from "./actions"

export const useGetUsers = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["users", accessToken],
        queryFn: () => gettUsers(accessToken!),
        enabled: !!accessToken,
    })
}

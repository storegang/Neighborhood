import { useQuery } from "@tanstack/react-query"
import { getCategories, getPosts } from "./actions"
import { User } from "@/Models/User"

export const useGetPosts = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["posts", accessToken],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!),
    })
}

export const useGetCategories = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["categories", accessToken],
        queryFn: () => getCategories(accessToken!),
        enabled: !!accessToken,
    })
}

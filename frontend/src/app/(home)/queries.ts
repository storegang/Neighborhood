import { useQuery } from "@tanstack/react-query"
import { getCategories, getPosts } from "./actions"

export const useGetPosts = (accessToken: string | undefined) => {
    const query = useQuery({
        queryKey: ["posts", accessToken],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!),
    })
    return query
}

export const useGetCategories = (accessToken: string | undefined) => {
    const query = useQuery({
        queryKey: ["categories", accessToken],
        enabled: !!accessToken,
        queryFn: () => getCategories(accessToken!),
    })
    return query
}

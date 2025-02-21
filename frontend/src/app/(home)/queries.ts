import { useQuery } from "@tanstack/react-query"
import { getPosts } from "./actions"

export const useGetPosts = (accessToken: string | undefined) => {
    const query = useQuery({
        queryKey: ["posts", accessToken],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!),
    })
    return query
}

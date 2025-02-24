import { apiFetcher } from "@/fetchers/apiFetcher"
import { Post } from "@/Models/Post"

export const getPosts = async (accessToken: string): Promise<Post[]> => {
    const response = await apiFetcher<{ posts: Post[] }>({
        path: "/post",
        accessToken: accessToken,
    })

    return response.posts
}

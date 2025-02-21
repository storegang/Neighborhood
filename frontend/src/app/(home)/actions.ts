import { apiFetcher } from "@/fetchers/apiFetcher"
import { Post } from "@/Models/Post"
import { Category } from "../../Models/Category"

export const getPosts = async (accessToken: string): Promise<Post[]> => {
    const response = await apiFetcher<{ posts: Post[] }>({
        path: "/post",
        accessToken: accessToken,
    })

    return response.posts
}

export const getCategories = async (
    accessToken: string
): Promise<Category[]> => {
    const response = await apiFetcher<{ categories: Category[] }>({
        path: "/category",
        accessToken: accessToken,
    })

    return response.categories
}

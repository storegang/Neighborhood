"use server"

import { apiFetcher } from "@/fetchers/apiFetcher"
import { Post, Category } from "@/Models"

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @param category If a category is specified, only posts from that category will be returned
 * @returns Posts from the server, either all posts or posts from a specific category
 */
export const getPosts = async (
    accessToken: string,
    category?: Category
): Promise<Post[]> => {
    const response = await apiFetcher<{ posts: Post[] }>({
        path: category ? "/post/fromcategory=" + category : "/post",
        accessToken: accessToken,
    })

    return response.posts
}

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @returns The categories from the server
 */
export const getCategories = async (
    accessToken: string
): Promise<Category[]> => {
    const response = await apiFetcher<{ categories: Category[] }>({
        path: "/category",
        accessToken: accessToken,
    })
    return response.categories
}

export type CreatePostInput = {
    title: string
    content: string
    categoryId: string
    userUID: string
    accessToken: string
}
/**
 *
 * @param input The input for the post to be created
 * @returns The created post
 */
export const createPost = async (
    input: CreatePostInput
): Promise<{
    post: Post
}> => {
    const response = apiFetcher<{ post: Post }>({
        path: "/post",
        method: "POST",
        accessToken: input.accessToken,
        body: {
            title: input.title,
            description: input.content,
            categoryId: input.categoryId,
            imageUrls: [],
            // AccessToken kan brukes til å hente brukerdata istedet
            authorUserId: input.userUID,

            // Alle disse kan flyttes til backend
            authorUser: {
                id: "",
                name: "",
                avatar: "",
                neighborhoodId: "",
            },
            id: "",
            datePosted: new Date().toISOString(),
            dateLastEdited: new Date().toISOString(),

            commentCount: 0,
            likedByUserCount: 0,
            likedByCurrentUser: false,
        },
    })

    return response
}

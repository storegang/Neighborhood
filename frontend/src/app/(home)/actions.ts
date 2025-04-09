"use server"

import { apiFetcher } from "@/fetchers/apiFetcher"
import { Post, Category, User } from "@/Models"

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
    console.log(category ? "/post/fromcategory=" + category : "/post")
    const response = await apiFetcher<{ posts: Post[] }>({
        path: category ? "/post/fromcategory=" + category : "/post",
        accessToken: accessToken,
    })

    return response.posts
}

export const getComments = async (
    accessToken: string,
    postId: string
): Promise<Comment[]> => {
    const response = await apiFetcher<{ comments: Comment[] }>({
        path: `/comment/allfrompost=${postId}`,
        accessToken: accessToken,
    })
    return response.comments
}

/**
 * Sends a request to like a post.
 *
 * @param postId - The ID of the post to like.
 * @param accessToken - The access token for authentication.
 * @returns A promise that resolves when the request is complete.
 */
export const likePost = async (
    postId: string,
    accessToken: string
): Promise<any> => {
    await apiFetcher<{ success: boolean }>({
        path: `/post/like/${postId}`,
        method: "PUT",
        accessToken: accessToken,
    })
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

/**
 *
 */
export const addComment = async (
    comment: string,
    postId: string,
    accessToken: string,
    userId: User["uid"]
) => {
    const response = await apiFetcher<{ comment: Comment }>({
        path: "/comment",
        method: "POST",
        accessToken: accessToken,

        body: {
            content: comment,
            authorUserId: userId,
            parentPostId: postId,

            /* Flyttes til backend */
            id: "string",
            datePosted: "2025-04-08T19:10:41.186Z",
            dateLastEdited: "2025-04-08T19:10:41.186Z",
            authorUser: {
                id: "string",
                name: "string",
                avatar: "string",
                neighborhoodId: "string",
                userRole: 0,
            },
            imageUrl: "string",
            likedByUserCount: 0,
            likedByCurrentUser: true,
        },
    })

    return response
}

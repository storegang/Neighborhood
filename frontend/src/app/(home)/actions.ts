"use server"

import { apiFetcher } from "@/fetchers/apiFetcher"
import {
    PostResponse,
    PostRequest,
    Category,
    User,
    LikeRequest,
    CommentRequest,
    CommentResponse,
} from "@/Models"

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @param category If a category is specified, only posts from that category will be returned
 * @returns Posts from the server, either all posts or posts from a specific category
 */
export const getPosts = async (
    accessToken: string,
    category?: Category
): Promise<PostResponse[]> => {
    const response = await apiFetcher<{ posts: PostResponse[] }>({
        path: category ? "/post/fromcategory=" + category : "/post",
        accessToken: accessToken,
    })

    return response.posts
}

export const getUser = async (
    accessToken: string,
    uid: string
): Promise<User> => {
    const response = await apiFetcher<User>({
        path: `/user/${uid}`,
        accessToken,
    })

    return response
}

/**
 * Fetches comments for a specific post.
 *
 * @param accessToken - The access token for authentication.
 * @param postId - The ID of the post to fetch comments from.
 * @returns A promise that resolves to an array of comments.
 */
export const getComments = async (
    accessToken: string,
    postId: string
): Promise<CommentResponse[]> => {
    const response = await apiFetcher<{ comments: CommentResponse[] }>({
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
    postId: LikeRequest["postId"],
    accessToken: string
): Promise<any> => {
    await apiFetcher<{ success: boolean }>({
        path: `/post/like/${postId}`,
        method: "PUT",
        accessToken: accessToken,
    })
}

/**
 * Fetches categories from the server.
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
 * Creates a post on the server.
 *
 * @param input The input for the post to be created
 * @returns The created post
 */
export const createPost = async (
    input: CreatePostInput
): Promise<{
    post: PostRequest
}> => {
    const response = apiFetcher<{ post: PostRequest }>({
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
        },
    })

    return response
}

/**
 * Adds a comment to a post.
 *
 * @param comment The comment to be added
 * @param postId The ID of the post to add the comment to
 * @param accessToken The access token for authentication
 * @param userId The ID of the user adding the comment
 * @returns The response from the server
 *
 */
export const addComment = async (
    comment: CommentRequest["content"],
    postId: PostResponse["id"],
    accessToken: User["accessToken"],
    userId: User["uid"]
): Promise<CommentResponse> => {
    const response = await apiFetcher<{ comment: CommentResponse }>({
        path: "/comment",
        method: "POST",
        accessToken: accessToken,

        body: {
            content: comment,
            authorUserId: userId,
            parentPostId: postId,

            id: "",
            datePosted: "2025-04-08T19:10:41.186Z",
            dateLastEdited: "2025-04-08T19:10:41.186Z",
            imageUrl: "",
            likedByUserCount: 0,
            likedByCurrentUser: true,
        },
    })

    return response.comment
}

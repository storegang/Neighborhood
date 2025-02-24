"use server"

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

export type CreatePostInput = {
    title: string
    content: string
    categoryId: string
    userUID: string
    accessToken: string
}

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

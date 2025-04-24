import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"

import {
    createPost,
    CreatePostInput,
    getCategories,
    getComments,
    getPosts,
    addComment,
    likePost,
    getUser,
} from "./actions"

import { Category, User } from "@/Models"
import { LikeRequest } from "@/Models/Likes"

/**
 * Gets the user from the server.
 *
 * @param user - User input, for the accessToken.
 * @returns The query calling the getUser function.
 */
export const useGetUser = (firebaseUser: User | null) => {
    const accessToken = firebaseUser?.accessToken
    const uid = firebaseUser?.uid

    return useQuery({
        queryKey: ["user", uid],
        enabled: !!accessToken && !!uid,
        queryFn: () => getUser(accessToken!, uid!),
        select: (data) => data ?? null,
    })
}

/**
 * Gets the posts from the server.
 *
 * @param user - User input, for the accessToken.
 * @param category - Category input, for filtering posts if needed.
 * @returns The query calling the getPosts function.
 */
export const useGetPosts = (user: User | null, category: Category | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["posts", accessToken, category?.id],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!, category),
    })
}

// TODO: Add specific neighborhood to the query
/**
 * Gets the categories, in the specific neighborhood, from the server.
 *
 * @param user - User input, for the accessToken.
 * @returns The query calling the getCategories function.
 */
export const useGetCategories = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["categories", accessToken],
        queryFn: () => getCategories(accessToken!),
        enabled: !!accessToken,
    })
}

/**
 * Creates a post on the server.
 *
 * @returns The mutation calling the createPost function.
 */
export const useCreatePost = () => {
    return useMutation({
        mutationFn: (input: CreatePostInput) => {
            return createPost(input)
        },
    })
}

/**
 * Custom hook to like a post.
 *
 * @param {User | null} user - The user object containing the access token. If null, the user is not authenticated.
 * @returns {UseMutationResult} The mutation result object from the `useMutation` hook.
 */
export const useLikePost = (user: User | null) => {
    const accessToken = user?.accessToken
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (postId: LikeRequest["postId"]) =>
            likePost(postId, accessToken!),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["posts"] })
        },
    })
}

export const useGetComments = (user: User | null, postId: string) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["comments", postId, accessToken],
        enabled: !!accessToken,
        queryFn: () => getComments(accessToken!, postId),
    })
}

export const useAddComment = (user: User | null, postId: string) => {
    const accessToken = user?.accessToken
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (comment: string) =>
            addComment(comment, postId, accessToken!, user?.uid!),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["comments", postId] })
        },
        onError: (error) => {
            console.error("Error adding comment:", error)
        },
    })
}

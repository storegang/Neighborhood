import { useMutation, useQuery } from "@tanstack/react-query"
import {
    createPost,
    CreatePostInput,
    getCategories,
    getPosts,
    likePost,
} from "./actions"
import { Category, User } from "@/Models"

/**
 * Gets the posts from the server.
 *
 * @param user - User input, for the accessToken.
 * @param category - Category input, for filtering posts if needed.
 * @returns The query calling the getPosts function.
 */
export const useGetPosts = (user: User | null, category?: Category) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["posts", accessToken, category],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!),
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
    return useMutation({
        mutationFn: (postId: string) => {
            return likePost(postId, accessToken!)
        },
    })
}

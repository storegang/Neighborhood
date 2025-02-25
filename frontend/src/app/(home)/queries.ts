import { useMutation, useQuery } from "@tanstack/react-query"
import { createPost, CreatePostInput, getCategories, getPosts } from "./actions"
import { User } from "@/Models"

export const useGetPosts = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["posts", accessToken],
        enabled: !!accessToken,
        queryFn: () => getPosts(accessToken!),
    })
}

export const useGetCategories = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["categories", accessToken],
        queryFn: () => getCategories(accessToken!),
        enabled: !!accessToken,
    })
}

export const useCreatePost = () => {
    return useMutation({
        mutationFn: (input: CreatePostInput) => {
            return createPost(input)
        },
    })
}

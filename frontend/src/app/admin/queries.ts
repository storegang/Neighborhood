import { Meeting, User } from "@/Models"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import {
    createCategory,
    createMeeting,
    deleteCategory,
    getMeetings,
    getUsers,
} from "./actions"
import { CategoryRequest } from "../../Models/Category"

/**
 *
 * @param user The user to be used for the request
 * @returns Calls the gettUsers function
 */
export const useGetUsers = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["users", accessToken],
        queryFn: () => getUsers(accessToken!),
        enabled: !!accessToken,
    })
}

/**
 *
 * @param user The user to be used for the request
 * @return Query for getting meetings
 */
export const useGetMeetings = (user: User | null) => {
    const accessToken = user?.accessToken
    return useQuery({
        queryKey: ["meetings", accessToken],
        queryFn: () => getMeetings(accessToken!),
        enabled: !!accessToken,
    })
}

/**
 *
 * @param user The user to be used for the request
 * @returns Mutation for creating a meeting
 */
export const useCreateMeeting = (user: User | null) => {
    const accessToken = user?.accessToken
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (meeting: Omit<Meeting, "id">) =>
            createMeeting(accessToken!, meeting),

        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ["meetings", accessToken],
            })
        },
        onError: (error) => {
            console.error("Error adding meeting:", error)
        },
    })
}

export const useCreateCategory = () => {
    return useMutation({
        mutationFn: (input: CategoryRequest) => {
            return createCategory(input)
        },
    })
}

export const useDeleteCategories = (user: User | null) => {
    const accessToken = user?.accessToken
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: (categoryId: string) =>
            deleteCategory(categoryId, accessToken!),
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ["categories", accessToken],
            })
        },
    })
}

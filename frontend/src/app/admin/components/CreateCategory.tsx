"use client"

import { useUser } from "@/lib/getUser"
import { useCreateCategory } from "../queries"
import { useQueryClient } from "@tanstack/react-query"
import { useGetCategories } from "@/app/(home)/queries"

export const CreateCategory: React.FC = () => {
    const user = useUser()

    const queryClient = useQueryClient()

    const { mutate: createCategory, isPending, isError } = useCreateCategory()
    const { data: categories } = useGetCategories(user ?? null)

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        createCategory(
            {
                name: e.currentTarget.categoryName.value as string,
                accessToken: user?.accessToken,
                neighborhoodId: user?.neighborhoodId,
            },
            {
                onSuccess: () => {
                    queryClient.invalidateQueries({
                        queryKey: ["categories", user?.accessToken],
                    })
                },
                onError: (error) => {
                    console.error("Error creating post:", error)
                },
            }
        )
    }

    return (
        <div className="flex flex-col gap-4">
            <h2 className="text-lg font-bold">Create Category</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="categoryName"
                    placeholder="Category Name"
                    required
                />
                <button type="submit" disabled={isPending}>
                    {isPending ? "Creating..." : "Create Category"}
                </button>
            </form>
        </div>
    )
}

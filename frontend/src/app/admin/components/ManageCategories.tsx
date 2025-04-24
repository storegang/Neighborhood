"use client"

import { useUser } from "@/lib/getUser"
import { useCreateCategory } from "../queries"
import { useQueryClient } from "@tanstack/react-query"
import { useGetCategories } from "@/app/(home)/queries"
import { useState } from "react"

export const ManageCategories: React.FC = () => {
    const user = useUser()

    const queryClient = useQueryClient()

    const { mutate: createCategory, isPending, isError } = useCreateCategory()
    const { data: categories } = useGetCategories(user ?? null)

    const [categoryName, setCategoryName] = useState("")

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        createCategory(
            {
                name: categoryName,
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
        <form
            onSubmit={handleSubmit}
            className="card card-border border-base-300"
        >
            <div className="card-body">
                <h2 className="card-title">Manage categories</h2>
                <input
                    type="text"
                    placeholder="Type here"
                    className="input"
                    onChange={(e) => setCategoryName(e.target.value)}
                />
                <div className="card-actions justify-end">
                    <button type="submit" className="btn btn-secondary">
                        Create
                    </button>
                </div>
            </div>
        </form>
    )
}

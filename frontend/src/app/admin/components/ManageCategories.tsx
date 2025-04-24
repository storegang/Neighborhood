"use client"

import { useUser } from "@/lib/getUser"
import { useCreateCategory, useDeleteCategory } from "../queries"
import { useQueryClient } from "@tanstack/react-query"
import { useGetCategories } from "@/app/(home)/queries"
import { useState } from "react"

export const ManageCategories: React.FC = () => {
    const user = useUser()

    const queryClient = useQueryClient()

    const { mutate: createCategory, isPending, isError } = useCreateCategory()
    const { data: categories } = useGetCategories(user ?? null)
    const { mutate: deleteCategory } = useDeleteCategory(user ?? null)

    const [selectedCategories, setSelectedCategories] = useState<string[]>([])

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
                onSettled: () => {
                    setCategoryName("")
                },
                onError: (error) => {
                    console.error("Error creating category:", error)
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
                <ul className="mt-2 flex flex-wrap gap-2 space-y-1 overflow-auto">
                    {categories ? (
                        categories.map((category) => {
                            return (
                                <li
                                    key={category.id}
                                    className={`badge h-fit w-fit cursor-pointer ${
                                        selectedCategories?.includes(
                                            category.id
                                        )
                                            ? "badge-accent"
                                            : "badge-neutral"
                                    }`}
                                    onClick={() =>
                                        setSelectedCategories(
                                            (prev: string[]) =>
                                                prev?.includes(category.id)
                                                    ? prev.filter(
                                                          (id) =>
                                                              id !== category.id
                                                      )
                                                    : [
                                                          ...(prev || []),
                                                          category.id,
                                                      ]
                                        )
                                    }
                                >
                                    {category.name}
                                </li>
                            )
                        })
                    ) : (
                        <p className="text-sm text-gray-500">
                            No categories available
                        </p>
                    )}
                </ul>
                <input
                    type="text"
                    placeholder="Type here"
                    className="input validator"
                    onChange={(e) => setCategoryName(e.target.value)}
                    required
                />
                <div className="card-actions justify-end">
                    <button type="submit" className="btn btn-secondary">
                        Create
                    </button>
                    {!!selectedCategories.length && (
                        <button
                            type="button"
                            className="btn btn-error"
                            onClick={() => {
                                selectedCategories.forEach((categoryId) => {
                                    deleteCategory(categoryId)
                                })
                                setSelectedCategories([])
                            }}
                        >
                            Delete
                        </button>
                    )}
                </div>
            </div>
        </form>
    )
}

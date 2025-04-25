"use client"

import { useUser } from "@/lib/getUser"
import { useCreateCategory, useDeleteCategory } from "../queries"
import { useQueryClient } from "@tanstack/react-query"
import { useGetCategories } from "@/app/(home)/queries"
import { useState } from "react"
import { ListItemSkeleton } from "./ListItemSkeleton"

export const ManageCategories: React.FC = () => {
    const user = useUser()

    const queryClient = useQueryClient()

    const { mutate: createCategory, isPending: isCreatingCategory } =
        useCreateCategory()
    const {
        data: categories,
        isLoading,
        isError,
    } = useGetCategories(user ?? null)
    const { mutate: deleteCategory, isPending: isDeletingCategory } =
        useDeleteCategory(user ?? null)

    const [selectedCategories, setSelectedCategories] = useState<string[]>([])
    const [categoryError, setCategoryError] = useState<string | null>(null)
    const [categoryName, setCategoryName] = useState("")

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setCategoryError(null)

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
                    setCategoryName("")
                },

                onError: (error) => {
                    console.error("Error creating category:", error)

                    setCategoryError(
                        "An error occurred while creating the category."
                    )
                },
            }
        )
    }

    if (isError) {
        return (
            <div className="card card-border border-base-300">
                <div className="card-body">
                    <h2 className="card-title">Manage categories</h2>
                    <div className="alert alert-error w-fit shadow-lg">
                        Could not load categories.
                    </div>
                </div>
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="card card-border border-base-300">
                <div className="card-body">
                    <h2 className="card-title">Manage categories</h2>
                    <ul className="list">
                        <ListItemSkeleton />
                        <ListItemSkeleton />
                        <ListItemSkeleton />
                        <ListItemSkeleton />
                        <ListItemSkeleton />
                    </ul>
                </div>
            </div>
        )
    }

    if (!categories?.length) {
        return (
            <div className="card card-border border-base-300">
                <div className="card-body">
                    <h2 className="card-title">Manage categories</h2>
                    <div className="alert alert-info shadow-lg">
                        No categories found
                    </div>
                </div>
            </div>
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
                    {categories.map((category) => {
                        return (
                            <li
                                key={category.id}
                                className={`badge h-fit cursor-pointer ${
                                    selectedCategories?.includes(category.id)
                                        ? "badge-neutral"
                                        : "badge-soft"
                                }`}
                                onClick={() =>
                                    setSelectedCategories((prev: string[]) =>
                                        prev?.includes(category.id)
                                            ? prev.filter(
                                                  (id) => id !== category.id
                                              )
                                            : [...(prev || []), category.id]
                                    )
                                }
                            >
                                {category.name}
                            </li>
                        )
                    })}
                </ul>

                <div className="card-actions justify-start">
                    <input
                        type="text"
                        placeholder="Type here"
                        className="input validator"
                        onChange={(e) => setCategoryName(e.target.value)}
                        required
                        value={categoryName}
                    />
                    <button
                        type="submit"
                        className="btn btn-secondary"
                        disabled={isCreatingCategory}
                    >
                        {isCreatingCategory ? (
                            <span className="loading loading-spinner">
                                Creating
                            </span>
                        ) : (
                            "Create"
                        )}
                    </button>
                    {!!selectedCategories.length && (
                        <button
                            type="button"
                            className="btn btn-error"
                            onClick={() => {
                                setCategoryError(null)
                                selectedCategories.forEach((categoryId) => {
                                    deleteCategory(categoryId, {
                                        onSuccess: () => {
                                            setSelectedCategories([])
                                        },
                                        onError: (error) => {
                                            setSelectedCategories([])
                                            console.error(
                                                "Error deleting category:",
                                                error
                                            )
                                            setCategoryError(
                                                "An error occurred while deleting the category."
                                            )
                                        },
                                    })
                                })
                            }}
                            disabled={isDeletingCategory}
                        >
                            {isDeletingCategory ? (
                                <span className="loading loading-spinner">
                                    Deleting
                                </span>
                            ) : (
                                "Delete"
                            )}
                        </button>
                    )}
                </div>
                {categoryError && (
                    <div className="alert alert-error w-fit shadow-lg">
                        {categoryError}
                    </div>
                )}
            </div>
        </form>
    )
}

"use client"

import { User, PostRequest, Category } from "@/Models"
import { useState } from "react"
import { useCreatePost, useGetCategories } from "../queries"
import { useQueryClient } from "@tanstack/react-query"
import { Alert } from "@/components"

type CreatePostProps = {
    user: User
    onPostCreated?: (newPost: PostRequest) => void
}

export const CreatePost: React.FC<CreatePostProps> = ({ user }) => {
    const [selectedCategory, setSelectedCategory] = useState<Category["id"]>("")
    const [title, setTitle] = useState("")
    const [content, setContent] = useState("")
    const [categoryWarning, setCategoryWarning] = useState(false)
    const [displayError, setDisplayError] = useState(false)

    const { data: categories } = useGetCategories(user)

    const { isPending, isError, mutate: createPost } = useCreatePost()

    const queryClient = useQueryClient()

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        if (!selectedCategory) {
            setCategoryWarning(true)
            return
        }

        isError && setDisplayError(true)

        const selectedCategoryObj = categories?.find(
            (category) => category.id === selectedCategory
        )

        createPost(
            {
                title,
                content,
                categoryId: selectedCategoryObj?.id || "",
                userUID: user.uid,
                accessToken: user.accessToken,
            },
            {
                onSuccess: () => {
                    setTitle("")
                    setContent("")
                    setSelectedCategory("")
                    queryClient.invalidateQueries({
                        queryKey: ["posts", user.accessToken],
                    })
                    setCategoryWarning(false)
                    setDisplayError(false)
                },
                onError: (error) => {
                    console.error("Error creating post:", error)
                },
            }
        )
    }

    const colorClasses: Record<string, string> = {
        neutral: "btn-neutral",
        primary: "btn-primary",
        secondary: "btn-secondary",
        accent: "btn-accent",
        info: "btn-info",
        success: "btn-success",
        warning: "btn-warning",
        error: "btn-error",
    }
    return (
        <section className="border-base-300 collapse-arrow collapse border">
            <input type="checkbox" />
            <div className="border-base-300 collapse-title border-b border-dashed">
                <p className="flex items-center gap-2 text-sm font-medium">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth="1.5"
                        stroke="currentColor"
                        className="size-5 opacity-40"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10"
                        ></path>
                    </svg>
                    Write a new post
                </p>
            </div>
            <div className="collapse-content flex flex-col gap-4 text-sm"></div>
            <form
                onSubmit={handleSubmit}
                className="collapse-content flex flex-col gap-4"
            >
                <div className="flex items-center justify-between pt-4">
                    <input
                        type="text"
                        placeholder="Title"
                        className="input input-md"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                    <button className="btn btn-xs">Add files</button>
                </div>
                <textarea
                    className="textarea textarea-border w-full resize-none"
                    placeholder="What's happening?"
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                ></textarea>
                <div className="flex flex-wrap gap-4">
                    {categories?.map((category) => (
                        <button
                            type="button"
                            className={`btn btn-xs ${
                                selectedCategory === category.id
                                    ? "btn-active"
                                    : "btn-soft"
                            } ${colorClasses[category.color.toLowerCase()] || "btn-neutral"}`}
                            onClick={() => setSelectedCategory(category.id)}
                            key={category.id}
                        >
                            {category.name}
                        </button>
                    ))}
                </div>
                <div className="flex justify-end gap-4">
                    <button
                        className="btn btn-sm btn-primary"
                        type="submit"
                        disabled={isPending}
                    >
                        {isPending ? (
                            <>
                                <span className="loading loading-circular"></span>
                                <span>loading</span>
                            </>
                        ) : (
                            "Publish"
                        )}
                    </button>
                    {categoryWarning ? (
                        <Alert
                            type="warning"
                            message="You need to set a category"
                        />
                    ) : displayError ? (
                        <Alert type="error" message="Error creating post" />
                    ) : null}
                </div>
            </form>
        </section>
    )
}

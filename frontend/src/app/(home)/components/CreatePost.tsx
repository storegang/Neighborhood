"use client"

import { Category } from "@/Models/Category"
import { User } from "@/Models/User"
import { useState } from "react"
import { Post } from "@/Models/Post"
import { useCreatePost, useGetCategories } from "../queries"
import { useQueryClient } from "@tanstack/react-query"

type CreatePostProps = {
    user: User
    onPostCreated?: (newPost: Post) => void
}

export const CreatePost: React.FC<CreatePostProps> = ({ user }) => {
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(
        null
    )
    const [title, setTitle] = useState("")
    const [content, setContent] = useState("")

    const handleCategorySelect = (category: Category) => {
        setSelectedCategory((prev) =>
            prev?.id === category.id ? null : category
        )
    }

    const { data: categories } = useGetCategories(user)

    const {
        isPending,
        isError,
        isSuccess,
        mutate: createPost,
    } = useCreatePost()

    const queryClient = useQueryClient()

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        if (!selectedCategory) {
            // TODO: Legg på en feilmelding til brukeren
            return
        }

        createPost(
            {
                title,
                content,
                categoryId: selectedCategory.id,
                userUID: user.uid,
                accessToken: user.accessToken,
            },
            {
                onSuccess: () => {
                    setTitle("")
                    setContent("")
                    setSelectedCategory(null)
                    queryClient.invalidateQueries({
                        queryKey: ["posts", user.accessToken],
                    })
                },
                onError: (error) => {
                    console.error("Error creating post:", error)
                },
            }
        )
    }

    return (
        <section className="card card-border border-base-300 card-sm overflow-hidden">
            <div className="border-base-300 border-b border-dashed">
                <div className="flex items-center gap-2 p-4">
                    <div className="grow">
                        <div className="flex items-center gap-2 text-sm font-medium">
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
                        </div>
                    </div>
                </div>
            </div>
            <form onSubmit={handleSubmit}>
                <div className="card-body gap-4">
                    <div className="flex items-center justify-between">
                        <input
                            type="text"
                            placeholder="Title"
                            className="input input-md"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
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
                                className={`btn btn-xs btn-${category.color} ${
                                    selectedCategory?.id === category.id
                                        ? "btn-active"
                                        : "btn-soft"
                                }`}
                                onClick={() => handleCategorySelect(category)}
                                key={category.id}
                            >
                                {category.name}
                            </button>
                        ))}
                    </div>
                    <div className="card-actions justify-end">
                        <button
                            className="btn btn-sm btn-primary"
                            type="submit"
                            disabled={isPending}
                        >
                            {isPending ? (
                                <>
                                    <span className="loading loading-circular"></span>
                                    <span>loading...</span>
                                </>
                            ) : (
                                "Publish"
                            )}
                        </button>
                    </div>
                </div>
            </form>
        </section>
    )
}

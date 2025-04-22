"use client"

import { useState } from "react"

import { PostCard, CreatePost, CategoriesList } from "./index"
import { useUser } from "@/lib/getUser"
import { useGetCategories, useGetPosts } from "../queries"
import { Alert, CardSkeletonLoader } from "@/components"
import { Category } from "@/Models"

export const Feed: React.FC = () => {
    const user = useUser()

    const [selectedCategory, setSelectedCategory] = useState<Category | null>(
        null
    )

    const {
        data: posts = [],
        isLoading: postsLoading,
        isError: postsError,
    } = useGetPosts(user, selectedCategory)

    const { data: categories = [], isLoading: categoriesLoading } =
        useGetCategories(user)

    if (!user || postsLoading || categoriesLoading) {
        return (
            <div className="mx-auto mt-6 flex w-full flex-col gap-6 lg:w-96 xl:w-1/2">
                <CardSkeletonLoader />
                <CardSkeletonLoader />
            </div>
        )
    }

    if (postsError) {
        return (
            <div className="mx-auto mt-6 flex w-full flex-col gap-6 lg:w-96 xl:w-1/2">
                <Alert type="error" message="Error loading posts" />
            </div>
        )
    }

    return (
        <div className="mx-auto mt-6 flex flex-col gap-6 lg:w-2/3 lg:flex-row">
            <section className="flex-1">
                <CreatePost user={user} />
                <section aria-label="Post feed">
                    {posts.length > 0 ? (
                        posts.map((post) => (
                            <PostCard key={post.id} {...post} />
                        ))
                    ) : (
                        <p>No posts found</p>
                    )}
                </section>
            </section>

            <aside className="w-1/2 lg:w-1/3">
                <CategoriesList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onSelectCategory={setSelectedCategory}
                />
            </aside>
        </div>
    )
}

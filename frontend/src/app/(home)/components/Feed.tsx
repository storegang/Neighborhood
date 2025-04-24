"use client"

import { useState } from "react"

import { PostCard, CreatePost, CategoriesList } from "./index"
import { useUser } from "@/lib/getUser"
import { useGetCategories, useGetPosts } from "../queries"
import { Alert, CardSkeletonLoader } from "@/components"
import { CategoryResponse } from "@/Models"
import { MobileCategoriesList } from "./CategoriesList"

export const Feed: React.FC = () => {
    const user = useUser()

    const [selectedCategory, setSelectedCategory] =
        useState<CategoryResponse | null>(null)

    const {
        data: posts = [],
        isLoading: postsLoading,
        isError: postsError,
    } = useGetPosts(user ?? null, selectedCategory)

    const { data: categories = [], isLoading: categoriesLoading } =
        useGetCategories(user ?? null)

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
        <div className="mx-auto mt-6 mb-16 flex flex-col gap-6 md:mb-0 lg:w-2/3 lg:flex-row">
            <section className="flex-1">
                <CreatePost user={user} />
                <MobileCategoriesList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onSelectCategory={setSelectedCategory}
                />
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

            <aside className="w-1/2 lg:w-1/4">
                <CategoriesList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onSelectCategory={setSelectedCategory}
                />
            </aside>
        </div>
    )
}

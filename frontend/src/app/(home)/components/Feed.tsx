"use client"

import { PostCard, CreatePost } from "./index"
import { useUser } from "@/lib/getUser"
import { useGetCategories, useGetPosts } from "../queries"
import { Alert, CardSkeletonLoader } from "@/components"

export const Feed: React.FC = () => {
    const user = useUser()

    const {
        data: posts = [],
        isLoading: postsLoading,
        isError: postsError,
    } = useGetPosts(user)
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
        <section className="mx-auto mt-6 flex flex-col gap-6 lg:w-2/3">
            <CreatePost user={user} />
            <section>
                {posts.length > 0 ? (
                    posts.map((post) => <PostCard key={post.id} {...post} />)
                ) : (
                    <p>No posts found</p>
                )}
            </section>
        </section>
    )
}

"use client"

import { PostCard } from "./index"
import { useUser } from "@/lib/getUser"
import { useGetPosts } from "../queries"

export const Feed: React.FC = () => {
    const user = useUser()

    const { data: posts, isLoading, isError } = useGetPosts(user?.accessToken)

    if (isLoading) {
        return <p>Loading...</p>
    }

    if (isError) {
        return <p>Error</p>
    }

    if (!posts?.length) {
        return <p>No posts found</p>
    }

    return (
        <div className="mx-auto w-full lg:w-96 xl:w-1/2">
            {posts.map((post) => (
                <PostCard
                    key={post.id}
                    id={post.id}
                    title={post.title}
                    description={post.description}
                    datePosted={post.datePosted}
                    dateLastEdited={post.dateLastEdited}
                    authorUserId={post.authorUserId}
                    categoryId={post.categoryId}
                    imageUrls={post.imageUrls}
                    authorUser={post.authorUser}
                    commentCount={post.commentCount}
                    likedByUserCount={post.likedByUserCount}
                    likedByCurrentUser={post.likedByCurrentUser}
                />
            ))}
        </div>
    )
}

export default Feed

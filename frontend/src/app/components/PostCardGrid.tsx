import { PostCard } from "./index"

export const PostCardGrid: React.FC = () => {
    const posts = [
        {
            id: 1,
            title: "Post Title 1",
            description: "Post Description 1",
        },
        {
            id: 2,
            title: "Post Title 2",
            description: "Post Description 2",
        },
        {
            id: 3,
            title: "Post Title 3",
            description: "Post Description 3",
        },
    ]

    return (
        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
            {posts.map((post) => (
                <PostCard title={post.title} description={post.description} />
            ))}
        </div>
    )
}

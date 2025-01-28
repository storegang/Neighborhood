import { PostCard } from "./index"

export const PostCardGrid: React.FC = () => {
    const posts = [
        {
            id: 1,
            title: "Post Title 1",
            description: "Post Description 1",
            images: [
                "https://placecats.com/600/400",
                "https://placecats.com/neo/600/400",
                "https://placecats.com/g/600/400",
            ],
        },
        {
            id: 2,
            title: "Post Title 2",
            description: "Post Description 2",
            images: [
                "https://placecats.com/millie/600/400",
                "https://placecats.com/millie_neo/600/400",
            ],
        },
        {
            id: 3,
            title: "Post Title 3",
            description: "Post Description 3",
            images: ["https://placecats.com/neo_2/600/400"],
        },
    ]

    return (
        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
            {posts.map((post) => (
                <PostCard
                    title={post.title}
                    description={post.description}
                    key={post.title}
                    images={post.images}
                />
            ))}
        </div>
    )
}

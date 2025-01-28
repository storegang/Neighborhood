import { PostCard } from "./index"

export const PostCardGrid: React.FC = () => {
    const posts = [
        {
            id: 1,
            title: "Post Title 1",
            description: "Post Description 1",
            imageList: [
                "https://placecats.com/600/400",
                "https://placecats.com/neo/600/400",
                "https://placecats.com/g/600/400",
            ],
        },
    ]

    return (
        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
            {posts.map((post) => (
                <PostCard
                    title={post.title}
                    description={post.description}
                    key={post.title}
                    imageList={post.imageList}
                />
            ))}
        </div>
    )
}

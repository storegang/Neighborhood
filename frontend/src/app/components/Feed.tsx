import { PostCard } from "./index"

export const Feed: React.FC = () => {
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
        <div className="w-full">
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

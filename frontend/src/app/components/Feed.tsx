import { PostCard } from "./index"

import fakePosts from "../fakePosts.json"

export const Feed: React.FC = () => {
    return (
        <div className="w-full lg:w-96 mx-auto">
            {fakePosts.posts.map((post) => (
                <PostCard
                    author={post.author}
                    likes={post.likes}
                    comments={post.comments}
                    title={post.title}
                    description={post.description}
                    key={post.title}
                    imageList={post.imageList}
                />
            ))}
        </div>
    )
}

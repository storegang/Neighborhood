import { Carousel } from "./index"
import { Post } from "@/Models/Post"

export const PostCard: React.FC<Post> = ({
    id,
    title,
    author,
    content,
    imageList,
    likes,
    comments,
    dateTimePosted,
    Neighborhood,
    category,
}) => {
    return (
        <div className="card shadow-sm">
            <figure>
                {imageList && imageList.length > 0 ? (
                    <Carousel imageList={imageList} />
                ) : null}
            </figure>
            <div className="card-body">
                <div className="flex h-8 w-full items-center justify-start gap-4">
                    <div className="avatar w-8">
                        <div className="mask mask-squircle">
                            <img
                                className="h-full w-full"
                                src={author.avatar}
                            />
                        </div>
                    </div>
                    <p className="w-1/2 truncate text-sm">{author.name}</p>
                </div>
                <h2 className="card-title">{title}</h2>
                <p>{content}</p>
            </div>
        </div>
    )
}

import {
    ChatBubbleLeftIcon,
    HandThumbUpIcon,
} from "@heroicons/react/24/outline"

import { Carousel, CommentField } from "./index"
import { Post } from "@/Models/Post"

export const PostCard: React.FC<Post> = ({
    id,
    title,
    author,
    description,
    imageList,
    likes,
    comments,
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
                <p>{description}</p>
                <div tabIndex={0} className="collapse">
                    <div className="collapse-title card-actions items-center justify-end">
                        <HandThumbUpIcon className="hover:text-primary h-4 w-4 cursor-pointer text-gray-400" />
                        {likes.count}
                        <ChatBubbleLeftIcon className="hover:text-primary h-4 w-4 cursor-pointer text-gray-400" />
                        {comments.count}
                    </div>
                    <div className="collapse-content">
                        <CommentField comments={comments.comments} />
                    </div>
                </div>
            </div>
        </div>
    )
}

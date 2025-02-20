import { formatDate } from "@/lib/formatDate"
import { Carousel } from "./index"
import { Post } from "@/Models/Post"

export const PostCard: React.FC<Post> = ({
    title,
    description,
    imageUrls,
    categoryId,
    authorUser,
    commentCount,
    likedByUserCount,
    likedByCurrentUser,
    datePosted,
    dateLastEdited,
}) => {
    const { name, avatar } = authorUser

    return (
        <div className="card shadow-sm">
            <figure>
                {imageUrls && imageUrls.length > 0 ? (
                    <Carousel imageList={imageUrls} />
                ) : null}
            </figure>
            <div className="card-body">
                <div className="flex h-8 w-full items-center justify-start gap-4">
                    <div className="avatar w-8">
                        <div className="mask mask-squircle">
                            <img
                                alt={name}
                                className="h-full w-full"
                                src={avatar}
                            />
                        </div>
                    </div>
                    <p className="w-1/2 truncate text-sm">{name}</p>
                    <div className="badge badge-soft badge-primary">
                        Category
                    </div>
                </div>
                {/* 
                TODO: Get the right category */}
                <p className="text-neutral-500">{formatDate(datePosted)}</p>
                <h2 className="card-title">{title}</h2>
                <p>{description}</p>
                <div className="card-actions justify-end">
                    <button className="btn btn-ghost btn-sm">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="2.5"
                            stroke="currentColor"
                            className="size-[1.2em]"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M21 8.25c0-2.485-2.099-4.5-4.688-4.5-1.935 0-3.597 1.126-4.312 2.733-.715-1.607-2.377-2.733-4.313-2.733C5.1 3.75 3 5.765 3 8.25c0 7.22 9 12 9 12s9-4.78 9-12Z"
                            />
                        </svg>
                        <span>{likedByUserCount}</span>
                    </button>
                    <button className="btn btn-ghost btn-sm">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth={1.5}
                            stroke="currentColor"
                            className="size-[1.2em]"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M7.5 8.25h9m-9 3H12m-9.75 1.51c0 1.6 1.123 2.994 2.707 3.227 1.129.166 2.27.293 3.423.379.35.026.67.21.865.501L12 21l2.755-4.133a1.14 1.14 0 0 1 .865-.501 48.172 48.172 0 0 0 3.423-.379c1.584-.233 2.707-1.626 2.707-3.228V6.741c0-1.602-1.123-2.995-2.707-3.228A48.394 48.394 0 0 0 12 3c-2.392 0-4.744.175-7.043.513C3.373 3.746 2.25 5.14 2.25 6.741v6.018Z"
                            />
                        </svg>

                        <span>{commentCount}</span>
                    </button>
                </div>
            </div>
        </div>
    )
}

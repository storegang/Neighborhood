import { formatDate } from "@/lib/formatDate"
import { Post } from "@/Models/Post"
import Image from "next/image"
import { useGetCategories } from "../queries"
import { useUser } from "@/lib/getUser"

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

    likedByCurrentUser = true

    return (
        <div className="card shadow-sm">
            {imageUrls && imageUrls.length ? (
                <div className="lg:hidden">
                    <figure className="lg:h-32 lg:w-32 lg:self-center">
                        <Image
                            className="h-full w-full object-cover"
                            src={imageUrls[0]}
                            alt={title}
                            width={400}
                            height={300}
                        />
                    </figure>
                </div>
            ) : null}
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
                <p className="text-neutral-500">{formatDate(datePosted)}</p>
                <div className="justify-between lg:flex">
                    <div className="lg:w-3/4">
                        <h2 className="card-title">{title}</h2>
                        <p>{description}</p>
                    </div>
                    {imageUrls && imageUrls.length ? (
                        <div className="ml-4 hidden gap-2 lg:flex lg:h-28 lg:w-28">
                            <Image
                                key={imageUrls[0]}
                                className="h-full w-full rounded-sm object-cover"
                                src={imageUrls[0]}
                                alt={title}
                                width={400}
                                height={300}
                            />
                        </div>
                    ) : null}
                </div>
                <div className="card-actions justify-start">
                    <button className="btn btn-ghost btn-sm">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 24 24"
                            fill={likedByCurrentUser ? "currentColor" : "none"}
                            stroke="currentColor"
                            strokeWidth="1.5"
                            className={`size-[1.2em] ${likedByCurrentUser ? "text-red-500" : "text-black"}`}
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="m11.645 20.91-.007-.003-.022-.012a15.247 15.247 0 0 1-.383-.218 25.18 25.18 0 0 1-4.244-3.17C4.688 15.36 2.25 12.174 2.25 8.25 2.25 5.322 4.714 3 7.688 3A5.5 5.5 0 0 1 12 5.052 5.5 5.5 0 0 1 16.313 3c2.973 0 5.437 2.322 5.437 5.25 0 3.925-2.438 7.111-4.739 9.256a25.175 25.175 0 0 1-4.244 3.17 15.247 15.247 0 0 1-.383.219l-.022.012-.007.004-.003.001a.752.752 0 0 1-.704 0l-.003-.001Z"
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

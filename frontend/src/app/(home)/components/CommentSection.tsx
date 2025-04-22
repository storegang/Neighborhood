import { useUser } from "@/lib/getUser"
import { useAddComment, useGetComments } from "../queries"
import { Comment } from "./Comment"
import { PostResponse } from "@/Models"

type CommentSectionProps = {
    postId: PostResponse["id"]
}

export const CommentSection: React.FC<CommentSectionProps> = ({ postId }) => {
    const user = useUser()

    const { mutate: addComment } = useAddComment(user!, postId)

    const { data: comments } = useGetComments(user!, postId)

    return (
        <section>
            {comments?.map((comment) => (
                <Comment key={comment.id} comment={comment} />
            ))}
            <div className="chat chat-start">
                <div className="chat-image">
                    <div className="chat avatar mask mask-squircle w-8">
                        <img
                            alt={user?.name}
                            className="h-full w-full object-contain"
                            src={user?.photoURL || undefined}
                        />
                    </div>
                </div>
                <form
                    onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
                        e.preventDefault()
                        /* @ts-expect-error Denne finnes og vil alltid finnes, er textarea under  */
                        const comment = e.currentTarget.elements.comment.value

                        if (!comment) return

                        addComment(comment)
                        e.currentTarget.reset()
                    }}
                    className="flex w-1/2 gap-2"
                >
                    <div className="chat-bubble w-full p-1">
                        <textarea
                            name="comment"
                            placeholder="Comment"
                            className="chat-bubble w-full resize-none ps-2 pe-2"
                        ></textarea>
                    </div>
                    <button
                        type="submit"
                        aria-label="comment"
                        data-tooltip-id="comment"
                        data-tooltip-content="Comment"
                        title="comment"
                        className="btn btn-xs btn-round btn-secondary grow-0 self-end"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="size-4"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M6 12 3.269 3.125A59.769 59.769 0 0 1 21.485 12 59.768 59.768 0 0 1 3.27 20.875L5.999 12Zm0 0h7.5"
                            />
                        </svg>
                    </button>
                </form>
            </div>
        </section>
    )
}

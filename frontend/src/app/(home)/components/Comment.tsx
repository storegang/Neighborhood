import { CommentResponse } from "@/Models/Comment"
import { formatRelativeDate } from "../../../lib/formatters/formatDate"

type CommentProps = {
    comment: CommentResponse
}

export const Comment: React.FC<CommentProps> = ({ comment }) => {
    const datePosted = new Date(comment.datePosted)
    const dateLastEdited = new Date(comment.dateLastEdited)

    const date = dateLastEdited > datePosted ? dateLastEdited : datePosted

    return (
        <>
            <div className="chat chat-start">
                <div className="chat-image avatar">
                    <div className="w-10 rounded-full">
                        <img
                            alt="Tailwind CSS chat bubble component"
                            src={
                                comment.authorUser.avatar ||
                                "https://png.pngtree.com/png-vector/20220608/ourmid/pngtree-man-avatar-isolated-on-white-background-png-image_4891418.png"
                            }
                        />
                    </div>
                </div>
                <div className="chat-header">
                    {comment.authorUser.name}
                    <time className="text-xs opacity-50">
                        {formatRelativeDate(date)}
                    </time>
                </div>
                <div className="chat-bubble">{comment.content}</div>
            </div>
        </>
    )
}

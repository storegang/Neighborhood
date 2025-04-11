import { User } from "./User"

export type CommentRequest = {
    id: string
    authorUserId: string
    content: string
    datePosted?: Date
    dateLastEdited?: Date
    parentPostId: string
}

export type CommentResponse = {
    authorUser: User
    authorUserId: User["uid"]
    content: string
    dateLastEdited: Date
    datePosted: Date
    id: string
    imageUrls: string[]
    likedByCurrentUser: boolean
    likedByUserCount: number
    parentPostId: string
}

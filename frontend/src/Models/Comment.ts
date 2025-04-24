import { User } from "./User"

export type CommentRequest = {
    content: string
    parentPostId: string
    imageUrl: string[]
}

export type CommentResponse = {
    id: string
    content: string
    authorUser: User
    authorUserId: User["uid"]
    dateLastEdited: Date
    datePosted: Date
    imageUrls: string[]
    likedByCurrentUser: boolean
    likedByUserCount: number
    parentPostId: string
}

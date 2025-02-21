import { User } from "./User"

export type Post = {
    id: string
    title: string
    description: string
    datePosted: string
    dateLastEdited: string
    authorUserId: string
    authorUser: User
    categoryId: string
    imageUrls: string[]
    commentCount: number
    likedByUserCount: number
    likedByCurrentUser: boolean
}

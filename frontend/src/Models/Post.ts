import { User } from "./User"

export type Post = {
    id: string
    title: string
    description: string
    datePosted: string
    dateLastEdited: string | null
    authorUserId: string
    authorUser: User
    categoryId: string | null
    imageUrls: string[]
    commentCount: number
    likedByUserCount: number
    likedByCurrentUser: boolean
}

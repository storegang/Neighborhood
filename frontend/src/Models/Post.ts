import { Category } from "./Category"
import { User } from "./User"

export type PostRequest = {
    title: string
    description: string
    categoryId: Category["id"]
    imageUrls: string[]
}

export type PostResponse = {
    id: string
    title: string
    description: string
    datePosted: string
    dateLastEdited: string | null
    authorUserId: User["uid"]
    authorUser: User
    categoryId: Category["id"]
    imageUrls: string[]
    commentCount: number
    likedByUserCount: number
    likedByCurrentUser: boolean
}

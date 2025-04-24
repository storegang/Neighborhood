import { CategoryResponse } from "./Category"
import { User } from "./User"

export type PostRequest = {
    title: string
    content: string
    categoryId: string
    userUID: string
    accessToken: string
}

export type PostResponse = {
    id: string
    title: string
    description: string
    datePosted: string
    dateLastEdited: string | null
    authorUserId: User["uid"]
    authorUser: User
    categoryId: CategoryResponse["id"]
    imageUrls: string[]
    commentCount: number
    likedByUserCount: number
    likedByCurrentUser: boolean
}

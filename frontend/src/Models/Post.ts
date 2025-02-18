import { User } from "./User"
import { Neighborhood } from "./Neighborhood"
import { Comment } from "./Comment"
import { Category } from "./Category"

export type Post = {
    id: string
    author: User
    title: string
    content: string
    datePosted?: Date
    dateLastEdited?: Date
    categoryId: string
    imageList?: string[]
    likes?: string[]
}

import { User } from "./User"
import { Neighborhood } from "./Neighborhood"

export type Post = {
    id: string
    author: User
    title: string
    content: string
    imageList?: string[]
    likes: string[]
    comments: Comment[]
    dateTimePosted: Date
    Neighborhood: Neighborhood
}

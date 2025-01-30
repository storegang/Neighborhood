import { Comment } from "./Comment"
import { Likes } from "./Likes"
import { User } from "./User"

export type Post = {
    id?: number
    author: User
    title: string
    description: string
    imageList?: string[]
    likes: Likes
    comments: {
        count: number
        comments: Comment[]
    }
}

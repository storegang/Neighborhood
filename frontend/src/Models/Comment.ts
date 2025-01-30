import { Likes } from "./Likes"
import { User } from "./User"

export type Comment = {
    id: number
    author: User
    content: string
    likes: Likes
}

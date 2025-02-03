import { User } from "./User"

export type Comment = {
    id: string
    author: User
    content: string
    likes: string[]
    postedDate: Date
}

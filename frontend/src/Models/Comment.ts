import { User } from "./User"

export type Comment = {
    id: string
    authorId: string
    content: string
    datePosted?: Date
    dateLastEdited?: Date
    parentPostId: string
    likes?: string[]
    image?: string
}

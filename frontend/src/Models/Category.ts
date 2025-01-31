import { Post } from "./Post"

export type Category = {
    id: string
    name: string
    color: string
    posts: Post[]
}

import { PostResponse } from "./Post"
import { User } from "./User"

export type LikeRequest = {
    postId: PostResponse["id"]
    User: User
}

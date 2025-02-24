import { User as FirebaseUser } from "firebase/auth"

export type User = FirebaseUser & {
    uid: string
    name: string
    avatar?: string
    neighborhoodId?: string
    accessToken: string
}

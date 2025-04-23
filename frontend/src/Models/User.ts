import { User as FirebaseUser } from "firebase/auth"

export type User = FirebaseUser & {
    id: string
    name: string
    avatar?: string
    neighborhoodId?: string
    accessToken: string
}

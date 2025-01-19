"use client"

import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"

interface PostProps {
    name: string
    title: string
    content?: string
    profilePicture?: string
    timeStamp?: Date
}

const Post: React.FC<PostProps> = ({
    title,
    content,
    profilePicture,
    name,
}) => {
    const placeholderTimeStamp = new Date()
    placeholderTimeStamp.setHours(placeholderTimeStamp.getHours() - 3)

    const now = new Date()
    const diffInMilliseconds = now.getTime() - placeholderTimeStamp.getTime()
    const diffInHours = Math.floor(diffInMilliseconds / (1000 * 60 * 60))

    return (
        <Card>
            <div className="flex flex-col">
                <div className="flex items-center">
                    <Avatar>
                        <AvatarImage src={profilePicture} />
                        <AvatarFallback>CN</AvatarFallback>
                    </Avatar>
                    <CardHeader>
                        <CardTitle>{name}</CardTitle>
                    </CardHeader>
                </div>
                <CardDescription>
                    <p>{diffInHours} hours ago</p>
                </CardDescription>
            </div>
            <CardContent>
                <h2>{title}</h2>
                <p>{content}</p>
            </CardContent>
            <CardFooter>
                <p>Her kommer det funksjon for å kunne kommentere/like</p>
            </CardFooter>
        </Card>
    )
}

export default Post

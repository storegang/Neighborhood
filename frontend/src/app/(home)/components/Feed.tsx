"use client"

import { Post } from "@/Models/Post"
import { PostCard } from "./index"
import { useUser } from "@/lib/getUser"

export const Feed: React.FC = () => {
    const fakePost: Post = {
        id: "101",
        author: {
            id: "27",
            name: "Marie Nilsen",
            avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Aiden",
        },
        title: "Trenger hjelp med snømåking ❄️",
        content:
            "Hei naboer! Jeg har skadet foten og sliter med å måke innkjørselen min. Er det noen som har tid til å hjelpe meg i dag? Jeg stiller med kaffe og varm kakao! ☕️❄️",
        imageList: [
            "https://plus.unsplash.com/premium_photo-1737912828325-f72452894789?q=80&w=2574&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
        ],
        likes: ["3", "8", "19"],
        
        datePosted: new Date("2025-02-03T13:00:00Z"),
        
        categoryId: "",
    }

    const user = useUser()

    console.log(user)

    return (
        <div className="mx-auto w-full lg:w-96">
            <PostCard
                author={fakePost.author}
                likes={fakePost.likes}
                title={fakePost.title}
                content={fakePost.content}
                key={fakePost.title}
                imageList={fakePost.imageList}
                datePosted={fakePost.datePosted}
                categoryId={fakePost.categoryId}
                id={fakePost.id}
            />
        </div>
    )
}

import { Post } from "@/Models/Post"
import { PostCard } from "./index"

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
        comments: [
            {
                id: "201",
                author: {
                    id: "8",
                    name: "Lise Berg",
                    avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Amaya&backgroundType=solid,gradientLinear&backgroundColor=b6e3f4,c0aede,d1d4f9,ffd5dc,ffdfbf,transparent",
                },
                content: "Jeg kan komme innom etter jobb! 😊",
                likes: ["27", "15"],
                postedDate: new Date("2025-02-03T14:30:00Z"),
            },
            {
                id: "202",
                author: {
                    id: "15",
                    name: "Torgeir Nilsen",
                    avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Caleb&backgroundType=solid,gradientLinear&backgroundColor=b6e3f4,c0aede,d1d4f9,ffd5dc,ffdfbf,transparent",
                },
                content: "Har en snøfreser, kan ta en tur forbi senere i dag!",
                likes: ["8", "23"],
                postedDate: new Date("2025-02-03T15:00:00Z"),
            },
        ],
        dateTimePosted: new Date("2025-02-03T13:00:00Z"),
        Neighborhood: {
            id: "10",
            name: "Bjørndal Borettslag",
            description: "Et hyggelig nabolag i Oslo med engasjerte beboere!",
            members: [
                {
                    id: "27",
                    name: "Marie Nilsen",
                    avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Aiden",
                },
                {
                    id: "8",
                    name: "Lise Berg",
                    avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Amaya&backgroundType=solid,gradientLinear&backgroundColor=b6e3f4,c0aede,d1d4f9,ffd5dc,ffdfbf,transparent",
                },
                {
                    id: "15",
                    name: "Torgeir Nilsen",
                    avatar: "https://api.dicebear.com/9.x/notionists/svg?seed=Caleb&backgroundType=solid,gradientLinear&backgroundColor=b6e3f4,c0aede,d1d4f9,ffd5dc,ffdfbf,transparent",
                },
            ],
            posts: [],
            categories: [],
        },
        category: {
            id: "1",
            name: "Hjelp",
            color: "#FF5733",
            posts: [],
        },
    }

    return (
        <div className="mx-auto w-full lg:w-96">
            <PostCard
                author={fakePost.author}
                likes={fakePost.likes}
                comments={fakePost.comments}
                title={fakePost.title}
                content={fakePost.content}
                key={fakePost.title}
                imageList={fakePost.imageList}
                dateTimePosted={fakePost.dateTimePosted}
                category={fakePost.category}
                Neighborhood={fakePost.Neighborhood}
                id={fakePost.id}
            />
        </div>
    )
}

import { BoardMember } from "./BoardMember"

export const BoardMembersList: React.FC = () => {
    // TODO: Get users that are board members, map over them and render BoardMember for each one

    return (
        <ul className="list bg-base-100 rounded-box h-80 w-1/2 overflow-y-scroll shadow-md">
            <li className="p-4 pb-2 text-xs tracking-wide opacity-60">
                Board members
            </li>
            <BoardMember
                name="Dio Lupa"
                role="Chairperson"
                phoneNumber="82936492"
                avatarUrl="https://img.daisyui.com/images/profile/demo/1@94.webp"
                email="diolupa@gmail.com"
            />
            <BoardMember
                name="Ellie Beilish"
                role="Vice Chairperson"
                phoneNumber="98394729"
                avatarUrl="https://img.daisyui.com/images/profile/demo/4@94.webp"
                email="ellie.beilish@gmail.com"
            />
            <BoardMember
                name="Sabrino Gardener"
                role="Board member"
                phoneNumber="93629473"
                avatarUrl="https://img.daisyui.com/images/profile/demo/3@94.webp"
            />
            <BoardMember
                name="Sabrino Gardener"
                role="Board member"
                phoneNumber="93629473"
                avatarUrl="https://img.daisyui.com/images/profile/demo/3@94.webp"
            />
            <BoardMember
                name="Sabrino Gardener"
                role="Board member"
                phoneNumber="93629473"
                avatarUrl="https://img.daisyui.com/images/profile/demo/3@94.webp"
            />
            <BoardMember
                name="Sabrino Gardener"
                role="Board member"
                phoneNumber="93629473"
                avatarUrl="https://img.daisyui.com/images/profile/demo/3@94.webp"
            />
            <BoardMember
                name="Sabrino Gardener"
                role="Board member"
                phoneNumber="93629473"
                avatarUrl="https://img.daisyui.com/images/profile/demo/3@94.webp"
            />
        </ul>
    )
}

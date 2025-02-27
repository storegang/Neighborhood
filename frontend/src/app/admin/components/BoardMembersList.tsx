import { BoardMember } from "./BoardMember"

export const BoardMembersList: React.FC = () => {
    // TODO: Get users that are board members, map over them and render BoardMember for each one

    return (
        <ul className="list bg-base-100 rounded-box w-1/2 shadow-md">
            <li className="p-4 pb-2 text-xs tracking-wide opacity-60">
                Board members
            </li>
            <BoardMember
                name="Dio Lupa"
                role="Chairperson"
                phoneNumber="82936492"
                avatarUrl="https://img.daisyui.com/images/profile/demo/1@94.webp"
            />
        </ul>
    )
}

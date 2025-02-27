import { formatPhoneNumber } from "@/lib/formatters/formatPhoneNumber"

export const BoardMembers: React.FC = () => {
    return (
        <ul className="list bg-base-100 rounded-box shadow-md">
            <li className="p-4 pb-2 text-xs tracking-wide opacity-60">
                Most played songs this week
            </li>

            <li className="list-row">
                <div>
                    <img
                        alt="Dio Lupa"
                        className="rounded-box size-10"
                        src="https://img.daisyui.com/images/profile/demo/1@94.webp"
                    />
                </div>
                <div>
                    <div>Dio Lupa</div>
                    <div className="text-xs font-semibold uppercase opacity-60">
                        Chairperson
                    </div>
                </div>
                <p className="list-col-wrap flex gap-2 text-xs">
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M2.25 6.75c0 8.284 6.716 15 15 15h2.25a2.25 2.25 0 0 0 2.25-2.25v-1.372c0-.516-.351-.966-.852-1.091l-4.423-1.106c-.44-.11-.902.055-1.173.417l-.97 1.293c-.282.376-.769.542-1.21.38a12.035 12.035 0 0 1-7.143-7.143c-.162-.441.004-.928.38-1.21l1.293-.97c.363-.271.527-.734.417-1.173L6.963 3.102a1.125 1.125 0 0 0-1.091-.852H4.5A2.25 2.25 0 0 0 2.25 4.5v2.25Z"
                        />
                    </svg>
                    {formatPhoneNumber("555-555-5555")}
                </p>
                <button className="btn btn-square btn-ghost">
                    {""}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M6 3L20 12 6 21 6 3z"></path>
                        </g>
                    </svg>
                </button>
                <button className="btn btn-square btn-ghost">
                    {" "}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z"></path>
                        </g>
                    </svg>
                </button>
            </li>

            <li className="list-row">
                <div>
                    <img
                        alt="Ellie Beilish"
                        className="rounded-box size-10"
                        src="https://img.daisyui.com/images/profile/demo/4@94.webp"
                    />
                </div>
                <div>
                    <div>Ellie Beilish</div>
                    <div className="text-xs font-semibold uppercase opacity-60">
                        Vice chair
                    </div>
                </div>
                <p className="list-col-wrap text-xs">
                    {formatPhoneNumber("555-555-5555")}
                </p>
                <button className="btn btn-square btn-ghost">
                    {""}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M6 3L20 12 6 21 6 3z"></path>
                        </g>
                    </svg>
                </button>
                <button className="btn btn-square btn-ghost">
                    {" "}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z"></path>
                        </g>
                    </svg>
                </button>
            </li>

            <li className="list-row">
                <div>
                    <img
                        alt="Sabrino Gardener"
                        className="rounded-box size-10"
                        src="https://img.daisyui.com/images/profile/demo/3@94.webp"
                    />
                </div>
                <div>
                    <div>Sabrino Gardener</div>
                    <div className="text-xs font-semibold uppercase opacity-60">
                        Board memnber
                    </div>
                </div>
                <p className="list-col-wrap text-xs">
                    "Cappuccino" quickly gained attention for its smooth melody
                    and relatable themes. The song’s success propelled Sabrino
                    into the spotlight, solidifying their status as a rising
                    star.
                </p>
                <button className="btn btn-square btn-ghost">
                    {}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M6 3L20 12 6 21 6 3z"></path>
                        </g>
                    </svg>
                </button>
                <button className="btn btn-square btn-ghost">
                    {" "}
                    <svg
                        className="size-[1.2em]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                    >
                        <g
                            strokeLinejoin="round"
                            strokeLinecap="round"
                            strokeWidth="2"
                            fill="none"
                            stroke="currentColor"
                        >
                            <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z"></path>
                        </g>
                    </svg>
                </button>
            </li>
        </ul>
    )
}

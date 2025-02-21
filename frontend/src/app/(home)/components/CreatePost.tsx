export const CreatePost: React.FC = () => {
    return (
        <div className="card card-border border-base-300 card-sm overflow-hidden">
            <div className="border-base-300 border-b border-dashed">
                <div className="flex items-center gap-2 p-4">
                    <div className="grow">
                        <div className="flex items-center gap-2 text-sm font-medium">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth="1.5"
                                stroke="currentColor"
                                className="size-5 opacity-40"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10"
                                ></path>
                            </svg>{" "}
                            Write a new post
                        </div>
                    </div>
                </div>
            </div>{" "}
            <div className="card-body gap-4">
                <div className="flex items-center justify-between">
                    <input
                        type="text"
                        placeholder="Title"
                        className="input input-md"
                    />
                    <button className="btn btn-xs">Add files</button>
                </div>{" "}
                <textarea
                    className="textarea textarea-border w-full resize-none"
                    placeholder="What's happening?"
                ></textarea>
                <div className="flex gap-4">
                    <div className="badge badge-soft badge-primary">
                        Primary
                    </div>
                    <div className="badge badge-soft badge-secondary">
                        Secondary
                    </div>
                    <div className="badge badge-soft badge-accent">Accent</div>
                    <div className="badge badge-soft badge-neutral">
                        Neutral
                    </div>
                    <div className="badge badge-soft badge-info">Info</div>
                </div>
                <div className="card-actions justify-end">
                    <button className="btn btn-primary">Publish</button>
                </div>
            </div>
        </div>
    )
}

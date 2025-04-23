"use client"

import { useState } from "react"
import { DatePicker } from "./DatePicker"

export const BoardMeetingsList: React.FC = () => {
    const [showScheduleMeetingDialog, setShowScheduleMeetingDialog] =
        useState(false)

    return (
        <>
            <div className="tabs tabs-border bg-base-100 w-full">
                <input
                    type="radio"
                    name="my_tabs_2"
                    className="tab"
                    aria-label="Planned meetings"
                    defaultChecked
                />
                <div className="tab-content border-base-300 bg-base-100 p-6">
                    <ul className="list">
                        <li className="list-row">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="size-6"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-9-6h.008v.008H12v-.008ZM12 15h.008v.008H12V15Zm0 2.25h.008v.008H12v-.008ZM9.75 15h.008v.008H9.75V15Zm0 2.25h.008v.008H9.75v-.008ZM7.5 15h.008v.008H7.5V15Zm0 2.25h.008v.008H7.5v-.008Zm6.75-4.5h.008v.008h-.008v-.008Zm0 2.25h.008v.008h-.008V15Zm0 2.25h.008v.008h-.008v-.008Zm2.25-4.5h.008v.008H16.5v-.008Zm0 2.25h.008v.008H16.5V15Z"
                                />
                            </svg>
                            <div>
                                <div>Friday May 2 2025</div>
                                <div className="text-xs font-semibold uppercase opacity-60">
                                    20:00 - 21:00
                                </div>
                            </div>
                        </li>
                        <li className="list-row">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="size-6"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-9-6h.008v.008H12v-.008ZM12 15h.008v.008H12V15Zm0 2.25h.008v.008H12v-.008ZM9.75 15h.008v.008H9.75V15Zm0 2.25h.008v.008H9.75v-.008ZM7.5 15h.008v.008H7.5V15Zm0 2.25h.008v.008H7.5v-.008Zm6.75-4.5h.008v.008h-.008v-.008Zm0 2.25h.008v.008h-.008V15Zm0 2.25h.008v.008h-.008v-.008Zm2.25-4.5h.008v.008H16.5v-.008Zm0 2.25h.008v.008H16.5V15Z"
                                />
                            </svg>
                            <div>
                                <div>Tuesday October 21 2025</div>
                                <div className="text-xs font-semibold uppercase opacity-60">
                                    19:00 - 20:00
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <input
                    type="radio"
                    name="my_tabs_2"
                    className="tab"
                    aria-label="Previous meetings"
                />
                <div className="tab-content border-base-300 bg-base-100 p-6">
                    <ul className="list">
                        <li className="list-row">
                            <div>
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    fill="none"
                                    viewBox="0 0 24 24"
                                    strokeWidth={1.5}
                                    stroke="currentColor"
                                    className="size-6"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-9-6h.008v.008H12v-.008ZM12 15h.008v.008H12V15Zm0 2.25h.008v.008H12v-.008ZM9.75 15h.008v.008H9.75V15Zm0 2.25h.008v.008H9.75v-.008ZM7.5 15h.008v.008H7.5V15Zm0 2.25h.008v.008H7.5v-.008Zm6.75-4.5h.008v.008h-.008v-.008Zm0 2.25h.008v.008h-.008V15Zm0 2.25h.008v.008h-.008v-.008Zm2.25-4.5h.008v.008H16.5v-.008Zm0 2.25h.008v.008H16.5V15Z"
                                    />
                                </svg>
                            </div>
                            <div>
                                <div>Friday October 11 2024</div>
                                <div className="text-xs font-semibold uppercase opacity-60">
                                    20:00 - 21:00
                                </div>
                            </div>
                        </li>
                        <li className="list-row">
                            <div>
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    fill="none"
                                    viewBox="0 0 24 24"
                                    strokeWidth={1.5}
                                    stroke="currentColor"
                                    className="size-6"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-9-6h.008v.008H12v-.008ZM12 15h.008v.008H12V15Zm0 2.25h.008v.008H12v-.008ZM9.75 15h.008v.008H9.75V15Zm0 2.25h.008v.008H9.75v-.008ZM7.5 15h.008v.008H7.5V15Zm0 2.25h.008v.008H7.5v-.008Zm6.75-4.5h.008v.008h-.008v-.008Zm0 2.25h.008v.008h-.008V15Zm0 2.25h.008v.008h-.008v-.008Zm2.25-4.5h.008v.008H16.5v-.008Zm0 2.25h.008v.008H16.5V15Z"
                                    />
                                </svg>
                            </div>
                            <div>
                                <div>Wednesday March 6 2024</div>
                                <div className="text-xs font-semibold uppercase opacity-60">
                                    19:00 - 20:00
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <ScheduleMeetingDialog
                show={showScheduleMeetingDialog}
                onClose={() => setShowScheduleMeetingDialog(false)}
            />
        </>
    )
}

const ScheduleMeetingDialog: React.FC<{
    show: boolean
    onClose: () => void
}> = () => {
    return (
        <>
            <button
                className="btn btn-secondary self-start"
                onClick={() =>
                    (
                        document.getElementById(
                            "schedule-meeting-dialog"
                        ) as HTMLDialogElement
                    )?.showModal()
                }
            >
                Schedule meeting
            </button>
            <dialog id="schedule-meeting-dialog" className="modal">
                <div className="modal-box">
                    <h3 className="text-lg font-bold">Schedule a meeting</h3>
                    <fieldset className="fieldset bg-base-200 border-base-300 rounded-box w-full border p-4">
                        <legend className="fieldset-legend">
                            Meeting details
                        </legend>
                        <DatePicker />
                        <legend className="fieldset-legend">Description</legend>
                        <textarea
                            className="textarea h-24"
                            placeholder="Meeting description"
                        ></textarea>
                        <div className="label">Optional</div>
                    </fieldset>
                    <div className="modal-action">
                        <form method="dialog">
                            <button className="btn btn-primary">
                                Schedule
                            </button>
                            <button className="btn">Close</button>
                        </form>
                    </div>
                </div>
            </dialog>
        </>
    )
}

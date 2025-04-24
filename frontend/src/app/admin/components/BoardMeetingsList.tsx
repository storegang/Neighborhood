"use client"

import { useState } from "react"
import { DatePicker } from "./DatePicker"
import { TabPanel, Tabs } from "@/components/Tabs"
import { useCreateMeeting, useGetMeetings } from "../queries"
import { useUser } from "@/lib/getUser"
import { formatRelativeDate } from "../../../lib/formatters/formatDate"
import { Meeting } from "@/Models"

export const BoardMeetingsList: React.FC = () => {
    const user = useUser()
    const { data: meetings, isLoading, isError } = useGetMeetings(user ?? null)

    if (isError) {
        return <div className="alert alert-error shadow-lg"> ERROR ERROR </div>
    }

    if (isLoading) {
        return (
            <div className="alert alert-info shadow-lg">
                Loading meetings...
            </div>
        )
    }

    if (!meetings?.length) {
        return (
            <div className="alert alert-info shadow-lg">
                No meetings scheduled
            </div>
        )
    }

    const plannedMeetings = meetings.filter(
        (meeting) => meeting.date > new Date()
    )
    const previousMeetings = meetings.filter(
        (meeting) => meeting.date < new Date()
    )

    return (
        <>
            <Tabs name="board-meetings">
                {!!plannedMeetings?.length && (
                    <TabPanel label="Planned meetings" defaultChecked>
                        <ul className="list">
                            {plannedMeetings?.map((meeting) => (
                                <MeetingItem
                                    key={meeting.id}
                                    meeting={meeting}
                                />
                            ))}
                        </ul>
                    </TabPanel>
                )}
                {!!previousMeetings?.length && (
                    <TabPanel label="Previous meetings">
                        <ul className="list">
                            {previousMeetings?.map((meeting) => (
                                <MeetingItem
                                    key={meeting.id}
                                    meeting={meeting}
                                />
                            ))}
                        </ul>
                    </TabPanel>
                )}
            </Tabs>
            <ScheduleMeetingDialog />
        </>
    )
}

const ScheduleMeetingDialog: React.FC = () => {
    const user = useUser()
    const {
        mutate: createMeeting,
        isPending,
        isError,
    } = useCreateMeeting(user ?? null)

    const [selectedDate, setSelectedDate] = useState<Date>(new Date())

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        const formData = new FormData(e.currentTarget)
        e.preventDefault()

        const title = formData.get("title") as string

        const type = formData.get("meeting-type") as string

        if (!title || !selectedDate || !type) {
            return
        }

        const meeting: Omit<Meeting, "id"> = {
            title,
            date: selectedDate,
            type: type === "board-meeting" ? "BOARD_MEETING" : "TOWN_HALL",
        }

        createMeeting(meeting, {
            onSuccess: () => {
                return (
                    document.getElementById(
                        "schedule-meeting-dialog"
                    ) as HTMLDialogElement
                )?.close()
            },
        })
    }

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
                    <form onSubmit={handleSubmit}>
                        <h3 className="text-lg font-bold">
                            Schedule a meeting
                        </h3>
                        <div className="fieldset bg-base-200 border-base-300 rounded-box mt-4 w-full border p-4">
                            <DatePicker
                                selectedDate={selectedDate}
                                setSelectedDate={setSelectedDate}
                            />

                            <label className="fieldset-legend" htmlFor="title">
                                Title
                            </label>
                            <input
                                name="title"
                                type="text"
                                className="input input-bordered validator"
                                id="title"
                                required
                            />
                            <legend className="fieldset-legend">
                                Meeting type
                            </legend>
                            <div className="flex w-fit flex-row-reverse gap-2">
                                <label
                                    htmlFor="board-meeting"
                                    className="label cursor-pointer"
                                >
                                    <span className="label-text">
                                        Board meeting
                                    </span>
                                </label>
                                <input
                                    type="radio"
                                    name="meeting-type"
                                    id="board-meeting"
                                    className="radio"
                                    value={"board-meeting"}
                                />
                            </div>
                            <div className="flex w-fit flex-row-reverse gap-2">
                                <label
                                    htmlFor="town-hall"
                                    className="label cursor-pointer"
                                >
                                    <span className="label-text">
                                        Town Hall
                                    </span>
                                </label>
                                <input
                                    type="radio"
                                    name="meeting-type"
                                    id="town-hall"
                                    className="radio"
                                    value={"town-hall"}
                                />
                            </div>
                        </div>
                        {isError && (
                            <div className="alert alert-error shadow-lg">
                                <div>
                                    <span>Something went wrong</span>
                                </div>
                            </div>
                        )}

                        <div className="modal-action">
                            {isPending ? (
                                <button className="btn btn-primary">
                                    <span className="loading loading-spinner">
                                        Scheduling
                                    </span>
                                </button>
                            ) : (
                                <button
                                    className="btn btn-primary"
                                    type="submit"
                                >
                                    Schedule
                                </button>
                            )}
                            <button
                                className="btn"
                                type="button"
                                onClick={() =>
                                    (
                                        document.getElementById(
                                            "schedule-meeting-dialog"
                                        ) as HTMLDialogElement
                                    )?.close()
                                }
                            >
                                Close
                            </button>
                        </div>
                    </form>
                </div>
            </dialog>
        </>
    )
}

const MeetingItem: React.FC<{ meeting: Meeting }> = ({ meeting }) => {
    const meetingTypeLabel =
        meeting?.type === "BOARD_MEETING" ? "Board meeting" : "Town Hall"

    return (
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
                <h2>{meeting?.title}</h2>
                <time className="text-xs font-semibold uppercase opacity-60">
                    {formatRelativeDate(meeting.date)}
                </time>
            </div>
            <div className="badge badge-neutral badge-outline ml-4">
                {meetingTypeLabel}
            </div>
        </li>
    )
}

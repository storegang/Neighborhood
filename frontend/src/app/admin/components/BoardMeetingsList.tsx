"use client"

import { useState } from "react"
import { DatePicker } from "./DatePicker"
import { TabPanel, Tabs } from "@/components/Tabs"
import { useGetMeetings } from "../queries"
import { useUser } from "@/lib/getUser"
import {
    formatDate,
    formatRelativeDate,
} from "../../../lib/formatters/formatDate"
import { Meeting } from "@/Models"

export const BoardMeetingsList: React.FC = () => {
    const [showScheduleMeetingDialog, setShowScheduleMeetingDialog] =
        useState(false)

    const user = useUser()
    const { data: meetings, isLoading, isError } = useGetMeetings(user)

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
                    <TabPanel label="Planned meetings">
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
            {/* <ScheduleMeetingDialog
                show={showScheduleMeetingDialog}
                onClose={() => setShowScheduleMeetingDialog(false)}
            /> */}
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

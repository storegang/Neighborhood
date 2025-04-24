import "react-day-picker/style.css"
import { DayPicker } from "react-day-picker"
import { setHours, setMinutes } from "date-fns"
import React, { ChangeEventHandler, useState } from "react"

import { formatDate } from "@/lib/formatters/formatDate"

// TODO: Required on fields

export const DatePicker: React.FC<{
    selectedDate: Date
    setSelectedDate: (date: Date) => void
}> = ({ selectedDate, setSelectedDate }) => {
    const [timeValue, setTimeValue] = useState<string>("00:00")

    const handleTimeChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const time = e.target.value
        if (!selectedDate) {
            setTimeValue(time)
            return
        }
        const [hours, minutes] = time.split(":").map((str) => parseInt(str, 10))
        const newSelectedDate = setHours(
            setMinutes(selectedDate, minutes),
            hours
        )
        setSelectedDate(newSelectedDate)
        setTimeValue(time)
    }

    const handleDaySelect = (date: Date | undefined) => {
        if (!timeValue || !date) {
            setSelectedDate(selectedDate)
            return
        }
        const [hours, minutes] = timeValue
            .split(":")
            .map((str) => parseInt(str, 10))
        const newDate = new Date(
            date.getFullYear(),
            date.getMonth(),
            date.getDate(),
            hours,
            minutes
        )
        setSelectedDate(newDate)
    }

    return (
        <div className="flex flex-col gap-2">
            <legend className="fieldset-legend">Date and time</legend>
            <label className="input">
                <span className="label">Date:</span>
                <button
                    type="button"
                    popoverTarget="rdp-popover"
                    style={{ anchorName: "--rdp" } as React.CSSProperties}
                >
                    {selectedDate ? formatDate(selectedDate) : ""}
                </button>
            </label>

            <div>
                <label className="input">
                    <span className="label">Time:</span>
                    <input
                        type="time"
                        value={timeValue}
                        name="time"
                        onChange={handleTimeChange}
                        className="input input-time input-sm"
                        required
                    />
                </label>
            </div>
            <div
                popover="auto"
                id="rdp-popover"
                className="dropdown"
                style={
                    {
                        positionAnchor: "--rdp",
                        padding: "1em",
                    } as React.CSSProperties
                }
            >
                <DayPicker
                    className="react-day-picker"
                    mode="single"
                    selected={selectedDate}
                    onSelect={handleDaySelect}
                />
            </div>
        </div>
    )
}

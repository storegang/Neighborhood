import "react-day-picker/style.css"
import { DayPicker } from "react-day-picker"
import { setHours, setMinutes } from "date-fns"
import React, { ChangeEventHandler, useState } from "react"

import { formatDate } from "@/lib/formatters/formatDate"

// TODO: Required on fields

export function DatePicker() {
    const [selected, setSelected] = useState<Date>()
    const [timeValue, setTimeValue] = useState<string>("00:00")

    const handleTimeChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const time = e.target.value
        if (!selected) {
            setTimeValue(time)
            return
        }
        const [hours, minutes] = time.split(":").map((str) => parseInt(str, 10))
        const newSelectedDate = setHours(setMinutes(selected, minutes), hours)
        setSelected(newSelectedDate)
        setTimeValue(time)
    }

    const handleDaySelect = (date: Date | undefined) => {
        if (!timeValue || !date) {
            setSelected(date)
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
        setSelected(newDate)
    }

    return (
        <div className="flex flex-col gap-2">
            <label className="input">
                <span className="label">Date:</span>
                <button
                    popoverTarget="rdp-popover"
                    className=""
                    style={{ anchorName: "--rdp" } as React.CSSProperties}
                >
                    {selected ? formatDate(selected) : ""}
                </button>
            </label>

            <div>
                <label className="input">
                    <span className="label">Time:</span>
                    <input
                        type="time"
                        value={timeValue}
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
                    selected={selected}
                    onSelect={handleDaySelect}
                />
            </div>
        </div>
    )
}

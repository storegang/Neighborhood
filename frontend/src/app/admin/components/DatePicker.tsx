import { formatDate } from "@/lib/formatters/formatDate"
import { useState } from "react"
import { DayPicker } from "react-day-picker"
import "react-day-picker/style.css"

export const DatePicker = () => {
    const [date, setDate] = useState<Date | undefined>()

    return (
        <>
            <button
                popoverTarget="rdp-popover"
                className="input input-border"
                style={{ anchorName: "--rdp" } as React.CSSProperties}
            >
                {date ? formatDate(date) : "Pick a date"}
            </button>
            <div
                popover="auto"
                id="rdp-popover"
                className="dropdown"
                style={{ positionAnchor: "--rdp" } as React.CSSProperties}
            >
                <DayPicker
                    className="react-day-picker"
                    mode="single"
                    selected={date}
                    onSelect={setDate}
                />
            </div>
        </>
    )
}

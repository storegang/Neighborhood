/**
 * Array of month names used for formatting dates.
 */
const monthNames = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
]

/**
 * Relative time formatter for displaying time differences in a human-readable format.
 */
const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" })

/**
 * Checks if two dates are on the same calendar day.
 *
 * @param d1 - The first date to compare.
 * @param d2 - The second date to compare.
 * @returns `true` if the dates are on the same day, otherwise `false`.
 */
function isSameDay(d1: Date, d2: Date): boolean {
    return (
        d1.getFullYear() === d2.getFullYear() &&
        d1.getMonth() === d2.getMonth() &&
        d1.getDate() === d2.getDate()
    )
}

/**
 * Adds a specified number of days to a given date.
 *
 * @param date - The date to modify.
 * @param days - The number of days to add.
 * @returns A new `Date` object with the added days.
 */
function addDays(date: Date, days: number): Date {
    const copy = new Date(date)
    copy.setDate(copy.getDate() + days)
    return copy
}

/**
 * Formats a date object into a time string (e.g., "02:30 PM").
 *
 * @param date - The date to format.
 * @returns A string representing the time in "hh:mm AM/PM" format.
 */
function formatTime(date: Date): string {
    return date.toLocaleTimeString("en-US", {
        hour: "2-digit",
        minute: "2-digit",
    })
}

/**
 * Formats a date object into a string with time included.
 *
 * @param date - The date to format.
 * @returns A string representing the date and time (e.g., "Apr 23, 2:30 PM").
 */
function formatWithTime(date: Date): string {
    const now = new Date()
    const includeYear = date.getFullYear() < now.getFullYear()

    const options: Intl.DateTimeFormatOptions = {
        day: "numeric",
        month: "short",
        hour: "2-digit",
        minute: "2-digit",
        ...(includeYear && { year: "numeric" }),
    }

    return date.toLocaleDateString("en-US", options)
}

/**
 * Formats a date object into a human-readable string.
 *
 * @param date - The date to format.
 * @returns A string representing the date (e.g., "April 23" or "April 23, 2024").
 */
export function formatDate(date: Date): string {
    const day = date.getDate()
    const month = monthNames[date.getMonth()]
    const year = date.getFullYear()
    const currentYear = new Date().getFullYear()

    return year === currentYear ? `${month} ${day}` : `${month} ${day}, ${year}`
}

/**
 * Formats a date object into a relative time string (e.g., "2 hours ago", "just now").
 *
 * @param date - The date to format.
 * @returns A string representing the relative time or a formatted date and time.
 */
export function formatRelativeDate(date: Date): string {
    const now = new Date()
    const diff = now.getTime() - date.getTime()

    const seconds = Math.floor(diff / 1000)
    const minutes = Math.floor(seconds / 60)
    const hours = Math.floor(minutes / 60)

    const isTomorrow = isSameDay(date, addDays(now, 1))

    if (diff >= 0 && seconds < 60) return "just now"
    if (diff >= 0 && minutes < 60) return rtf.format(-minutes, "minute")
    if (diff >= 0 && hours < 24) return rtf.format(-hours, "hour")

    const isToday = isSameDay(date, now)
    if (isToday) return `today ${formatTime(date)}`
    if (isTomorrow) return `tomorrow ${formatTime(date)}`

    return formatWithTime(date)
}

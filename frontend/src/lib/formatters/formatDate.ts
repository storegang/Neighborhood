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
 * Formats a date with "dd.mm" format, and includes year only if it's not the current year.
 * @param date The value to format
 * @returns The value in "dd.mm" or "dd.mm.yyyy" format
 */
export function formatDate(date: Date): string {
    const day = date.getDate()
    const month = monthNames[date.getMonth()]
    const year = date.getFullYear()
    const currentYear = new Date().getFullYear()

    return year === currentYear ? `${month} ${day}` : `${month} ${day}, ${year}`
}

const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" })

/**
 * Formats a date based on how much time has passed since the given date.
 * If the date is less than 24 hours ago, it shows "X hours ago", "X minutes ago", "just now", etc.
 * Otherwise, it formats the date in "dd.mm" format, adding the year only if it's not the current year.
 * @param date The date to format
 * @returns The formatted date string
 */
export function formatRelativeDate(date: Date): string {
    const now = new Date()
    const diff = now.getTime() - date.getTime()

    const seconds = Math.floor(diff / 1000)
    const minutes = Math.floor(seconds / 60)
    const hours = Math.floor(minutes / 60)
    const days = Math.floor(hours / 24)

    if (days >= 1) return formatDate(date)
    if (hours >= 1) return rtf.format(-hours, "hour")
    if (minutes >= 1) return rtf.format(-minutes, "minute")
    if (seconds >= 1) return rtf.format(-seconds, "second")
    return "just now"
}

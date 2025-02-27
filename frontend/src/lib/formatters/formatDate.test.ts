import { describe, it, expect } from "vitest"
import { formatDate, formatRelativeDate } from "./formatDate"

describe("formatDate", () => {
    it("should format date without year if it is the current year", () => {
        const date = new Date()
        const formattedDate = formatDate(date)
        const expected = `${date.toLocaleString("default", { month: "long" })} ${date.getDate()}`
        expect(formattedDate).toBe(expected)
    })

    it("should format date with year if it is not the current year", () => {
        const date = new Date("2020-01-01")
        const formattedDate = formatDate(date)
        const expected = `January 1, 2020`
        expect(formattedDate).toBe(expected)
    })
})

describe("formatRelativeDate", () => {
    it("should format date as 'just now' if it is less than a minute ago", () => {
        const date = new Date()
        const formattedDate = formatRelativeDate(date)
        expect(formattedDate).toBe("just now")
    })

    it("should format date as 'X minutes ago' if it is less than an hour ago", () => {
        const date = new Date()
        date.setMinutes(date.getMinutes() - 5)
        const formattedDate = formatRelativeDate(date)
        expect(formattedDate).toBe("5 minutes ago")
    })

    it("should format date as 'X hours ago' if it is less than a day ago", () => {
        const date = new Date()
        date.setHours(date.getHours() - 5)
        const formattedDate = formatRelativeDate(date)
        expect(formattedDate).toBe("5 hours ago")
    })

    it("should format date as 'dd.mm' if it is more than a day ago", () => {
        const date = new Date("2020-01-01")
        const formattedDate = formatRelativeDate(date)
        expect(formattedDate).toBe("January 1, 2020")
    })
})

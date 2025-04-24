import { describe, it, expect, vi } from "vitest"
import { formatDate, formatRelativeDate } from "./formatDate"
import { afterEach, beforeEach } from "node:test"

describe("formatDate", () => {
    it("formats date without year when in current year", () => {
        const date = new Date("2025-04-23T10:00:00")
        vi.setSystemTime(new Date("2025-04-23"))
        expect(formatDate(date)).toBe("April 23")
    })

    it("formats date with year when not in current year", () => {
        const date = new Date("2023-04-23T10:00:00")
        vi.setSystemTime(new Date("2025-04-23"))
        expect(formatDate(date)).toBe("April 23, 2023")
    })
})

describe("formatRelativeDate", () => {
    beforeEach(() => {
        vi.useFakeTimers()
        vi.setSystemTime(new Date("2025-04-23T12:00:00"))
    })

    afterEach(() => {
        vi.useRealTimers()
    })

    it('returns "just now" if under 60 seconds ago', () => {
        const date = new Date(2025, 3, 23, 11, 59, 45)
        vi.setSystemTime(new Date(2025, 3, 23, 12, 0, 0))
        expect(formatRelativeDate(date)).toBe("just now")
    })

    it('returns "x minutes ago" if under 60 minutes ago', () => {
        const date = new Date("2025-04-23T11:30:00")
        expect(formatRelativeDate(date)).toMatch(/30 minutes? ago/)
    })

    it('returns "x hours ago" if under 24 hours ago', () => {
        const date = new Date("2025-04-23T09:00:00")
        expect(formatRelativeDate(date)).toMatch(/3 hours? ago/)
    })

    it('returns "x hours ago" if earlier today and over 1 hour ago', () => {
        const date = new Date(2025, 3, 23, 6, 0, 0)
        vi.setSystemTime(new Date(2025, 3, 23, 12, 0, 0))
        const result = formatRelativeDate(date)
        expect(result).toBe("6 hours ago")
    })

    it('returns "tomorrow hh:mm" for dates tomorrow', () => {
        const date = new Date("2025-04-24T08:30:00")
        expect(formatRelativeDate(date)).toMatch(/^tomorrow/)
    })

    it("returns formatted date for future dates not tomorrow", () => {
        const date = new Date("2025-04-28T14:00:00")
        expect(formatRelativeDate(date)).toMatch(/Apr 28,? \d{1,2}:\d{2}/)
    })

    it("returns formatted date for past days (more than 24h ago)", () => {
        const date = new Date("2025-04-20T14:00:00")
        expect(formatRelativeDate(date)).toMatch(/Apr 20,? \d{1,2}:\d{2}/)
    })
})

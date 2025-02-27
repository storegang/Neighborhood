import { describe, expect, it } from "vitest"
import { formatPhoneNumber } from "./formatPhoneNumber"

const nbsp = "\u00A0"

describe("formatPhoneNumber", () => {
    it("formats mobile numbers correctly", () => {
        expect(formatPhoneNumber("98651731")).toEqual(
            `98${nbsp}65${nbsp}17${nbsp}31`
        )
        expect(formatPhoneNumber("48435298")).toEqual(
            `48${nbsp}43${nbsp}52${nbsp}98`
        )
    })

    it("formats 800-numbers correctly", () => {
        expect(formatPhoneNumber("81549300")).toEqual(`815${nbsp}49${nbsp}300`)
    })

    it("formats landline numbers correctly", () => {
        expect(formatPhoneNumber("22438634")).toEqual(
            `22${nbsp}43${nbsp}86${nbsp}34`
        )
    })

    it("ignores spaces in otherwise valid inputs", () => {
        expect(formatPhoneNumber("22 4386 3 4")).toEqual(
            `22${nbsp}43${nbsp}86${nbsp}34`
        )
    })

    it("does not format inputs with under 8 digits", () => {
        expect(formatPhoneNumber("2243863")).toEqual(`2243863`)
        expect(formatPhoneNumber("2 243 863")).toEqual(`2 243 863`)
    })

    it("does not format inputs with over 8 digits", () => {
        expect(formatPhoneNumber("224386345")).toEqual(`224386345`)
        expect(formatPhoneNumber("2 243 863 45")).toEqual(`2 243 863 45`)
    })

    it("does not format inputs with non-digit characters", () => {
        expect(formatPhoneNumber("224386dsf5")).toEqual(`224386dsf5`)
        expect(formatPhoneNumber("2 243 sdf 45")).toEqual(`2 243 sdf 45`)
    })

    it("formats number with country code", () => {
        expect(formatPhoneNumber("81549300", { countryCode: "47" })).toEqual(
            `+47${nbsp}815${nbsp}49${nbsp}300`
        )
        expect(formatPhoneNumber("22438634", { countryCode: "47" })).toEqual(
            `+47${nbsp}22${nbsp}43${nbsp}86${nbsp}34`
        )
        expect(formatPhoneNumber("81549300", { countryCode: "354" })).toEqual(
            `+354${nbsp}815${nbsp}49${nbsp}300`
        )
        expect(formatPhoneNumber("22438634", { countryCode: "354" })).toEqual(
            `+354${nbsp}22${nbsp}43${nbsp}86${nbsp}34`
        )
    })
})

describe("formatPhoneNumber with partial option", () => {
    it("formats mobile numbers correctly", () => {
        expect(formatPhoneNumber("9865", { partial: true })).toEqual(
            `98${nbsp}65`
        )
        expect(formatPhoneNumber("986517", { partial: true })).toEqual(
            `98${nbsp}65${nbsp}17`
        )
        expect(formatPhoneNumber("98651731", { partial: true })).toEqual(
            `98${nbsp}65${nbsp}17${nbsp}31`
        )
        expect(formatPhoneNumber("4843", { partial: true })).toEqual(
            `48${nbsp}43`
        )
        expect(formatPhoneNumber("484352", { partial: true })).toEqual(
            `48${nbsp}43${nbsp}52`
        )
        expect(formatPhoneNumber("48435298", { partial: true })).toEqual(
            `48${nbsp}43${nbsp}52${nbsp}98`
        )
    })

    it("formats 800-numbers correctly", () => {
        expect(formatPhoneNumber("8154", { partial: true })).toEqual(
            `815${nbsp}4`
        )
        expect(formatPhoneNumber("815493", { partial: true })).toEqual(
            `815${nbsp}49${nbsp}3`
        )
        expect(formatPhoneNumber("81549300", { partial: true })).toEqual(
            `815${nbsp}49${nbsp}300`
        )
    })

    it("formats landline numbers correctly", () => {
        expect(formatPhoneNumber("224", { partial: true })).toEqual(
            `22${nbsp}4`
        )
        expect(formatPhoneNumber("22438", { partial: true })).toEqual(
            `22${nbsp}43${nbsp}8`
        )
        expect(formatPhoneNumber("2243863", { partial: true })).toEqual(
            `22${nbsp}43${nbsp}86${nbsp}3`
        )
        expect(formatPhoneNumber("22438634", { partial: true })).toEqual(
            `22${nbsp}43${nbsp}86${nbsp}34`
        )
    })
})

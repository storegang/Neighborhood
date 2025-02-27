/**
 * Regular expressions for matching different types of phone numbers.
 */
export const PHONE_NUMBER_REGEX = {
    mobil: /^([8]\d{2})(\d{2})(\d{3})$/,
    fast: /^([2-9]\d)(\d{2})(\d{2})(\d{2})$/,
    mobilPartial: /^([8]\d{2})(\d{1,2})?(\d{1,3})?$/,
    fastPartial: /^([2-9]\d)(\d{1,2})?(\d{1,2})?(\d{1,2})?$/,
}

/**
 * Options for formatting phone numbers.
 */
type FormatPhoneNumberOptions = {
    partial?: boolean
    countryCode?: string
}

/**
 * Formats a phone number based on the provided options.
 *
 * @param input - The phone number to format.
 * @param options - Optional settings for formatting the phone number.
 * @returns The formatted phone number or the original input if it doesn't match the expected patterns.
 */
export const formatPhoneNumber = (
    input: string,
    options?: FormatPhoneNumberOptions
) => {
    // Remove all non-word characters from the input
    const strippedInput = input.replace(/\W/g, "")

    const mobilRegex = options?.partial
        ? PHONE_NUMBER_REGEX.mobilPartial
        : PHONE_NUMBER_REGEX.mobil
    const fastRegex = options?.partial
        ? PHONE_NUMBER_REGEX.fastPartial
        : PHONE_NUMBER_REGEX.fast

    const match =
        strippedInput.match(mobilRegex) || strippedInput.match(fastRegex)

    if (!match) {
        return input
    }

    const NON_BREAKING_SPACE = "\u00A0"

    // Build the formatted phone number
    return [
        options?.countryCode ? `+${options.countryCode}` : undefined, // Add country code if provided
        ...match.slice(1), // Add the matched groups from the regex
    ]
        .filter(Boolean) // Remove any undefined elements
        .join(NON_BREAKING_SPACE) // Join the elements with non-breaking spaces
}

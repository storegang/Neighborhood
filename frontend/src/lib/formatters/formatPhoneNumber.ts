/**
 * Regular expressions for matching different types of phone numbers.
 */
export const PHONE_NUMBER_REGEX = {
    mobile: /^([8]\d{2})(\d{2})(\d{3})$/,
    landline: /^([2-9]\d)(\d{2})(\d{2})(\d{2})$/,
    mobilePartial: /^([8]\d{2})(\d{1,2})?(\d{1,3})?$/,
    landlinePartial: /^([2-9]\d)(\d{1,2})?(\d{1,2})?(\d{1,2})?$/,
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
    //
    // Remove all non-word characters from the input
    const strippedInput = input.replace(/\W/g, "")

    const mobileRegex = options?.partial
        ? PHONE_NUMBER_REGEX.mobilePartial
        : PHONE_NUMBER_REGEX.mobile
    const landlineRegex = options?.partial
        ? PHONE_NUMBER_REGEX.landlinePartial
        : PHONE_NUMBER_REGEX.landline

    const match =
        strippedInput.match(mobileRegex) || strippedInput.match(landlineRegex)

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

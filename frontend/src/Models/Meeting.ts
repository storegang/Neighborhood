export type Meeting = {
    id: string
    date: Date
    title: string
    type: "BOARD_MEETING" | "TOWN_HALL"
}

// The API will expose a details endpoint returning a MeetingDetails type, which includes more info, such as the agenda and attachments, if any

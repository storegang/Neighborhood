import { apiFetcher } from "@/fetchers/apiFetcher"
import { Category, Meeting, User } from "@/Models"
import { CategoryRequest, CategoryResponse } from "@/Models/Category"

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @returns All users from the server
 */
export const getUsers = async (accessToken: string): Promise<User[]> => {
    const response = await apiFetcher<{ users: User[] }>({
        path: "/user",
        accessToken: accessToken,
    })
    return response.users
}

/**
 *
 * @param accessToken The accessToken to be used for the request
 * @returns All meetings from the server
 */
export const getMeetings = async (accessToken: string): Promise<Meeting[]> => {
    // Waiting for backend to implement meeting-related endpoints

    const meetingsResponse = new Promise<Meeting[]>((resolve) => {
        resolve([
            {
                id: "1",
                date: new Date("2024-10-01T10:00:00Z"),
                title: "Styremøte angående nye balkonger",
                type: "BOARD_MEETING",
            },
            {
                id: "2",
                date: new Date("2024-10-15T18:00:00Z"),
                title: "Årsmøte i Andeby Sameie",
                type: "TOWN_HALL",
            },
            {
                id: "3",
                date: new Date("2025-02-16T10:00:00Z"),
                title: "Befaring av ny lekeplass",
                type: "TOWN_HALL",
            },
            {
                id: "4",
                date: new Date("2025-03-24T18:00:00Z"),
                title: "Montering av nye balkonger",
                type: "TOWN_HALL",
            },
            {
                id: "5",
                date: new Date("2025-05-12T10:00:00Z"),
                title: "Styremøte angående lån hos Onkel Skrue Bank",
                type: "BOARD_MEETING",
            },
            {
                id: "6",
                date: new Date("2025-06-01T18:00:00Z"),
                title: "Årsmøte i Andeby Sameie",
                type: "TOWN_HALL",
            },
            {
                id: "7",
                date: new Date("2025-07-15T10:00:00Z"),
                title: "Sommerfest for beboere i Andeby Sameie",
                type: "TOWN_HALL",
            },
        ])
    })

    const meetings = await meetingsResponse
    return meetings
}

/**
 *
 * @param accessToken The accesToken to be used for the request
 * @param meeting The meeting to be created
 * @returns The created meeting
 */
export const createMeeting = async (
    accessToken: string,
    meeting: Omit<Meeting, "id">
) => {
    // Waiting for backend to implement meeting-related endpoints

    const response = new Promise<Meeting>((resolve) => {
        resolve({
            id: "1",
            date: meeting.date,
            title: meeting.title,
            type: meeting.type,
        })
    })
}

export const createCategory = async (
    input: CategoryRequest
): Promise<{
    category: CategoryResponse
}> => {
    const response = apiFetcher<{ category: CategoryResponse }>({
        path: "/category",
        method: "POST",
        accessToken: input.accessToken,
        body: {
            name: input.name,
            neighborhoodId: input.neighborhoodId,
            color: "",
            id: "",
        },
    })

    return response
}

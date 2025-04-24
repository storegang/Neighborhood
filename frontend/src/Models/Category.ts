export type CategoryRequest = {
    name: string
    neighborhoodId?: string
    accessToken?: string
}

export type CategoryResponse = {
    id: string
    name: string
    color: string
    neighborhoodId: string
}

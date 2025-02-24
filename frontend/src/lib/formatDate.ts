export const formatDate = (isoString: string): string => {
    const date = new Date(isoString)
    const now = new Date()
    const diffMs = now.getTime() - date.getTime()
    const diffSec = Math.floor(diffMs / 1000)
    const diffMin = Math.floor(diffSec / 60)
    const diffHours = Math.floor(diffMin / 60)
    const diffDays = Math.floor(diffHours / 24)

    if (diffSec < 60) return "nå nettopp"
    if (diffMin < 60) return `for ${diffMin} min siden`
    if (diffHours < 24) return `for ${diffHours} t siden`
    if (diffDays === 1)
        return `i går kl. ${date.toLocaleTimeString("nb-NO", { hour: "2-digit", minute: "2-digit" })}`
    if (diffDays < 7) return `${diffDays} dager siden`

    return new Intl.DateTimeFormat("nb-NO", {
        year: "numeric",
        month: "long",
        day: "numeric",
        hour: "2-digit",
        minute: "2-digit",
    }).format(date)
}

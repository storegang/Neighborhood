"use client"

import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactNode, useState } from "react"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"

export const QueryProvider: React.FC<{ children: ReactNode }> = ({
    children,
}) => {
    const [client] = useState(new QueryClient())
    return (
        <QueryClientProvider client={client}>
            {children}
            <ReactQueryDevtools initialIsOpen={false} />
        </QueryClientProvider>
    )
}

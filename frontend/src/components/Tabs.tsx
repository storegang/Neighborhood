import { Children, cloneElement, isValidElement, ReactNode } from "react"

export const Tabs: React.FC<{
    name: string
    children: ReactNode
}> = ({ name, children }) => {
    const childrenWithName = Children.map(children, (child) => {
        if (isValidElement(child)) {
            /* @ts-expect-error The name-attribute should always be passed to Tabs-children */
            return cloneElement(child, { name })
        }

        return child
    })

    return (
        <div className="tabs tabs-border bg-base-100 w-full">
            {childrenWithName}
        </div>
    )
}

type TabPanelProps = {
    children: React.ReactNode
    name?: string
    label: string
    defaultChecked?: boolean
}

export const TabPanel: React.FC<TabPanelProps> = ({
    children,
    name,
    label,
    defaultChecked,
}) => {
    return (
        <>
            <input
                type="radio"
                name={name}
                className="tab"
                aria-label={label}
                defaultChecked={defaultChecked}
            />
            <div className="tab-content border-base-300 bg-base-100 p-6">
                {children}
            </div>
        </>
    )
}

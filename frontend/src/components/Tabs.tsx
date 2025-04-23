import { Children, cloneElement, isValidElement, ReactElement } from "react"

export const Tabs: React.FC<{
    name: string
    children: ReactElement<TabPanelProps>[]
}> = ({ name, children }) => {
    const childrenWithName = Children.map(children, (child) => {
        if (isValidElement(child)) {
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
}

export const TabPanel: React.FC<TabPanelProps> = ({
    children,
    name,
    label,
}) => {
    return (
        <>
            <input
                type="radio"
                name={name}
                className="tab"
                aria-label={label}
                defaultChecked
            />
            <div className="tab-content border-base-300 bg-base-100 p-6">
                {children}
            </div>
        </>
    )
}

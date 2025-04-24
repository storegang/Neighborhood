import { CategoryResponse } from "@/Models"

type CategoriesListProps = {
    selectedCategory: CategoryResponse | null
    onSelectCategory: (category: CategoryResponse | null) => void
    categories: CategoryResponse[]
}

export const CategoriesList: React.FC<CategoriesListProps> = ({
    selectedCategory,
    onSelectCategory,
    categories,
}) => {
    return (
        <article className="hidden sm:block">
            <h2 className="text-lg font-semibold">Categories</h2>
            <ul className="mt-2 flex flex-col gap-2 space-y-1">
                {categories.length > 0 ? (
                    categories.map((category) => (
                        <CategoriesListItem
                            key={category.id}
                            category={category}
                            selectedCategory={selectedCategory}
                            onSelectCategory={onSelectCategory}
                        />
                    ))
                ) : (
                    <p className="text-sm text-gray-500">
                        No categories available
                    </p>
                )}
            </ul>
        </article>
    )
}

export const MobileCategoriesList: React.FC<CategoriesListProps> = ({
    selectedCategory,
    onSelectCategory,
    categories,
}) => {
    return (
        <div
            tabIndex={0}
            className="collapse-arrow bg-base-100 border-base-300 collapse mb-4 border sm:hidden"
        >
            <input type="checkbox" />
            <div className="collapse-title font-semibold">Categories</div>
            <div className="collapse-content text-sm">
                <ul className="mt-2 flex flex-col gap-2 space-y-1">
                    {categories.length > 0 ? (
                        categories.map((category) => (
                            <CategoriesListItem
                                key={category.id}
                                category={category}
                                selectedCategory={selectedCategory}
                                onSelectCategory={onSelectCategory}
                            />
                        ))
                    ) : (
                        <p className="text-sm text-gray-500">
                            No categories available
                        </p>
                    )}
                </ul>
            </div>
        </div>
    )
}

const CategoriesListItem = ({
    category,
    selectedCategory,
    onSelectCategory,
}: {
    category: CategoryResponse
    selectedCategory: CategoryResponse | null
    onSelectCategory: (category: CategoryResponse | null) => void
}) => {
    return (
        <li
            key={category.id}
            className={`badge h-fit cursor-pointer ${
                selectedCategory === category ? "badge-neutral" : "badge-soft"
            }`}
            onClick={() =>
                onSelectCategory(
                    category.id === selectedCategory?.id ? null : category
                )
            }
        >
            {category.name}
        </li>
    )
}

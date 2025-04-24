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
        <article>
            <aside>
                <h2 className="text-lg font-semibold">Kategorier</h2>
                <ul className="mt-2 flex flex-col gap-2 space-y-1">
                    {categories.length > 0 ? (
                        categories.map((category) => (
                            <li
                                key={category.id}
                                className={`badge badge-neutral h-fit w-fit cursor-pointer ${
                                    selectedCategory === category
                                        ? "badge-primary"
                                        : ""
                                }`}
                                onClick={() =>
                                    onSelectCategory(
                                        category.id === selectedCategory?.id
                                            ? null
                                            : category
                                    )
                                }
                            >
                                {category.name}
                            </li>
                        ))
                    ) : (
                        <p className="text-sm text-gray-500">
                            No categories available
                        </p>
                    )}
                </ul>
            </aside>
        </article>
    )
}

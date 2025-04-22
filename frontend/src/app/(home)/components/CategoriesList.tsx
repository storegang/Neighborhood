import { Category } from "@/Models"

type CategoriesListProps = {
    selectedCategory: Category | null
    onSelectCategory: (category: Category | null) => void
    categories: Category[]
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
                                className={`badge badge-soft badge-secondary h-fit w-fit cursor-pointer ${
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
                            Ingen kategorier
                        </p>
                    )}
                </ul>
            </aside>
        </article>
    )
}
